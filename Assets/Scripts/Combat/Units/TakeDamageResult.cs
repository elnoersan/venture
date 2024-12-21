/// <summary>
/// Represents the result of a damage calculation in the combat system.
/// This class contains information about the unit that took damage, whether the unit is dead, and the amount of damage taken.
/// </summary>
public class TakeDamageResult
{
    private CombatUnit unit; // The unit that took damage.
    private bool isUnitDead; // Whether the unit is dead after taking damage.
    private int damageTaken; // The amount of damage taken by the unit.

    /// <summary>
    /// Initializes a new instance of the <see cref="TakeDamageResult"/> class.
    /// </summary>
    /// <param name="unit">The unit that took damage.</param>
    /// <param name="isUnitDead">Whether the unit is dead after taking damage.</param>
    /// <param name="damageTaken">The amount of damage taken by the unit.</param>
    public TakeDamageResult(CombatUnit unit, bool isUnitDead, int damageTaken)
    {
        this.unit = unit;
        this.isUnitDead = isUnitDead;
        this.damageTaken = damageTaken;
    }

    /// <summary>
    /// Gets the unit that took damage.
    /// </summary>
    public CombatUnit Unit => unit;

    /// <summary>
    /// Gets a value indicating whether the unit is dead after taking damage.
    /// </summary>
    public bool IsUnitDead => isUnitDead;

    /// <summary>
    /// Gets the amount of damage taken by the unit.
    /// </summary>
    public int DamageTaken => damageTaken;
}