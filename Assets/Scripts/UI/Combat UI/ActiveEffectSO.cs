using UnityEngine;

// A scriptable object that represents an active effect in the game
[CreateAssetMenu(fileName = "ActiveEffectDisplay", menuName = "Combat/Create new Active Effect SO")]
public class ActiveEffectSO : ScriptableObject
{
    [SerializeField] private CombatEffectType type; // The type of combat effect (e.g., buff, debuff)
    [SerializeField] private Sprite icon; // The icon representing the combat effect

    // Property to get or set the type of combat effect
    public CombatEffectType Type
    {
        get => type;
        set => type = value;
    }

    // Property to get or set the icon representing the combat effect
    public Sprite Icon
    {
        get => icon;
        set => icon = value;
    }
}