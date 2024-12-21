using System;
using UnityEngine;

// Serializable class representing an equipment slot in the UI
[Serializable]
public class UIEquipmentSlot
{
    [SerializeField] private GameObject unequippedIcon; // The icon displayed when the slot is empty
    [SerializeField] private GameObject equipmentIcon; // The icon displayed when an item is equipped
    [SerializeField] private EquipmentType equipmentType; // The type of equipment this slot can hold

    // Property for accessing the unequipped icon
    public GameObject UnequippedIcon
    {
        get => unequippedIcon;
        set => unequippedIcon = value;
    }

    // Property for accessing the equipment icon
    public GameObject EquipmentIcon
    {
        get => equipmentIcon;
        set => equipmentIcon = value;
    }

    // Property for accessing the equipment type
    public EquipmentType EquipmentType
    {
        get => equipmentType;
        set => equipmentType = value;
    }
}