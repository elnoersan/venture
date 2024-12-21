/// <summary>
/// Represents the types of combat moves that can be performed in a game.
/// </summary>
public enum CombatMoveType
{
    /// <summary>
    /// A physical attack that deals damage based on the user's physical strength.
    /// </summary>
    Physical,

    /// <summary>
    /// A magical attack that deals damage based on the user's magical power.
    /// </summary>
    Magical,

    /// <summary>
    /// A move that causes the user to suffer damage or a negative effect.
    /// </summary>
    Suffer,

    /// <summary>
    /// A move that reduces incoming damage or provides defense.
    /// </summary>
    Mitigate,

    /// <summary>
    /// A move that blocks incoming attacks or effects.
    /// </summary>
    Block,

    /// <summary>
    /// A move that restores health or heals the target.
    /// </summary>
    Heal,

    /// <summary>
    /// A move that enhances the target's abilities or provides a positive effect.
    /// </summary>
    Buff,

    /// <summary>
    /// A move that weakens the target's abilities or applies a negative effect.
    /// </summary>
    Debuff
}