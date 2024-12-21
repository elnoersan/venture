/// <summary>
/// Represents the various states of a combat encounter in a game.
/// </summary>
public enum CombatState
{
    /// <summary>
    /// Indicates that the combat has just started.
    /// </summary>
    START,

    /// <summary>
    /// Indicates that the player is selecting an action (e.g., attack, defend, use item).
    /// </summary>
    PLAYER_ACTION_SELECT,

    /// <summary>
    /// Indicates that the player is selecting a skill to use during their turn.
    /// </summary>
    PLAYER_SKILL_SELECT,

    /// <summary>
    /// Indicates that the player is selecting a target for their action or skill.
    /// </summary>
    PLAYER_TARGET_SELECT,

    /// <summary>
    /// Indicates that it is the enemy's turn to take action.
    /// </summary>
    ENEMY_TURN,

    /// <summary>
    /// Indicates that the player has won the combat.
    /// </summary>
    VICTORY,

    /// <summary>
    /// Indicates that the player has lost the combat.
    /// </summary>
    DEFEAT
}