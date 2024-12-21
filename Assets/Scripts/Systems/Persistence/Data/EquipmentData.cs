using System;
using UnityEngine;

// A serializable class that represents equipment data for saving and loading
[Serializable]
public class EquipmentData : SaveData
{
    [SerializeField] private EquipmentItem currentHeadItem; // The currently equipped head item
    [SerializeField] private EquipmentItem currentChestItem; // The currently equipped chest item
    [SerializeField] private EquipmentItem currentWaistItem; // The currently equipped waist item
    [SerializeField] private EquipmentItem currentFeetItem; // The currently equipped feet item
    [SerializeField] private EquipmentItem currentNeckItem; // The currently equipped neck item
    [SerializeField] private EquipmentItem currentWeaponItem; // The currently equipped weapon item
    [SerializeField] private EquipmentItem currentShieldItem; // The currently equipped shield item

    // Constructor to initialize the equipment data
    public EquipmentData(EquipmentItem currentHeadItem, EquipmentItem currentChestItem, EquipmentItem currentWaistItem, EquipmentItem currentFeetItem, EquipmentItem currentNeckItem, EquipmentItem currentWeaponItem, EquipmentItem currentShieldItem)
    {
        this.currentHeadItem = currentHeadItem;
        this.currentChestItem = currentChestItem;
        this.currentWaistItem = currentWaistItem;
        this.currentFeetItem = currentFeetItem;
        this.currentNeckItem = currentNeckItem;
        this.currentWeaponItem = currentWeaponItem;
        this.currentShieldItem = currentShieldItem;
    }

    // Property to get or set the currently equipped head item
    public EquipmentItem CurrentHeadItem
    {
        get => currentHeadItem;
        set => currentHeadItem = value;
    }

    // Property to get or set the currently equipped chest item
    public EquipmentItem CurrentChestItem
    {
        get => currentChestItem;
        set => currentChestItem = value;
    }

    // Property to get or set the currently equipped waist item
    public EquipmentItem CurrentWaistItem
    {
        get => currentWaistItem;
        set => currentWaistItem = value;
    }

    // Property to get or set the currently equipped feet item
    public EquipmentItem CurrentFeetItem
    {
        get => currentFeetItem;
        set => currentFeetItem = value;
    }

    // Property to get or set the currently equipped neck item
    public EquipmentItem CurrentNeckItem
    {
        get => currentNeckItem;
        set => currentNeckItem = value;
    }

    // Property to get or set the currently equipped weapon item
    public EquipmentItem CurrentWeaponItem
    {
        get => currentWeaponItem;
        set => currentWeaponItem = value;
    }

    // Property to get or set the currently equipped shield item
    public EquipmentItem CurrentShieldItem
    {
        get => currentShieldItem;
        set => currentShieldItem = value;
    }

    // Method to reset data before saving (not implemented yet)
    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}