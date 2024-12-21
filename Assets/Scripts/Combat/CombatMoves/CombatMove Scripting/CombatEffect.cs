/// <summary>
/// Represents an effect applied during a combat encounter, such as a buff, debuff, or status effect.
/// </summary>
public class CombatEffect
{
    // The strength or magnitude of the effect.
    private float power;

    // Indicates whether the effect expires at the start of the turn.
    private bool expiresAtStartOfTurn;

    // Indicates whether the effect has a duration that is tied to the number of turns.
    private bool hasTurnDuration;

    // Tracks the duration of the effect (e.g., how many turns it lasts).
    private DurationTracker durationTracker;

    // The type of combat effect (e.g., poison, burn, buff, debuff).
    private CombatEffectType combatEffectType;

    /// <summary>
    /// Initializes a new instance of the <see cref="CombatEffect"/> class based on the properties of a combat move.
    /// </summary>
    /// <param name="move">The combat move that applies this effect.</param>
    public CombatEffect(CombatMove move)
    {
        // Set the power of the effect based on the move's power.
        power = move.GetPower();

        // Determine if the effect expires at the start of the turn.
        expiresAtStartOfTurn = move.GetExpiresAtStartOfTurn();

        // Initialize the duration tracker with the move's duration.
        durationTracker = new DurationTracker(move.GetDuration());

        // Check if the effect has a turn-based duration.
        hasTurnDuration = durationTracker.GetRemainingDuration() > 0;

        // Set the type of the combat effect.
        combatEffectType = move.GetEffectType();
    }

    /// <summary>
    /// Gets or sets the power of the combat effect.
    /// </summary>
    public float Power
    {
        get => power;
        set => power = value;
    }

    /// <summary>
    /// Gets or sets the duration tracker for the combat effect.
    /// </summary>
    public DurationTracker DurationTracker
    {
        get => durationTracker;
        set => durationTracker = value;
    }

    /// <summary>
    /// Gets or sets the type of the combat effect.
    /// </summary>
    public CombatEffectType CombatEffectType
    {
        get => combatEffectType;
        set => combatEffectType = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the effect expires at the start of the turn.
    /// </summary>
    public bool ExpiresAtStartOfTurn
    {
        get => expiresAtStartOfTurn;
        set => expiresAtStartOfTurn = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the effect has a duration tied to the number of turns.
    /// </summary>
    public bool HasTurnDuration
    {
        get => hasTurnDuration;
        set => hasTurnDuration = value;
    }
}