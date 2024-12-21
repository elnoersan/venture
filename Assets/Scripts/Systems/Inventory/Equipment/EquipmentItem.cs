using System;
using System.Collections.Generic;
using UnityEngine;

// Create a menu option in Unity to create an Equipment Item asset
[CreateAssetMenu(menuName = "Inventory System/Items/Create Equipment Item")]
// Make the class serializable so it can be saved and loaded
[Serializable]
public class EquipmentItem : InventoryItem
{
    [SerializeField] private EquipmentType equipmentType; // The type of equipment (e.g., Weapon, Armor)
    [SerializeField] private List<StatBonus> statBonuses; // A list of stat bonuses provided by the equipment

    // Constructor to set the item type to Equipment
    public EquipmentItem()
    {
        itemType = ItemType.Equipment;
    }

    // Override the UseItem method to handle using the equipment item
    public override void UseItem()
    {
        // Find the EquipmentManager in the scene and assign this equipment item to it
        FindObjectOfType<EquipmentManager>().AssignEquipmentItem(this);
    }

    // Property to get or set the equipment type
    public EquipmentType EquipmentType
    {
        get => equipmentType;
        set => equipmentType = value;
    }

    // Method to get all stat bonuses provided by the equipment
    public List<StatBonus> GetAllStatBonuses()
    {
        return statBonuses;
    }
}