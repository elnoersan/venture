/// <summary>
/// Represents the result of a combat encounter, including experience points gained and the player's current health.
/// </summary>
public class CombatResult
{
    // The amount of experience points (XP) gained by the player after the combat.
    private int xpGained;

    // The player's current health points (HP) after the combat.
    private float playerCurrentHp;

    /// <summary>
    /// Initializes a new instance of the <see cref="CombatResult"/> class with the specified XP gained and player's current HP.
    /// </summary>
    /// <param name="xpGained">The amount of XP gained by the player.</param>
    /// <param name="playerCurrentHp">The player's current HP after the combat.</param>
    public CombatResult(int xpGained, float playerCurrentHp)
    {
        this.xpGained = xpGained;
        this.playerCurrentHp = playerCurrentHp;
    }

    /// <summary>
    /// Gets or sets the amount of XP gained by the player.
    /// </summary>
    public int XpGained
    {
        get => xpGained;
        set => xpGained = value;
    }

    /// <summary>
    /// Gets or sets the player's current HP after the combat.
    /// </summary>
    public float PlayerCurrentHp
    {
        get => playerCurrentHp;
        set => playerCurrentHp = value;
    }
}