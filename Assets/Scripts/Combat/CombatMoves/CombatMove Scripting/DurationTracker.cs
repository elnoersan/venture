/// <summary>
/// Tracks the duration of an effect in a combat system, managing how many turns remain before the effect expires.
/// </summary>
public class DurationTracker
{
    // The remaining number of turns before the effect expires.
    private int remainingDuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="DurationTracker"/> class with the specified duration.
    /// </summary>
    /// <param name="duration">The initial duration of the effect in turns.</param>
    public DurationTracker(int duration)
    {
        // Set the initial duration of the effect.
        SetDuration(duration);
    }

    /// <summary>
    /// Gets the remaining duration of the effect.
    /// </summary>
    /// <returns>The number of turns remaining before the effect expires.</returns>
    public int GetRemainingDuration()
    {
        return remainingDuration;
    }

    /// <summary>
    /// Sets the duration of the effect to a specified number of turns.
    /// </summary>
    /// <param name="amountOfTurns">The number of turns the effect should last.</param>
    public void SetDuration(int amountOfTurns)
    {
        remainingDuration = amountOfTurns;
    }

    /// <summary>
    /// Checks if the effect is still active.
    /// </summary>
    /// <returns><c>true</c> if the effect is active (i.e., has remaining duration); otherwise, <c>false</c>.</returns>
    public bool isEffectActive()
    {
        return remainingDuration > 0;
    }

    /// <summary>
    /// Decreases the duration of the effect by one turn.
    /// </summary>
    public void DecreaseDuration()
    {
        // Only decrease the duration if it is greater than zero.
        if (remainingDuration > 0) remainingDuration--;
    }
}