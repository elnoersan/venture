/// <summary>
/// Represents the possible target types for a combat move in a game.
/// </summary>
public enum CombatMoveTargets
{
    /// <summary>
    /// The move targets the user or the entity performing the move.
    /// </summary>
    Self,

    /// <summary>
    /// The move targets a single enemy or ally.
    /// </summary>
    Singular,

    /// <summary>
    /// The move targets entities that are adjacent to the user (e.g., neighboring enemies or allies).
    /// </summary>
    Adjacent,

    /// <summary>
    /// The move targets all entities in the combat (e.g., all enemies or all allies).
    /// </summary>
    Global
}