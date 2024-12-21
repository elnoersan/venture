using System.Collections.Generic;
using UnityEngine;

/*
 * Manages the turn order of units in combat based on their speed.
 * 1) Get all active units
 * 2) Find the fastest (sort)
 * 3) Fastest is the baseline (will act first every round)
 * 4) Use ratio (fastest speed/slower unit speed)
 * 5) If unit's turn counter < current turn + 1 -> act and increment (unit's turn counter + unit's turn ratio)
 * 6) Else, do not increment and check again next round.
 */

/// <summary>
/// Manages the turn order of units in combat based on their speed.
/// This class determines which unit acts next by calculating turn ratios and managing turn counters.
/// </summary>
public class TurnManager
{
    /// <summary>
    /// Initializes the TurnManager with a list of active units.
    /// </summary>
    /// <param name="incomingUnits">The list of GameObjects representing active units in combat.</param>
    public TurnManager(List<GameObject> incomingUnits)
    {
        sortedUnits = new List<CombatUnit>();
        activeUnits = new List<TurnUnit>();

        // Extract CombatUnit components from the incoming GameObjects.
        incomingUnits.ForEach(go => sortedUnits.Add(go.GetComponent<CombatUnit>()));

        // Sort the units based on their speed.
        sortedUnits.Sort();

        // Set up the turn list based on the sorted units.
        SetupTurnList();
    }

    private List<CombatUnit> sortedUnits; // The list of units sorted by speed.
    private List<TurnUnit> activeUnits; // The list of active units with their turn ratios.
    private float fastestSpeed; // The speed of the fastest unit.
    private int currentTurn = 1; // The current turn counter.

    /// <summary>
    /// Sets up the turn list by calculating turn ratios for each unit.
    /// </summary>
    public void SetupTurnList()
    {
        // The fastest unit's speed is used as the baseline.
        fastestSpeed = sortedUnits[0].CurrentSpeed;
        Debug.Log("FASTEST UNIT: " + sortedUnits[0]);

        // Calculate turn ratios for each unit and add them to the activeUnits list.
        sortedUnits.ForEach(unit =>
        {
            activeUnits.Add(GetTurnUnit(unit));
        });

        // Sort the active units based on their turn ratios.
        activeUnits.Sort();
        Debug.Log("FASTEST UNIT: " + activeUnits[0].CombatUnit.UnitName);
    }

    /// <summary>
    /// Creates a TurnUnit object for the given CombatUnit, including its turn ratio.
    /// </summary>
    /// <param name="unit">The CombatUnit to create a TurnUnit for.</param>
    /// <returns>A TurnUnit object representing the unit and its turn ratio.</returns>
    public TurnUnit GetTurnUnit(CombatUnit unit)
    {
        // Calculate the turn ratio based on the fastest unit's speed.
        float turnRatio = fastestSpeed / unit.CurrentSpeed;

        return new TurnUnit(
            unit,
            turnRatio
        );
    }

    /// <summary>
    /// Determines the next unit to act in the combat.
    /// </summary>
    /// <returns>The CombatUnit that is next to act.</returns>
    public CombatUnit GetNextTurn()
    {
        List<TurnUnit> unitsToActThisTurn = new List<TurnUnit>();

        // Check which units are ready to act based on their turn counters.
        activeUnits.ForEach(unit =>
        {
            if (unit.TurnCounter < currentTurn + 1)
            {
                unitsToActThisTurn.Add(unit);
            }
        });

        // If no units are ready to act this turn, increment the turn counter and check again.
        if (unitsToActThisTurn.Count > 0)
        {
            // Sort the units that are ready to act to ensure the correct order.
            unitsToActThisTurn.Sort();

            // Cache the reference to the unit that will act next.
            TurnUnit nextUnitToActThisTurn = unitsToActThisTurn[0];

            // Increment the turn counter of the unit that's about to act.
            activeUnits.Find(turnUnit => turnUnit.Equals(nextUnitToActThisTurn)).TurnCounter +=
                nextUnitToActThisTurn.TurnRatio;

            // Return the unit to the combat system.
            return unitsToActThisTurn[0].CombatUnit;
        }
        else
        {
            currentTurn++;
            return GetNextTurn();
        }
    }

    /// <summary>
    /// Removes a disabled unit from the active units list.
    /// </summary>
    /// <param name="disabledUnit">The CombatUnit to remove.</param>
    public void RemoveFromActiveUnits(CombatUnit disabledUnit)
    {
        for (int i = activeUnits.Count - 1; i >= 0; i--)
        {
            if (activeUnits[i].CombatUnit.Equals(disabledUnit))
            {
                Debug.Log("Removed from speed manager list: " + activeUnits[i].CombatUnit.UnitName);
                activeUnits.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Debug method to print the turn order of active units.
    /// </summary>
    public void DebugPrintTurnOrder()
    {
        activeUnits.ForEach(unit => Debug.Log("Name: " + unit.CombatUnit.UnitName + " -- Speed: " + unit.CombatUnit.CurrentSpeed));
    }
}