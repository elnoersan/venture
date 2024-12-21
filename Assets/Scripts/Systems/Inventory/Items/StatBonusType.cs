using System;

// A serializable enum that defines the different types of stat bonuses
[Serializable]
public enum StatBonusType
{
    MaxHp,           // Bonus to the maximum HP of a unit
    Strength,        // Bonus to the strength stat (e.g., physical damage)
    Agility,         // Bonus to the agility stat (e.g., speed, dodge chance)
    Intellect,       // Bonus to the intellect stat (e.g., magical damage)
    AttackPower,     // Bonus to attack power (e.g., physical damage output)
    AbilityPower,    // Bonus to ability power (e.g., magical damage output)
    PhysCritChance,  // Bonus to physical critical hit chance
    MagicCritChance, // Bonus to magical critical hit chance
    Armor,           // Bonus to physical armor (reduces physical damage)
    MagicArmor,      // Bonus to magical armor (reduces magical damage)
    Block,           // Bonus to block chance or block amount
    Dodge,           // Bonus to dodge chance
    Speed            // Bonus to movement or attack speed
}