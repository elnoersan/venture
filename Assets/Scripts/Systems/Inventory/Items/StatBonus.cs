using System;
using UnityEngine;

// A serializable class that represents a bonus to a specific stat
[Serializable]
public class StatBonus
{
    [SerializeField] private StatBonusType statBonusType; // The type of stat being modified (e.g., Strength, Agility)
    [SerializeField] private float value; // The value of the bonus (e.g., +10 Strength)

    // Constructor to initialize the StatBonus with a specific type and value
    public StatBonus(StatBonusType statBonusType, float value)
    {
        this.statBonusType = statBonusType;
        this.value = value;
    }

    // Property to get or set the type of stat bonus
    public StatBonusType StatBonusType
    {
        get => statBonusType;
        set => statBonusType = value;
    }

    // Property to get or set the value of the stat bonus
    public float Value
    {
        get => value;
        set => this.value = value;
    }
}