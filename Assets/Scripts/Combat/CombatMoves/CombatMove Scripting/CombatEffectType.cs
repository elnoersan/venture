/// <summary>
/// Represents the types of combat effects that can be applied during a combat encounter.
/// </summary>
public enum CombatEffectType
{
    /// <summary>
    /// Represents no effect (default value).
    /// </summary>
    None,

    /// <summary>
    /// Represents a healing or regenerative effect that restores health over time.
    /// </summary>
    Renew,

    /// <summary>
    /// Represents a damaging effect that deals damage over time (e.g., poison).
    /// </summary>
    Poison,

    /// <summary>
    /// Represents an effect that prevents the use of abilities or skills (e.g., silence).
    /// </summary>
    Silence,

    /// <summary>
    /// Represents a buff that increases the target's strength or attack power.
    /// </summary>
    Strengthen,

    /// <summary>
    /// Represents a debuff that decreases the target's strength or attack power.
    /// </summary>
    Weaken,

    /// <summary>
    /// Represents an effect that blocks incoming damage or actions.
    /// </summary>
    Block,

    /// <summary>
    /// Represents a mitigation effect that reduces physical damage taken.
    /// </summary>
    PhysMitigation,

    /// <summary>
    /// Represents a mitigation effect that reduces magical damage taken.
    /// </summary>
    MagicMitigation,

    /// <summary>
    /// Represents a mitigation effect that reduces all types of damage taken.
    /// </summary>
    AllMitigation
}