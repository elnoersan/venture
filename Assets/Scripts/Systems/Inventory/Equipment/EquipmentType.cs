using System;

// A serializable enum that defines different types of equipment
[Serializable]
public enum EquipmentType
{
    Head,   // Equipment for the head (e.g., helmets)
    Chest,  // Equipment for the chest (e.g., armor)
    Waist,  // Equipment for the waist (e.g., belts)
    Feet,   // Equipment for the feet (e.g., boots)
    Neck,   // Equipment for the neck (e.g., necklaces)
    Weapon, // Equipment for the weapon slot (e.g., swords, bows)
    Shield  // Equipment for the shield slot (e.g., shields)
}