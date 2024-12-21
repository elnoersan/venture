using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
* Handles all active effects that require action on a unit, each round.
* Called by the Unit or the system before it's turn.
*/
public class CombatEffectManager : MonoBehaviour
{
    // The unit associated with this effect manager.
    private CombatUnit unit;

    // The combat system that manages the overall combat.
    private CombatSystem combatSystem;

    // A list of active combat effects currently affecting the unit.
    private List<CombatEffect> activeEffects;

    /// <summary>
    /// Initializes the CombatEffectManager by finding the associated CombatUnit and CombatSystem, and initializing the list of active effects.
    /// </summary>
    private void Awake()
    {
        unit = GetComponent<CombatUnit>();
        combatSystem = FindObjectOfType<CombatSystem>();
        activeEffects = new List<CombatEffect>();
    }

    /// <summary>
    /// Processes all active effects on the unit, either at the start or end of the turn.
    /// </summary>
    /// <param name="isStartOfTurn">Whether the processing is happening at the start of the turn.</param>
    public void ProcessActiveEffects(bool isStartOfTurn)
    {
        List<CombatEffect> expiredEffects = new List<CombatEffect>();

        // Iterate through all active effects and apply their effects.
        ActiveEffects.ForEach(activeEffect =>
        {
            // Check if the effect should be processed at the start of the turn.
            if (isStartOfTurn == activeEffect.ExpiresAtStartOfTurn)
            {
                // Handle healing effects.
                if (activeEffect.CombatEffectType.Equals(CombatEffectType.Renew))
                {
                    unit.Heal(activeEffect.Power);
                    DecreaseDurationOrExpire(activeEffect, expiredEffects);
                }

                // Handle poison effects.
                if (activeEffect.CombatEffectType.Equals(CombatEffectType.Poison))
                {
                    TakeDamageResult result = unit.TakeDamage(activeEffect.Power, CombatMoveType.Suffer);
                    DecreaseDurationOrExpire(activeEffect, expiredEffects);
                    combatSystem.CheckForDeath(result);
                }

                // Handle standard active effects that are being handled elsewhere.
                if (activeEffect.CombatEffectType.Equals(CombatEffectType.Block) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.Weaken) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.Strengthen) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.PhysMitigation) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.MagicMitigation) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.AllMitigation) ||
                    activeEffect.CombatEffectType.Equals(CombatEffectType.Silence))
                {
                    DecreaseDurationOrExpire(activeEffect, expiredEffects);
                }
            }
        });

        // Remove expired effects after processing to avoid list manipulation during iteration.
        expiredEffects.ForEach(expiredEffect => ActiveEffects.Remove(expiredEffect));
    }

    /// <summary>
    /// Decreases the duration of the active effect and marks it for expiration if its duration reaches zero.
    /// </summary>
    /// <param name="activeEffect">The active effect to process.</param>
    /// <param name="expiredEffects">The list of effects to be removed.</param>
    private static void DecreaseDurationOrExpire(CombatEffect activeEffect, List<CombatEffect> expiredEffects)
    {
        activeEffect.DurationTracker.DecreaseDuration();
        if (!activeEffect.DurationTracker.isEffectActive()) expiredEffects.Add(activeEffect);
    }

    /// <summary>
    /// Checks if a specific effect is currently active on the unit.
    /// </summary>
    /// <param name="effect">The type of effect to check for.</param>
    /// <returns><c>true</c> if the effect is active; otherwise, <c>false</c>.</returns>
    public bool IsEffectActive(CombatEffectType effect)
    {
        var effectFound = ActiveEffects.Find(activeEffect => activeEffect.CombatEffectType == effect);
        return effectFound != null;
    }

    /// <summary>
    /// Calculates the multiplier for a specific effect type based on the active effects.
    /// </summary>
    /// <param name="type">The type of effect to calculate the multiplier for.</param>
    /// <returns>The calculated multiplier.</returns>
    public float GetEffectMultiplier(CombatEffectType type)
    {
        List<CombatEffect> activeEffectsOfType =
            activeEffects.FindAll(effect => effect.CombatEffectType == type);

        float multiplier = 1;

        if (activeEffectsOfType.Count == 0)
        {
            return multiplier;
        }

        for (int i = 0; i < activeEffectsOfType.Count; i++)
        {
            if (type == CombatEffectType.Strengthen)
            {
                multiplier = multiplier * (activeEffectsOfType[i].Power / 100) + 1;
            }

            if (type == CombatEffectType.Weaken)
                multiplier -= (activeEffectsOfType[i].Power / 100);
        }

        return multiplier;
    }

    /// <summary>
    /// Adds a new combat effect to the unit and updates the UI to reflect the change.
    /// </summary>
    /// <param name="move">The combat move that applies the effect.</param>
    public void AddCombatEffect(CombatMove move)
    {
        Debug.Log("Adding " + move.GetEffectType() + " to " + unit.UnitName);
        CombatEffect combatEffect = new CombatEffect(move);
        activeEffects.Add(combatEffect);

        UIStatDisplay uiStatDisplay = FindObjectsOfType<UIStatDisplay>().ToList().Find(statDisplay => statDisplay.ConnectedUnit == unit);
        uiStatDisplay.AddActiveEffect(combatEffect);
    }

    /// <summary>
    /// Gets or sets the list of active combat effects.
    /// </summary>
    public List<CombatEffect> ActiveEffects
    {
        get => activeEffects;
        set => activeEffects = value;
    }

    /// <summary>
    /// Debug method to print all active effects to the console.
    /// </summary>
    public void DebugPrintEffects()
    {
        ActiveEffects.ForEach(effect => Debug.Log("Active effect: " + effect.CombatEffectType));
    }
}