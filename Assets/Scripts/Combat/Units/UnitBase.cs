using System;
using UnityEngine;

/// <summary>
/// Represents the base data for a unit in the combat system.
/// This ScriptableObject contains all the base stats, growth rates, and visual properties of a unit.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Combat/Create new Unit SO")]
public class UnitBase : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string unitName; // The name of the unit.
    [TextArea][SerializeField] private string description; // A detailed description of the unit.
    [SerializeField] private UnitType unitType; // The type of the unit (e.g., Player, Enemy).
    [SerializeField] private bool isPlayerUnit; // Whether the unit is a player unit.

    [Header("VFX")]
    [SerializeField] private Sprite idleSprite; // The idle sprite of the unit.
    [SerializeField] private float spriteLocalScale = 1; // The local scale of the unit's sprite.
    [SerializeField] private float spriteVerticalOffset = 0; // The vertical offset of the unit's sprite.

    [Header("Base Stats")]
    [SerializeField] private int maxHp; // The base maximum health points of the unit.
    [SerializeField] private float strength; // The base strength of the unit.
    [SerializeField] private float agility; // The base agility of the unit.
    [SerializeField] private float intellect; // The base intellect of the unit.
    [SerializeField] private float attackPower; // The base attack power of the unit.
    [SerializeField] private float abilityPower; // The base ability power of the unit.
    [SerializeField] private float physicalCritChance; // The base physical critical hit chance of the unit.
    [SerializeField] private float magicalCritChance; // The base magical critical hit chance of the unit.
    [SerializeField] private float physicalDefense; // The base physical defense of the unit.
    [SerializeField] private float magicalDefense; // The base magical defense of the unit.
    [SerializeField] private float physicalBlockPower; // The base physical block power of the unit.
    [SerializeField] private float dodgeChance; // The base dodge chance of the unit.
    [SerializeField] private float speed; // The base speed of the unit.

    [Header("Stat Ratios")]
    [SerializeField] private float strengthApRatio = 2; // The ratio of strength to attack power.
    [SerializeField] private float agilityApRatio = 1; // The ratio of agility to attack power.
    [SerializeField] private float intellectAbpRatio = 2; // The ratio of intellect to ability power.
    [SerializeField] private float agilityCritRatio = 0.05f; // The ratio of agility to physical critical hit chance.
    [SerializeField] private float intellectCritRatio = 0.05f; // The ratio of intellect to magical critical hit chance.
    [SerializeField] private float strengthPhysDefRatio = 1; // The ratio of strength to physical defense.
    [SerializeField] private float agilityPhysDefRatio = 2; // The ratio of agility to physical defense.
    [SerializeField] private float agilityDodgeRatio = 0.05f; // The ratio of agility to dodge chance.
    [SerializeField] private float agilitySpeedRatio = 1; // The ratio of agility to speed.
    [SerializeField] private float critMultiplier = 2; // The multiplier applied to critical hits.

    [Header("Growth Rates")]
    [SerializeField] private float maxHpGrowth = 2; // The growth rate of maximum health points per level.
    [SerializeField] private float strengthGrowth = 1; // The growth rate of strength per level.
    [SerializeField] private float agilityGrowth = 1; // The growth rate of agility per level.
    [SerializeField] private float intellectGrowth = 1; // The growth rate of intellect per level.
    [SerializeField] private float physicalDefenseGrowth = 1; // The growth rate of physical defense per level.
    [SerializeField] private float magicalDefenseGrowth = 2; // The growth rate of magical defense per level.
    [SerializeField] private float physicalBlockPowerGrowth = 1; // The growth rate of physical block power per level.
    [SerializeField] private float speedGrowth = 2; // The growth rate of speed per level.

    /// <summary>
    /// Gets or sets the name of the unit.
    /// </summary>
    public string UnitName
    {
        get => unitName;
        set => unitName = value;
    }

    /// <summary>
    /// Gets or sets the description of the unit.
    /// </summary>
    public string Description
    {
        get => description;
        set => description = value;
    }

    /// <summary>
    /// Gets or sets the type of the unit (e.g., Player, Enemy).
    /// </summary>
    public UnitType UnitType
    {
        get => unitType;
        set => unitType = value;
    }

    /// <summary>
    /// Gets or sets the idle sprite of the unit.
    /// </summary>
    public Sprite IdleSprite
    {
        get => idleSprite;
        set => idleSprite = value;
    }

    /// <summary>
    /// Gets or sets the local scale of the unit's sprite.
    /// </summary>
    public float SpriteLocalScale
    {
        get => spriteLocalScale;
        set => spriteLocalScale = value;
    }

    /// <summary>
    /// Gets or sets the vertical offset of the unit's sprite.
    /// </summary>
    public float SpriteVerticalOffset
    {
        get => spriteVerticalOffset;
        set => spriteVerticalOffset = value;
    }

    /// <summary>
    /// Gets or sets the base maximum health points of the unit.
    /// </summary>
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    /// <summary>
    /// Gets or sets the base strength of the unit.
    /// </summary>
    public float Strength
    {
        get => strength;
        set => strength = value;
    }

    /// <summary>
    /// Gets or sets the base agility of the unit.
    /// </summary>
    public float Agility
    {
        get => agility;
        set => agility = value;
    }

    /// <summary>
    /// Gets or sets the base intellect of the unit.
    /// </summary>
    public float Intellect
    {
        get => intellect;
        set => intellect = value;
    }

    /// <summary>
    /// Gets or sets the base attack power of the unit.
    /// </summary>
    public float AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    /// <summary>
    /// Gets or sets the base ability power of the unit.
    /// </summary>
    public float AbilityPower
    {
        get => abilityPower;
        set => abilityPower = value;
    }

    /// <summary>
    /// Gets or sets the base physical critical hit chance of the unit.
    /// </summary>
    public float PhysicalCritChance
    {
        get => physicalCritChance;
        set => physicalCritChance = value;
    }

    /// <summary>
    /// Gets or sets the base magical critical hit chance of the unit.
    /// </summary>
    public float MagicalCritChance
    {
        get => magicalCritChance;
        set => magicalCritChance = value;
    }

    /// <summary>
    /// Gets or sets the base physical defense of the unit.
    /// </summary>
    public float PhysicalDefense
    {
        get => physicalDefense;
        set => physicalDefense = value;
    }

    /// <summary>
    /// Gets or sets the base magical defense of the unit.
    /// </summary>
    public float MagicalDefense
    {
        get => magicalDefense;
        set => magicalDefense = value;
    }

    /// <summary>
    /// Gets or sets the base physical block power of the unit.
    /// </summary>
    public float PhysicalBlockPower
    {
        get => physicalBlockPower;
        set => physicalBlockPower = value;
    }

    /// <summary>
    /// Gets or sets the base dodge chance of the unit.
    /// </summary>
    public float DodgeChance
    {
        get => dodgeChance;
        set => dodgeChance = value;
    }

    /// <summary>
    /// Gets or sets the base speed of the unit.
    /// </summary>
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    /// <summary>
    /// Gets or sets the ratio of strength to attack power.
    /// </summary>
    public float StrengthApRatio
    {
        get => strengthApRatio;
        set => strengthApRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of agility to attack power.
    /// </summary>
    public float AgilityApRatio
    {
        get => agilityApRatio;
        set => agilityApRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of intellect to ability power.
    /// </summary>
    public float IntellectAbpRatio
    {
        get => intellectAbpRatio;
        set => intellectAbpRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of agility to physical critical hit chance.
    /// </summary>
    public float AgilityCritRatio
    {
        get => agilityCritRatio;
        set => agilityCritRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of intellect to magical critical hit chance.
    /// </summary>
    public float IntellectCritRatio
    {
        get => intellectCritRatio;
        set => intellectCritRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of strength to physical defense.
    /// </summary>
    public float StrengthPhysDefRatio
    {
        get => strengthPhysDefRatio;
        set => strengthPhysDefRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of agility to physical defense.
    /// </summary>
    public float AgilityPhysDefRatio
    {
        get => agilityPhysDefRatio;
        set => agilityPhysDefRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of agility to dodge chance.
    /// </summary>
    public float AgilityDodgeRatio
    {
        get => agilityDodgeRatio;
        set => agilityDodgeRatio = value;
    }

    /// <summary>
    /// Gets or sets the ratio of agility to speed.
    /// </summary>
    public float AgilitySpeedRatio
    {
        get => agilitySpeedRatio;
        set => agilitySpeedRatio = value;
    }

    /// <summary>
    /// Gets or sets the multiplier applied to critical hits.
    /// </summary>
    public float CritMultiplier
    {
        get => critMultiplier;
        set => critMultiplier = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of maximum health points per level.
    /// </summary>
    public float MaxHpGrowth
    {
        get => maxHpGrowth;
        set => maxHpGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of strength per level.
    /// </summary>
    public float StrengthGrowth
    {
        get => strengthGrowth;
        set => strengthGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of agility per level.
    /// </summary>
    public float AgilityGrowth
    {
        get => agilityGrowth;
        set => agilityGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of intellect per level.
    /// </summary>
    public float IntellectGrowth
    {
        get => intellectGrowth;
        set => intellectGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of physical defense per level.
    /// </summary>
    public float PhysicalDefenseGrowth
    {
        get => physicalDefenseGrowth;
        set => physicalDefenseGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of magical defense per level.
    /// </summary>
    public float MagicalDefenseGrowth
    {
        get => magicalDefenseGrowth;
        set => magicalDefenseGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of physical block power per level.
    /// </summary>
    public float PhysicalBlockPowerGrowth
    {
        get => physicalBlockPowerGrowth;
        set => physicalBlockPowerGrowth = value;
    }

    /// <summary>
    /// Gets or sets the growth rate of speed per level.
    /// </summary>
    public float SpeedGrowth
    {
        get => speedGrowth;
        set => speedGrowth = value;
    }

    /// <summary>
    /// Gets or sets whether the unit is a player unit.
    /// </summary>
    public bool IsPlayerUnit
    {
        get => isPlayerUnit;
        set => isPlayerUnit = value;
    }
}