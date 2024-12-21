using UnityEngine;

/// <summary>
/// Represents a ScriptableObject that defines a combat move type, including its type and associated icon.
/// </summary>
[CreateAssetMenu(fileName = "CombatMoveType", menuName = "Combat/Create new Type SO")]
public class CombatMoveTypeSO : ScriptableObject
{
    // The type of the combat move (e.g., Physical, Magical, Heal).
    [SerializeField] private CombatMoveType type;

    // The icon associated with the combat move type.
    [SerializeField] private Sprite icon;

    /// <summary>
    /// Gets the type of the combat move.
    /// </summary>
    /// <returns>The combat move type.</returns>
    public CombatMoveType GetType()
    {
        return type;
    }

    /// <summary>
    /// Gets the icon associated with the combat move type.
    /// </summary>
    /// <returns>The icon of the combat move type.</returns>
    public Sprite GetIcon()
    {
        return icon;
    }
}