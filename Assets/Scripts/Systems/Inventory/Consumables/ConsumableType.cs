using System;

// A serializable enum that defines different types of consumable items
[Serializable]
public enum ConsumableType
{
    HpRecovery,   // Consumable that restores health points (HP)
    StatpointUp,  // Consumable that increases the player's stat points
    StrengthUp,   // Consumable that increases the player's strength stat
    AgilityUp,    // Consumable that increases the player's agility stat
    IntellectUp   // Consumable that increases the player's intellect stat
}