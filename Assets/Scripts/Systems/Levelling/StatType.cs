using System;

// A serializable enum that defines the different types of stats
[Serializable]
public enum StatType
{
    Strength,  // Represents the strength stat, which might affect physical damage or other related attributes
    Agility,   // Represents the agility stat, which might affect speed, dodge chance, or other related attributes
    Intellect  // Represents the intellect stat, which might affect magical damage, mana, or other related attributes
}