using System.Collections.Generic;
using UnityEngine;

/*
 * Used for handling execution of different skill types (who's it targetting, defensive, offensive etc)
 */
public class SkillExecutor : MonoBehaviour
{
    private CombatSystem combatSystem; // Reference to the CombatSystem script
    private SkillManager skillManager; // Reference to the SkillManager script
    private UICombatLog combatLog;     // Reference to the UICombatLog script for logging combat actions

    // Initialize references to other scripts in the scene
    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>(); // Find and assign the CombatSystem script
        skillManager = FindObjectOfType<SkillManager>(); // Find and assign the SkillManager script
        combatLog = FindObjectOfType<UICombatLog>();     // Find and assign the UICombatLog script
    }

    // Filters move by target and executes the move
    public List<TakeDamageResult> ExecuteMove(CombatMove move, CombatUnit user, CombatUnit target, List<GameObject> allEnemies = null)
    {
        skillManager.PutCombatMoveOnCooldown(move); // Put the move on cooldown after it's used

        // Determine the target type of the move and execute accordingly
        switch (move.GetTargets())
        {
            case CombatMoveTargets.Self:
                ExecuteMoveOnSelf(move, user); // Execute the move on the user themselves
                break;
            case CombatMoveTargets.Adjacent:
                return ExecuteMoveOnMultipleTargets(move, user, target, allEnemies); // Execute on adjacent targets
            case CombatMoveTargets.Global:
                return ExecuteMoveOnMultipleTargets(move, user, target, allEnemies); // Execute on all targets
            case CombatMoveTargets.Singular:
                return new List<TakeDamageResult> { ExecuteMoveOnTarget(move, user, target) }; // Execute on a single target
        }

        return null; // Return null if no targets are specified
    }

    /*
     * Self target
     */
    public void ExecuteMoveOnSelf(CombatMove move, CombatUnit unit)
    {
        // Determine the type of move and execute accordingly
        switch (move.GetType())
        {
            case CombatMoveType.Heal:
                ExecuteHealTypeMove(move, unit); // Execute a healing move
                break;
            case CombatMoveType.Block:
                ExecuteDefendTypeMove(move, unit); // Execute a blocking move
                break;
            case CombatMoveType.Mitigate:
                ExecuteDefendTypeMove(move, unit); // Execute a mitigation move
                break;
            case CombatMoveType.Buff:
                AddBuffToUnit(move, unit); // Apply a buff to the unit
                break;
        }
    }

    /*
     * Global or Adjacent targets
     */
    public List<TakeDamageResult> ExecuteMoveOnMultipleTargets(CombatMove move, CombatUnit attacker, CombatUnit target, List<GameObject> allEnemies)
    {
        List<TakeDamageResult> results = new List<TakeDamageResult>();

        // If the move targets all enemies globally
        if (move.GetTargets() == CombatMoveTargets.Global)
        {
            allEnemies.ForEach(enemyGO =>
            {
                TakeDamageResult result = ExecuteMoveOnTarget(move, attacker, enemyGO.GetComponent<CombatUnit>());
                results.Add(result);
            });

            return results;
        }
        else // If the move targets adjacent enemies
        {
            combatSystem.GetTargetAndActiveAdjacentPosition(target).ForEach(enemy =>
            {
                TakeDamageResult result = ExecuteMoveOnTarget(move, attacker, enemy);
                results.Add(result);
            });

            return results;
        }
    }

    /*
     * Singular Target
     */
    public TakeDamageResult ExecuteMoveOnTarget(CombatMove move, CombatUnit attacker, CombatUnit target)
    {
        // Determine the type of move and execute accordingly
        if (move.GetType().Equals(CombatMoveType.Debuff))
        {
            return AddDebuffToUnit(move, target); // Apply a debuff to the target
        }

        if (move.GetType().Equals(CombatMoveType.Suffer))
        {
            return ExecuteSufferAttackMove(move, attacker, target); // Execute a "suffer" attack move
        }

        return ExecuteRegularAttackMove(attacker, target, move); // Execute a regular attack move
    }

    /*
     * Physical or Magical attack
     */
    private TakeDamageResult ExecuteRegularAttackMove(CombatUnit attacker, CombatUnit target, CombatMove move)
    {
        float damage = attacker.DoDamage(move); // Calculate the damage dealt by the attacker
        var takeDamageResult = target.TakeDamage(damage, move.GetType()); // Apply the damage to the target
        combatLog.UsedOffensiveCombatMove(move, attacker, target, takeDamageResult.DamageTaken); // Log the attack
        return takeDamageResult;
    }

    /*
     * Suffer attack
     */
    private TakeDamageResult ExecuteSufferAttackMove(CombatMove move, CombatUnit attacker, CombatUnit target)
    {
        // Suffer damage is true damage and does not get multiplied.
        // Sometimes this is called due to a combat effect, log is not correct.
        combatLog.UsedOffensiveCombatMove(move, attacker, target, move.GetPower()); // Log the attack
        return target.TakeDamage(move.GetPower(), CombatMoveType.Suffer); // Apply true damage to the target
    }

    /*
     * Healing
     */
    private void ExecuteHealTypeMove(CombatMove move, CombatUnit unit)
    {
        if (move.GetEffectType().Equals(CombatEffectType.Renew))
        {
            AddBuffToUnit(move, unit); // Apply a buff if the healing move has a "renew" effect
        }
        else
        {
            combatLog.PlayerHealed(move); // Log the healing action
            unit.Heal(move.GetPower()); // Heal the unit
        }
    }

    /*
     * Block or Mitigate
     */
    private void ExecuteDefendTypeMove(CombatMove move, CombatUnit unit)
    {
        if (move.GetEffectType() == CombatEffectType.Block)
        {
            Debug.Log("Added Block");

            combatLog.PlayerAppliedBlock(move); // Log the block action

            unit.CurrentPhysicalBlock = move.GetPower(); // Apply physical block to the unit
            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move); // Add the block effect
        }
        else
        {
            Debug.Log("Activating mitigation");

            combatLog.UnitAppliedEffect(unit, move); // Log the mitigation action

            // Apply mitigation based on the type of mitigation
            switch (move.GetEffectType())
            {
                case CombatEffectType.PhysMitigation:
                    float physMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = physMitigation / 100;
                    break;
                case CombatEffectType.MagicMitigation:
                    float magicMitigation = move.GetPower();
                    unit.CurrentMagicalMitigation = magicMitigation / 100;
                    break;
                case CombatEffectType.AllMitigation:
                    float allPhysMitigation = move.GetPower();
                    float allMagicMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = allPhysMitigation / 100;
                    unit.CurrentMagicalMitigation = allMagicMitigation / 100;
                    break;
            }

            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move); // Add the mitigation effect
        }
    }

    /*
     * Apply active beneficial effect to self
     */
    private void AddBuffToUnit(CombatMove buffMove, CombatUnit unit)
    {
        Debug.Log("Buffed with " + buffMove.GetEffectType() + " effect");

        unit.GetComponent<CombatEffectManager>().AddCombatEffect(buffMove); // Add the buff effect
        combatLog.UnitAppliedEffect(unit, buffMove); // Log the buff application
    }

    /*
     * Apply active negative effect to target
     */
    private TakeDamageResult AddDebuffToUnit(CombatMove move, CombatUnit target)
    {
        Debug.Log("Debuffed with " + move.GetEffectType() + " effect");

        target.GetComponent<CombatEffectManager>().AddCombatEffect(move); // Add the debuff effect
        combatLog.UnitAppliedEffect(target, move); // Log the debuff application

        return new TakeDamageResult(target, false, 0); // Return a result indicating no damage was taken
    }
}