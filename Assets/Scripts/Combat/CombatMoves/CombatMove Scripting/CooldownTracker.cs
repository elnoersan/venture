/// <summary>
/// Tracks the cooldown of a move or ability in a combat system, managing how many turns remain until the move can be used again.
/// </summary>
public class CooldownTracker
{
    // The remaining number of turns before the move can be used again.
    private int remainingCooldown;

    /// <summary>
    /// Initializes a new instance of the <see cref="CooldownTracker"/> class with the specified remaining cooldown.
    /// </summary>
    /// <param name="remainingCooldown">The initial remaining cooldown for the move.</param>
    public CooldownTracker(int remainingCooldown)
    {
        this.remainingCooldown = remainingCooldown;
    }

    /// <summary>
    /// Gets the remaining cooldown for the move.
    /// </summary>
    /// <returns>The number of turns remaining before the move can be used again.</returns>
    public int GetRemainingCooldown()
    {
        return remainingCooldown;
    }

    /// <summary>
    /// Puts the move on cooldown for a specified number of turns.
    /// </summary>
    /// <param name="amountOfTurns">The number of turns the move should be on cooldown.</param>
    public void PutMoveOnCooldown(int amountOfTurns)
    {
        // The cooldown is set to the specified number of turns plus one additional turn.
        remainingCooldown = amountOfTurns + 1;
    }

    /// <summary>
    /// Checks if the move is currently on cooldown.
    /// </summary>
    /// <returns><c>true</c> if the move is on cooldown; otherwise, <c>false</c>.</returns>
    public bool isMoveOnCooldown()
    {
        return remainingCooldown > 0;
    }

    /// <summary>
    /// Decreases the cooldown counter by one turn.
    /// </summary>
    public void DecreaseCooldownCounter()
    {
        // Only decrease the cooldown if it is greater than zero.
        if (remainingCooldown > 0) remainingCooldown--;
    }
}