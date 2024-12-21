using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Represents a unit in the combat system, including its base stats, current stats, and combat-related behaviors.
/// This class implements the IComparable interface to allow sorting based on speed.
/// </summary>
public class CombatUnit : MonoBehaviour, IComparable
{
    [Header("Base Stats")]
    private UnitBase unitBase; // The base data for the unit.
    private Sprite idleSprite; // The idle sprite of the unit.
    [SerializeField] private int level; // The level of the unit.

    // Combat tracking stats:
    [Header("Stat Inspection")]
    [SerializeField] private float currentHp; // The current health points of the unit.
    [SerializeField] private float currentMaxHp; // The current maximum health points of the unit.
    [SerializeField] private float currentStrength; // The current strength of the unit.
    [SerializeField] private float currentAgility; // The current agility of the unit.
    [SerializeField] private float currentIntellect; // The current intellect of the unit.
    [SerializeField] private float currentAttackPower; // The current attack power of the unit.
    [SerializeField] private float currentAbilityPower; // The current ability power of the unit.
    [SerializeField] private float currentPhysicalCritChance; // The current physical critical hit chance of the unit.
    [SerializeField] private float currentMagicalCritChance; // The current magical critical hit chance of the unit.
    [SerializeField] private float currentPhysDef; // The current physical defense of the unit.
    [SerializeField] private float currentMagicDef; // The current magical defense of the unit.
    [SerializeField] private float currentPhysicalMitigation; // The current physical mitigation of the unit.
    [SerializeField] private float currentPhysicalBlock; // The current physical block of the unit.
    [SerializeField] private float currentMagicalMitigation; // The current magical mitigation of the unit.
    [SerializeField] private float currentDodge; // The current dodge chance of the unit.
    [SerializeField] private float currentSpeed; // The current speed of the unit.
    [SerializeField] private bool isAlive; // Whether the unit is alive.

    private List<CombatMoveType> acceptedDamageTypes = new List<CombatMoveType>
        { CombatMoveType.Physical, CombatMoveType.Magical, CombatMoveType.Suffer }; // The types of damage the unit can take.

    private TalentManager talentManager; // The manager for handling talents and abilities.
    private CombatEffectManager combatEffectsManager; // The manager for handling combat effects.

    /// <summary>
    /// Initializes the CombatUnit by finding the TalentManager and CombatEffectManager components.
    /// </summary>
    private void Awake()
    {
        talentManager = GetComponent<TalentManager>();
        combatEffectsManager = GetComponent<CombatEffectManager>();
    }

    /// <summary>
    /// Initializes the unit with base stats and level.
    /// </summary>
    /// <param name="unitbase">The base data for the unit.</param>
    /// <param name="spawnLevel">The level at which the unit is spawned.</param>
    public void InitiateUnit(UnitBase unitbase, int spawnLevel)
    {
        Level = spawnLevel;
        this.unitBase = unitbase;

        CurrentHp = MaxHp;
        CurrentMaxHp = MaxHp;
        CurrentStrength = Strength;
        CurrentAgility = Agility;
        CurrentIntellect = Intellect;
        CurrentAttackPower = AttackPower;
        CurrentAbilityPower = AbilityPower;
        CurrentPhysicalCritChance = PhysicalCritChance;
        CurrentMagicalCritChance = MagicalCritChance;
        CurrentPhysDef = PhysicalDefense;
        CurrentMagicDef = MagicalDefense;
        CurrentPhysicalMitigation = 0;
        CurrentPhysicalBlock = 0;
        CurrentMagicalMitigation = 0;
        CurrentDodge = DodgeChance;
        CurrentSpeed = Speed;
    }

    /// <summary>
    /// Calculates the damage done by the unit based on the given move.
    /// </summary>
    /// <param name="move">The combat move to calculate damage for.</param>
    /// <returns>The calculated damage.</returns>
    public float DoDamage(CombatMove move)
    {
        if (!acceptedDamageTypes.Contains(move.GetType()))
        {
            Debug.Log("Attempted to damage with wrong type: " + move.GetType());
            return 0;
        }

        float power = move.GetPower();
        float finalDamage = power;
        float critCounter = Random.Range(0, 100);

        Debug.Log("Chance it's a crit: " + critCounter);

        switch (move.GetType())
        {
            case CombatMoveType.Physical:
                float damageAfterAttackPower = power + CurrentAttackPower;

                finalDamage = critCounter < (CurrentPhysicalCritChance * 100) ? damageAfterAttackPower * unitBase.CritMultiplier : damageAfterAttackPower;
                break;
            case CombatMoveType.Magical:
                float damageAfterAbilityPower = power + CurrentAbilityPower;

                finalDamage = critCounter < (CurrentMagicalCritChance * 100) ? damageAfterAbilityPower * unitBase.CritMultiplier : damageAfterAbilityPower;
                break;
        }

        return finalDamage;
    }

    /// <summary>
    /// Applies damage to the unit based on the given damage and move type.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    /// <param name="moveType">The type of damage (e.g., Physical, Magical).</param>
    /// <returns>A TakeDamageResult containing the result of the damage application.</returns>
    public TakeDamageResult TakeDamage(float damage, CombatMoveType moveType)
    {
        if (!acceptedDamageTypes.Contains(moveType))
        {
            Debug.Log("Attempted to damage with wrong type: " + moveType);
            return null;
        }

        switch (moveType)
        {
            case CombatMoveType.Physical:
                {
                    /*
                     * Check if attack is dodged.
                     * Mitigate total damage of skill with percentage (e.g. 10%, 25%) and round -- // 11 * (1 - 0.1) = 9.9 -> 10;
                     * Block damage of skill by a flat amount, decided by currentPhysicalBlock (if any was applied) times the block power of unit. -- // 10 - (2 * 1) = 8;
                     * Subtract physical armor from damage of skill -- // 5 - 2 = 3;
                     * Take damage
                     * Mathf.Round to round to nearest whole number
                     * Mathf.Clamp to avoid subtracting negative numbers
                     * Damage is returned rounded to int for UI purposes
                     */
                    float hitChance = Random.Range(0, 100);

                    Debug.Log("Hit chance: " + hitChance);
                    Debug.Log("Dodge chance: " + CurrentDodge);
                    if (hitChance < CurrentDodge * 100)
                    {
                        Debug.Log("Attack was dodged");
                        return new TakeDamageResult(this, false, 0);
                    }
                    else
                    {
                        Debug.Log("initial damage: " + damage);

                        // Calculate mitigation
                        // Mitigation should be active effect
                        float damageAfterMitigation = damage;
                        if (combatEffectsManager.IsEffectActive(CombatEffectType.PhysMitigation))
                        {
                            damageAfterMitigation = talentManager.CalculatePhysicalMitigation(damage);
                        }

                        // Calculate block
                        // Only apply if block is active on unit, if not skip.
                        float damageAfterBlock = damageAfterMitigation;
                        if (combatEffectsManager.IsEffectActive(CombatEffectType.Block))
                        {
                            damageAfterBlock = talentManager.CalculateBlock(damageAfterMitigation);
                        }

                        // Calculate armor
                        float damageAfterArmor = talentManager.CalculateArmor(damageAfterBlock);

                        // Round to nearest int.
                        int finalDamageAfterArmor = Mathf.RoundToInt(damageAfterArmor);

                        Debug.Log("Damage taken: " + finalDamageAfterArmor);
                        CurrentHp -= finalDamageAfterArmor;

                        return new TakeDamageResult(this, CurrentHp <= 0, finalDamageAfterArmor);
                    }
                }
            case CombatMoveType.Magical:
                {
                    int damageAfterMitigation = Mathf.RoundToInt(damage * (1 - CurrentMagicalMitigation));
                    float damageAfterArmor = Mathf.Clamp(
                        (damageAfterMitigation - CurrentMagicDef),
                        0, float.MaxValue);
                    int finalDamageAfterArmor = Mathf.RoundToInt(damageAfterArmor);

                    CurrentHp -= finalDamageAfterArmor;

                    return new TakeDamageResult(this, CurrentHp <= 0, finalDamageAfterArmor);
                }
            // Suffer Damage
            default:
                CurrentHp -= Mathf.RoundToInt(damage);

                return new TakeDamageResult(this, CurrentHp <= 0, Mathf.RoundToInt(damage));
        }
    }

    /// <summary>
    /// Heals the unit by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(float amount)
    {
        if ((CurrentHp + amount) > CurrentMaxHp)
        {
            CurrentHp = CurrentMaxHp;
        }
        else
        {
            CurrentHp += amount;
            CurrentHp = Mathf.Round(CurrentHp);
        }
    }

    /// <summary>
    /// Compares this CombatUnit to another CombatUnit based on their speed.
    /// </summary>
    /// <param name="obj">The object to compare to (must be a CombatUnit).</param>
    /// <returns>An integer indicating the relative order of the CombatUnits.</returns>
    public int CompareTo(object obj)
    {
        CombatUnit other = obj as CombatUnit;
        return other.CurrentSpeed.CompareTo(this.CurrentSpeed);
    }

    /// <summary>
    /// Gets or sets the level of the unit.
    /// </summary>
    public int Level
    {
        get => level;
        set => level = value;
    }

    /// <summary>
    /// Gets the maximum health points of the unit.
    /// </summary>
    public int MaxHp
    {
        get => Mathf.RoundToInt(unitBase.MaxHp + (unitBase.MaxHpGrowth * (level - 1)));
    }

    /// <summary>
    /// Gets the strength of the unit.
    /// </summary>
    public float Strength
    {
        get => unitBase.IsPlayerUnit ? unitBase.Strength : unitBase.Strength + (unitBase.StrengthGrowth * (level - 1));
    }

    /// <summary>
    /// Gets the agility of the unit.
    /// </summary>
    public float Agility
    {
        get => unitBase.IsPlayerUnit ? unitBase.Agility : unitBase.Agility + (unitBase.AgilityGrowth * (level - 1));
    }

    /// <summary>
    /// Gets the intellect of the unit.
    /// </summary>
    public float Intellect
    {
        get => unitBase.IsPlayerUnit ? unitBase.Intellect : unitBase.Intellect + (unitBase.Intellect * (level - 1));
    }

    /// <summary>
    /// Gets the attack power of the unit.
    /// </summary>
    public float AttackPower
    {
        get => unitBase.AttackPower
               + (unitBase.Strength * unitBase.StrengthApRatio)
               + (unitBase.Agility * unitBase.AgilityApRatio);

    }

    /// <summary>
    /// Gets the ability power of the unit.
    /// </summary>
    public float AbilityPower
    {
        get => unitBase.AbilityPower
               + (unitBase.Intellect * unitBase.IntellectAbpRatio);
    }

    /// <summary>
    /// Gets the physical critical hit chance of the unit.
    /// </summary>
    public float PhysicalCritChance
    {
        get => unitBase.PhysicalCritChance
               + (unitBase.Agility * unitBase.AgilityCritRatio) / 100;
    }

    /// <summary>
    /// Gets the magical critical hit chance of the unit.
    /// </summary>
    public float MagicalCritChance
    {
        get => unitBase.MagicalCritChance
               + (unitBase.Intellect * unitBase.IntellectCritRatio) / 100;
    }

    /// <summary>
    /// Gets the physical defense of the unit.
    /// </summary>
    public float PhysicalDefense
    {
        get =>
            unitBase.PhysicalDefense
               + (unitBase.PhysicalDefenseGrowth * (level - 1))
               + (unitBase.Strength * unitBase.StrengthPhysDefRatio)
               + (unitBase.Agility * unitBase.AgilityPhysDefRatio);
    }

    /// <summary>
    /// Gets the magical defense of the unit.
    /// </summary>
    public float MagicalDefense
    {
        get => unitBase.MagicalDefense
               + (unitBase.MagicalDefenseGrowth * (level - 1));
    }

    /// <summary>
    /// Gets the physical block power of the unit.
    /// </summary>
    public float PhysicalBlockPower
    {
        get => unitBase.PhysicalBlockPower
               + (unitBase.PhysicalBlockPowerGrowth * (level - 1));
    }

    /// <summary>
    /// Gets the dodge chance of the unit.
    /// </summary>
    public float DodgeChance
    {
        get => unitBase.DodgeChance
               + (unitBase.Agility * unitBase.AgilityDodgeRatio) / 100;
    }

    /// <summary>
    /// Gets the speed of the unit.
    /// </summary>
    public float Speed
    {
        get => unitBase.Speed
               + (unitBase.SpeedGrowth * (level - 1))
               + (unitBase.Agility * unitBase.AgilitySpeedRatio);
    }

    /// <summary>
    /// Gets or sets the name of the unit.
    /// </summary>
    public string UnitName
    {
        get => unitBase.UnitName;
        set => unitBase.UnitName = value;
    }

    /// <summary>
    /// Gets or sets the description of the unit.
    /// </summary>
    public string Description
    {
        get => unitBase.Description;
        set => unitBase.Description = value;
    }

    /// <summary>
    /// Gets or sets the type of the unit (e.g., Player, Enemy).
    /// </summary>
    public UnitType UnitType
    {
        get => unitBase.UnitType;
        set => unitBase.UnitType = value;
    }

    /// <summary>
    /// Gets or sets whether the unit is a player unit.
    /// </summary>
    public bool IsPlayerUnit
    {
        get => unitBase.IsPlayerUnit;
        set => unitBase.IsPlayerUnit = value;
    }

    /// <summary>
    /// Gets or sets the idle sprite of the unit.
    /// </summary>
    public Sprite IdleSprite
    {
        get => unitBase.IdleSprite;
        set => unitBase.IdleSprite = value;
    }

    /// <summary>
    /// Gets or sets the strength-to-attack power ratio of the unit.
    /// </summary>
    public float StrengthApRatio
    {
        get => unitBase.StrengthApRatio;
        set => unitBase.StrengthApRatio = value;
    }

    /// <summary>
    /// Gets or sets the agility-to-attack power ratio of the unit.
    /// </summary>
    public float AgilityApRatio
    {
        get => unitBase.AgilityApRatio;
        set => unitBase.AgilityApRatio = value;
    }

    /// <summary>
    /// Gets or sets the intellect-to-ability power ratio of the unit.
    /// </summary>
    public float IntellectAbpRatio
    {
        get => unitBase.IntellectAbpRatio;
        set => unitBase.IntellectAbpRatio = value;
    }

    /// <summary>
    /// Gets or sets the agility-to-critical hit ratio of the unit.
    /// </summary>
    public float AgilityCritRatio
    {
        get => unitBase.AgilityCritRatio;
        set => unitBase.AgilityCritRatio = value;
    }

    /// <summary>
    /// Gets or sets the intellect-to-critical hit ratio of the unit.
    /// </summary>
    public float IntellectCritRatio
    {
        get => unitBase.IntellectCritRatio;
        set => unitBase.IntellectCritRatio = value;
    }

    /// <summary>
    /// Gets or sets the strength-to-physical defense ratio of the unit.
    /// </summary>
    public float StrengthPhysDefRatio
    {
        get => unitBase.StrengthPhysDefRatio;
        set => unitBase.StrengthPhysDefRatio = value;
    }

    /// <summary>
    /// Gets or sets the agility-to-physical defense ratio of the unit.
    /// </summary>
    public float AgilityPhysDefRatio
    {
        get => unitBase.AgilityPhysDefRatio;
        set => unitBase.AgilityPhysDefRatio = value;
    }

    /// <summary>
    /// Gets or sets the agility-to-dodge ratio of the unit.
    /// </summary>
    public float AgilityDodgeRatio
    {
        get => unitBase.AgilityDodgeRatio;
        set => unitBase.AgilityDodgeRatio = value;
    }

    /// <summary>
    /// Gets or sets the agility-to-speed ratio of the unit.
    /// </summary>
    public float AgilitySpeedRatio
    {
        get => unitBase.AgilitySpeedRatio;
        set => unitBase.AgilitySpeedRatio = value;
    }

    /// <summary>
    /// Gets or sets the maximum health points growth of the unit.
    /// </summary>
    public float MaxHpGrowth
    {
        get => unitBase.MaxHpGrowth;
        set => unitBase.MaxHpGrowth = value;
    }

    /// <summary>
    /// Gets or sets the strength growth of the unit.
    /// </summary>
    public float StrengthGrowth
    {
        get => unitBase.StrengthGrowth;
        set => unitBase.StrengthGrowth = value;
    }

    /// <summary>
    /// Gets or sets the agility growth of the unit.
    /// </summary>
    public float AgilityGrowth
    {
        get => unitBase.AgilityGrowth;
        set => unitBase.AgilityGrowth = value;
    }

    /// <summary>
    /// Gets or sets the intellect growth of the unit.
    /// </summary>
    public float IntellectGrowth
    {
        get => unitBase.IntellectGrowth;
        set => unitBase.IntellectGrowth = value;
    }

    /// <summary>
    /// Gets or sets the physical defense growth of the unit.
    /// </summary>
    public float PhysicalDefenseGrowth
    {
        get => unitBase.PhysicalDefenseGrowth;
        set => unitBase.PhysicalDefenseGrowth = value;
    }

    /// <summary>
    /// Gets or sets the magical defense growth of the unit.
    /// </summary>
    public float MagicalDefenseGrowth
    {
        get => unitBase.MagicalDefenseGrowth;
        set => unitBase.MagicalDefenseGrowth = value;
    }

    /// <summary>
    /// Gets or sets the physical block power growth of the unit.
    /// </summary>
    public float PhysicalBlockPowerGrowth
    {
        get => unitBase.PhysicalBlockPowerGrowth;
        set => unitBase.PhysicalBlockPowerGrowth = value;
    }

    /// <summary>
    /// Gets or sets the speed growth of the unit.
    /// </summary>
    public float SpeedGrowth
    {
        get => unitBase.SpeedGrowth;
        set => unitBase.SpeedGrowth = value;
    }

    /// <summary>
    /// Gets or sets the current health points of the unit.
    /// </summary>
    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = value;
    }

    /// <summary>
    /// Gets or sets the current maximum health points of the unit.
    /// </summary>
    public float CurrentMaxHp
    {
        get => currentMaxHp;
        set => currentMaxHp = value;
    }

    /// <summary>
    /// Gets or sets the current strength of the unit.
    /// </summary>
    public float CurrentStrength
    {
        get => currentStrength;
        set => currentStrength = value;
    }

    /// <summary>
    /// Gets or sets the current agility of the unit.
    /// </summary>
    public float CurrentAgility
    {
        get => currentAgility;
        set => currentAgility = value;
    }

    /// <summary>
    /// Gets or sets the current intellect of the unit.
    /// </summary>
    public float CurrentIntellect
    {
        get => currentIntellect;
        set => currentIntellect = value;
    }

    /// <summary>
    /// Gets or sets the current attack power of the unit.
    /// </summary>
    public float CurrentAttackPower
    {
        get => currentAttackPower * combatEffectsManager.GetEffectMultiplier(CombatEffectType.Strengthen) * (combatEffectsManager.GetEffectMultiplier(CombatEffectType.Weaken));
        set => currentAttackPower = value;
    }

    /// <summary>
    /// Gets or sets the current ability power of the unit.
    /// </summary>
    public float CurrentAbilityPower
    {
        get => currentAbilityPower;
        set => currentAbilityPower = value;
    }

    /// <summary>
    /// Gets or sets the current physical critical hit chance of the unit.
    /// </summary>
    public float CurrentPhysicalCritChance
    {
        get => currentPhysicalCritChance;
        set => currentPhysicalCritChance = value;
    }

    /// <summary>
    /// Gets or sets the current magical critical hit chance of the unit.
    /// </summary>
    public float CurrentMagicalCritChance
    {
        get => currentMagicalCritChance;
        set => currentMagicalCritChance = value;
    }

    /// <summary>
    /// Gets or sets the current physical defense of the unit.
    /// </summary>
    public float CurrentPhysDef
    {
        get => currentPhysDef;
        set => currentPhysDef = value;
    }

    /// <summary>
    /// Gets or sets the current magical defense of the unit.
    /// </summary>
    public float CurrentMagicDef
    {
        get => currentMagicDef;
        set => currentMagicDef = value;
    }

    /// <summary>
    /// Gets or sets the current physical mitigation of the unit.
    /// </summary>
    public float CurrentPhysicalMitigation
    {
        get => currentPhysicalMitigation;
        set => currentPhysicalMitigation = value;
    }

    /// <summary>
    /// Gets or sets the current physical block of the unit.
    /// </summary>
    public float CurrentPhysicalBlock
    {
        get => currentPhysicalBlock;
        set => currentPhysicalBlock = value;
    }

    /// <summary>
    /// Gets or sets the current magical mitigation of the unit.
    /// </summary>
    public float CurrentMagicalMitigation
    {
        get => currentMagicalMitigation;
        set => currentMagicalMitigation = value;
    }

    /// <summary>
    /// Gets or sets the current dodge chance of the unit.
    /// </summary>
    public float CurrentDodge
    {
        get => currentDodge;
        set => currentDodge = value;
    }

    /// <summary>
    /// Gets or sets the current speed of the unit.
    /// </summary>
    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    /// <summary>
    /// Gets or sets whether the unit is alive.
    /// </summary>
    public bool IsAlive
    {
        get => CurrentHp > 0;
        set => isAlive = value;
    }

    /// <summary>
    /// Gets the combat effects manager for the unit.
    /// </summary>
    public CombatEffectManager CombatEffectsManager
    {
        get => combatEffectsManager;
    }
}