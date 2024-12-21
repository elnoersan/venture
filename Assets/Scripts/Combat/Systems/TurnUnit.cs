using System;

/// <summary>
/// Represents a unit in the turn-based combat system, including its turn ratio and turn counter.
/// This class implements the IComparable interface to allow sorting based on turn counters.
/// </summary>
public class TurnUnit : IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TurnUnit"/> class.
    /// </summary>
    /// <param name="combatUnit">The CombatUnit associated with this TurnUnit.</param>
    /// <param name="turnRatio">The turn ratio of the unit, calculated based on its speed.</param>
    public TurnUnit(CombatUnit combatUnit, float turnRatio)
    {
        this._combatUnit = combatUnit;
        this.turnRatio = turnRatio;
        turnCounter = turnRatio; // Initialize the turn counter to the turn ratio.
    }

    private CombatUnit _combatUnit; // The CombatUnit associated with this TurnUnit.
    private float turnRatio; // The turn ratio of the unit, used to determine when it acts.
    private float turnCounter; // The current turn counter of the unit.

    /// <summary>
    /// Gets or sets the CombatUnit associated with this TurnUnit.
    /// </summary>
    public CombatUnit CombatUnit
    {
        get => _combatUnit;
        set => _combatUnit = value;
    }

    /// <summary>
    /// Gets or sets the turn ratio of the unit.
    /// </summary>
    public float TurnRatio
    {
        get => turnRatio;
        set => turnRatio = value;
    }

    /// <summary>
    /// Gets or sets the current turn counter of the unit.
    /// </summary>
    public float TurnCounter
    {
        get => turnCounter;
        set => turnCounter = value;
    }

    /// <summary>
    /// Compares this TurnUnit to another TurnUnit based on their turn counters.
    /// </summary>
    /// <param name="obj">The object to compare to (must be a TurnUnit).</param>
    /// <returns>An integer indicating the relative order of the TurnUnits.</returns>
    public int CompareTo(object obj)
    {
        TurnUnit other = obj as TurnUnit;
        return turnCounter.CompareTo(other.turnCounter);
    }
}