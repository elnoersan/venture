using UnityEngine;

/// <summary>
/// Represents a combat move in a game, encapsulating its properties and behavior.
/// </summary>
public class CombatMove
{
    // The base data for the combat move, containing all its properties.
    private CombatMoveBase _base;

    // Tracks the cooldown of the combat move.
    private CooldownTracker cooldownTracker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CombatMove"/> class with the specified base data.
    /// </summary>
    /// <param name="mBase">The base data for the combat move.</param>
    public CombatMove(CombatMoveBase mBase)
    {
        _base = mBase;
        // Initialize the cooldown tracker with an initial cooldown of 0.
        cooldownTracker = new CooldownTracker(0);
    }

    /// <summary>
    /// Gets the description of the combat move.
    /// </summary>
    /// <returns>The description of the move.</returns>
    public string GetDescription()
    {
        return _base.Description;
    }

    /// <summary>
    /// Gets the cooldown tracker for the combat move.
    /// </summary>
    /// <returns>The cooldown tracker associated with the move.</returns>
    public CooldownTracker GetCooldownTracker()
    {
        return cooldownTracker;
    }

    /// <summary>
    /// Gets the icon image associated with the combat move.
    /// </summary>
    /// <returns>The icon image of the move.</returns>
    public Sprite getIconImage()
    {
        return _base.IconImage;
    }

    /// <summary>
    /// Gets the name of the combat move.
    /// </summary>
    /// <returns>The name of the move.</returns>
    public string GetName()
    {
        return _base.name;
    }

    /// <summary>
    /// Gets the power of the combat move.
    /// </summary>
    /// <returns>The power of the move.</returns>
    public int GetPower()
    {
        return _base.Power;
    }

    /// <summary>
    /// Gets the cooldown duration of the combat move.
    /// </summary>
    /// <returns>The cooldown duration of the move.</returns>
    public int GetCooldown()
    {
        return _base.Cooldown;
    }

    /// <summary>
    /// Gets the type of the combat move.
    /// </summary>
    /// <returns>The type of the move.</returns>
    public CombatMoveType GetType()
    {
        return _base.Type.GetType();
    }

    /// <summary>
    /// Gets the action type of the combat move.
    /// </summary>
    /// <returns>The action type of the move.</returns>
    public CombatAction GetActionType()
    {
        return _base.ActionType;
    }

    /// <summary>
    /// Gets the icon associated with the combat move's type.
    /// </summary>
    /// <returns>The icon of the move's type.</returns>
    public Sprite GetIcon()
    {
        return _base.Type.GetIcon();
    }

    /// <summary>
    /// Gets the duration of the combat move's effect.
    /// </summary>
    /// <returns>The duration of the move's effect.</returns>
    public int GetDuration()
    {
        return _base.Duration;
    }

    /// <summary>
    /// Gets the target type of the combat move.
    /// </summary>
    /// <returns>The target type of the move.</returns>
    public CombatMoveTargets GetTargets()
    {
        return _base.Targets;
    }

    /// <summary>
    /// Gets the effect type of the combat move.
    /// </summary>
    /// <returns>The effect type of the move.</returns>
    public CombatEffectType GetEffectType()
    {
        return _base.EffectType;
    }

    /// <summary>
    /// Gets a value indicating whether the effect of the combat move expires at the start of the turn.
    /// </summary>
    /// <returns><c>true</c> if the effect expires at the start of the turn; otherwise, <c>false</c>.</returns>
    public bool GetExpiresAtStartOfTurn()
    {
        return _base.ExpiresAtStartOfTurn;
    }
}