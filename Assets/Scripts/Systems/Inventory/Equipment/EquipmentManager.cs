using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, IDataPersistence, ItemHandler
{
    private InventoryManager inventoryManager; // Reference to the InventoryManager
    private UIEquipmentController equipmentUi; // Reference to the UIEquipmentController
    private GameManager gameManager; // Reference to the GameManager
    private UnitBase playerStats; // Reference to the player's stats

    // References to currently equipped items
    private EquipmentItem currentHeadItem;
    private EquipmentItem currentChestItem;
    private EquipmentItem currentWaistItem;
    private EquipmentItem currentFeetItem;
    private EquipmentItem currentNeckItem;
    private EquipmentItem currentWeaponItem;
    private EquipmentItem currentShieldItem;

    // Initialize references to other managers and controllers
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        equipmentUi = FindObjectOfType<UIEquipmentController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Assign an equipment item to the appropriate slot
    public void AssignEquipmentItem(EquipmentItem item)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();

        // Determine the equipment type and equip the item in the corresponding slot
        switch (item.EquipmentType)
        {
            case EquipmentType.Head:
                EquipItem(ref currentHeadItem, item);
                break;
            case EquipmentType.Chest:
                EquipItem(ref currentChestItem, item);
                break;
            case EquipmentType.Waist:
                EquipItem(ref currentWaistItem, item);
                break;
            case EquipmentType.Feet:
                EquipItem(ref currentFeetItem, item);
                break;
            case EquipmentType.Neck:
                EquipItem(ref currentNeckItem, item);
                break;
            case EquipmentType.Weapon:
                EquipItem(ref currentWeaponItem, item);
                break;
            case EquipmentType.Shield:
                EquipItem(ref currentShieldItem, item);
                break;
        }

        Debug.Log("Equipment Type was not found: " + item.EquipmentType);
    }

    // Unassign (unequip) an item from the specified slot
    public void UnassignEquipmentItem(EquipmentType position)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();

        // Determine the equipment type and unequip the item from the corresponding slot
        switch (position)
        {
            case EquipmentType.Head:
                UnequipItem(ref currentHeadItem);
                break;
            case EquipmentType.Chest:
                UnequipItem(ref currentChestItem);
                break;
            case EquipmentType.Waist:
                UnequipItem(ref currentWaistItem);
                break;
            case EquipmentType.Feet:
                UnequipItem(ref currentFeetItem);
                break;
            case EquipmentType.Neck:
                UnequipItem(ref currentNeckItem);
                break;
            case EquipmentType.Weapon:
                UnequipItem(ref currentWeaponItem);
                break;
            case EquipmentType.Shield:
                UnequipItem(ref currentShieldItem);
                break;
        }
    }

    /*
     * Equips an item in the specified slot and returns the unequipped item
     * Returns null if no item was unequipped.
     */
    private void EquipItem(ref EquipmentItem position, EquipmentItem itemToEquip)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();

        // If there's already an item in the slot, unequip it and add it back to the inventory
        if (position != null)
        {
            EquipmentItem unequippedItem = position;
            position = itemToEquip;
            UpdateStatBonuses(unequippedItem, position);
            equipmentUi.UpdateSelectedSlotOnEquip(itemToEquip);
            inventoryManager.AddItem(unequippedItem, 1);
        }
        else
        {
            // If the slot is empty, simply equip the new item
            position = itemToEquip;
            UpdateStatBonuses(null, position);
            equipmentUi.UpdateSelectedSlotOnEquip(itemToEquip);
        }
    }

    // Unequip an item from the specified slot
    private void UnequipItem(ref EquipmentItem position)
    {
        UpdateStatBonuses(position, null);
        position = null;
    }

    /*
     * Update reference to player's statbase
     * Subtract all stats from unequipped item
     * Add all stats from equipped item to player's stats.
     */
    public void UpdateStatBonuses(EquipmentItem unequippedItem, EquipmentItem equippedItem)
    {
        playerStats = gameManager.PlayerData.unitBase;

        // Remove stat bonuses from the unequipped item
        if (unequippedItem != null)
        {
            Debug.Log("Trying to remove stat bonuses for: " + unequippedItem.EquipmentType);
            unequippedItem.GetAllStatBonuses().ForEach(statBonus =>
            {
                UpdateStat(statBonus, true);
            });
        }

        // Add stat bonuses from the equipped item
        if (equippedItem != null)
        {
            equippedItem.GetAllStatBonuses().ForEach(statBonus =>
            {
                UpdateStat(statBonus, false);
            });
        }
    }

    /*
     * If item is unequipped, subtract the value instead of adding it.
     */
    public void UpdateStat(StatBonus statBonus, bool isUnequipped)
    {
        // If the item is unequipped, invert the stat bonus value
        if (isUnequipped) statBonus.Value *= -1;

        // Apply the stat bonus to the player's stats
        switch (statBonus.StatBonusType)
        {
            case StatBonusType.MaxHp:
                playerStats.MaxHp += Mathf.RoundToInt(statBonus.Value);
                break;
            case StatBonusType.Strength:
                playerStats.Strength += statBonus.Value;
                break;
            case StatBonusType.Agility:
                playerStats.Agility += statBonus.Value;
                break;
            case StatBonusType.Intellect:
                playerStats.Intellect += statBonus.Value;
                break;
            case StatBonusType.AttackPower:
                playerStats.AttackPower += statBonus.Value;
                break;
            case StatBonusType.AbilityPower:
                playerStats.AbilityPower += statBonus.Value;
                break;
            case StatBonusType.PhysCritChance:
                playerStats.Agility += statBonus.Value / 100; // E.g. 1% = + 0.01;
                break;
            case StatBonusType.MagicCritChance:
                playerStats.Agility += statBonus.Value / 100;
                break;
            case StatBonusType.Armor:
                playerStats.PhysicalDefense += statBonus.Value;
                break;
            case StatBonusType.MagicArmor:
                playerStats.MagicalDefense += statBonus.Value;
                break;
            case StatBonusType.Block:
                playerStats.PhysicalBlockPower += statBonus.Value;
                break;
            case StatBonusType.Dodge:
                playerStats.DodgeChance += statBonus.Value / 100;
                break;
            case StatBonusType.Speed:
                playerStats.Speed += statBonus.Value;
                break;
        }

        // Reset the stat bonus value if it was inverted
        if (isUnequipped) statBonus.Value *= -1;
    }

    // Get a list of all currently equipped items
    public List<EquipmentItem> GetAllEquippedItems()
    {
        return new List<EquipmentItem>
        {
            currentHeadItem,
            currentChestItem,
            currentWaistItem,
            currentFeetItem,
            currentNeckItem,
            currentWeaponItem,
            currentShieldItem
        };
    }

    // Load equipment data from saved game data
    public void LoadData(GameData data)
    {
        currentHeadItem = data.EquipmentData.CurrentHeadItem;
        currentChestItem = data.EquipmentData.CurrentChestItem;
        currentWaistItem = data.EquipmentData.CurrentWaistItem;
        currentFeetItem = data.EquipmentData.CurrentFeetItem;
        currentNeckItem = data.EquipmentData.CurrentNeckItem;
        currentWeaponItem = data.EquipmentData.CurrentWeaponItem;
        currentShieldItem = data.EquipmentData.CurrentShieldItem;
    }

    // Save equipment data to game data
    public void SaveData(GameData data)
    {
        data.EquipmentData.CurrentHeadItem = currentHeadItem;
        data.EquipmentData.CurrentChestItem = currentChestItem;
        data.EquipmentData.CurrentWaistItem = currentWaistItem;
        data.EquipmentData.CurrentFeetItem = currentFeetItem;
        data.EquipmentData.CurrentNeckItem = currentNeckItem;
        data.EquipmentData.CurrentWeaponItem = currentWeaponItem;
        data.EquipmentData.CurrentShieldItem = currentShieldItem;
    }
}