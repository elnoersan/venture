using UnityEngine;

/// <summary>
/// Represents the base data for a combat move, stored as a ScriptableObject.
/// This class defines the properties and attributes of a combat move, such as its name, type, power, cooldown, and visual effects.
/// </summary>
[CreateAssetMenu(fileName = "CombatMove", menuName = "Combat/Create new Combat Move")]
public class CombatMoveBase : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string name; // The name of the combat move.
    [TextArea][SerializeField] private string description; // A detailed description of the combat move.

    [Header("Stats")]
    [SerializeField] private CombatMoveTypeSO type; // The type of the combat move (e.g., Physical, Magical).
    [SerializeField] private CombatAction actionType; // The action type of the combat move (e.g., Attack, Heal).
    [SerializeField] private CombatMoveTargets targets; // The target type of the combat move (e.g., Self, Singular, Global).
    [SerializeField] private CombatEffectType effectType; // The effect type of the combat move (e.g., Poison, Buff).
    [SerializeField] private bool expiresAtStartOfTurn; // Whether the effect of the move expires at the start of the turn.
    [SerializeField] private int power; // The power or strength of the combat move.
    [SerializeField] private int cooldown = 0; // The cooldown duration of the combat move (in turns).
    [SerializeField] private int duration = 0; // The duration of the combat move's effect (in turns).

    [Header("VFX")]
    [SerializeField] private Sprite icon_image; // The icon image representing the combat move.

    /// <summary>
    /// Gets or sets the name of the combat move.
    /// </summary>
    public string Name
    {
        get => name;
        set => name = value;
    }

    /// <summary>
    /// Gets or sets the description of the combat move.
    /// </summary>
    public string Description
    {
        get => description;
        set => description = value;
    }

    /// <summary>
    /// Gets or sets the type of the combat move.
    /// </summary>
    public CombatMoveTypeSO Type
    {
        get => type;
        set => type = value;
    }

    /// <summary>
    /// Gets or sets the action type of the combat move.
    /// </summary>
    public CombatAction ActionType
    {
        get => actionType;
        set => actionType = value;
    }

    /// <summary>
    /// Gets or sets the power of the combat move.
    /// </summary>
    public int Power
    {
        get => power;
        set => power = value;
    }

    /// <summary>
    /// Gets or sets the cooldown duration of the combat move.
    /// </summary>
    public int Cooldown
    {
        get => cooldown;
        set => cooldown = value;
    }

    /// <summary>
    /// Gets or sets the icon image of the combat move.
    /// </summary>
    public Sprite IconImage
    {
        get => icon_image;
        set => icon_image = value;
    }

    /// <summary>
    /// Gets or sets the target type of the combat move.
    /// </summary>
    public CombatMoveTargets Targets
    {
        get => targets;
        set => targets = value;
    }

    /// <summary>
    /// Gets or sets the effect type of the combat move.
    /// </summary>
    public CombatEffectType EffectType
    {
        get => effectType;
        set => effectType = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the effect of the combat move expires at the start of the turn.
    /// </summary>
    public bool ExpiresAtStartOfTurn
    {
        get => expiresAtStartOfTurn;
        set => expiresAtStartOfTurn = value;
    }

    /// <summary>
    /// Gets or sets the duration of the combat move's effect.
    /// </summary>
    public int Duration
    {
        get => duration;
        set => duration = value;
    }
}