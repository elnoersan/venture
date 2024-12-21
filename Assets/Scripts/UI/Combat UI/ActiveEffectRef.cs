using UnityEngine;

// A class that holds a reference to an active combat effect
public class ActiveEffectRef : MonoBehaviour
{
    [SerializeField] private CombatEffect reference; // The combat effect being referenced

    // Property to get or set the combat effect reference
    public CombatEffect Reference
    {
        get => reference;
        set => reference = value;
    }
}