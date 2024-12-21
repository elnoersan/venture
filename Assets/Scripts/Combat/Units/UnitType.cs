using System;

/// <summary>
/// Represents the type of a unit in the combat system.
/// This enum defines the possible types of units, such as Enemy, Player, and Boss.
/// </summary>
[Serializable]
public enum UnitType
{
    /// <summary>
    /// Represents an enemy unit in the combat system.
    /// </summary>
    ENEMY,

    /// <summary>
    /// Represents a player unit in the combat system.
    /// </summary>
    PLAYER,

    /// <summary>
    /// Represents a boss unit in the combat system.
    /// </summary>
    BOSS
}