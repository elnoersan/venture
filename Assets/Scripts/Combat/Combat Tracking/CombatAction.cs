/// <summary>
/// Represents the possible actions a unit can take during combat.
/// </summary>
public enum CombatAction
{
    /// <summary>
    /// The unit performs a basic attack on an enemy.
    /// </summary>
    ATTACK,

    /// <summary>
    /// The unit defends, reducing incoming damage for the next turn.
    /// </summary>
    DEFEND,

    /// <summary>
    /// The unit provides support, such as healing or buffing allies.
    /// </summary>
    SUPPORT
}