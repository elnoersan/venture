using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : PersistentSingleton<InventoryManager>, IDataPersistence
{
    [SerializeField] private List<InventoryItemWrapper> items = new List<InventoryItemWrapper>(); // List of items in the inventory
    [SerializeField] private bool isDebug; // Debug flag to spawn items
    private EquipmentManager equipmentManager; // Reference to the EquipmentManager
    private UIInventoryController inventoryUI; // Reference to the UIInventoryController

    // Dictionary to map items to their counts
    private Dictionary<InventoryItem, int> itemCountMap = new Dictionary<InventoryItem, int>();

    // Initialize references and inventory
    private void Start()
    {
        equipmentManager = FindObjectOfType<EquipmentManager>();
        inventoryUI = FindObjectOfType<UIInventoryController>();
        InitInventory();
    }

    // Initialize the inventory
    public void InitInventory()
    {
        // If in debug mode or the item count map is empty, spawn items
        if (isDebug || itemCountMap.Count == 0) SpawnItems();
        inventoryUI.InitInventoryUI(); // Initialize the inventory UI
    }

    // Spawn items into the inventory
    private void SpawnItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemCountMap.Add(items[i].Item, items[i].Count); // Add items to the item count map
        }
    }

    // Use an item from the inventory
    public void UseItem(InventoryItem item)
    {
        // If the item is equipment, switch it with the equipped item in the slot
        // If the item is consumable, remove it from the inventory, destroy it, and apply its effects
        Debug.Log(string.Format("Used item: {0}", item.ItemName));
        if (item.ItemType == ItemType.Equipment)
        {
            RemoveItem(item, 1); // Remove the item from the inventory
            item.UseItem(); // Use the item (e.g., equip it)
        }

        if (item.ItemType == ItemType.Consumable)
        {
            RemoveItem(item, 1); // Remove the item from the inventory
            item.UseItem(); // Use the item (e.g., consume it)
        }
    }

    // Add an item to the inventory
    public void AddItem(InventoryItem item, int amountToAdd)
    {
        int currentItemCount;

        // Check if the item already exists in the inventory
        if (itemCountMap.TryGetValue(item, out currentItemCount))
        {
            itemCountMap[item] = currentItemCount + amountToAdd; // Update the item count
        }
        else
        {
            itemCountMap.Add(item, amountToAdd); // Add the item to the inventory
        }

        inventoryUI.CreateOrUpdateSlot(item, amountToAdd); // Update the UI to reflect the change
    }

    // Remove an item from the inventory
    public void RemoveItem(InventoryItem item, int amountToRemove)
    {
        inventoryUI = FindObjectOfType<UIInventoryController>(); // Re-fetch the UI reference

        int currentItemCount;

        // Check if the item exists in the inventory
        if (itemCountMap.TryGetValue(item, out currentItemCount))
        {
            itemCountMap[item] = currentItemCount - amountToRemove; // Update the item count
            if (currentItemCount - amountToRemove <= 0)
            {
                inventoryUI.DestroySlot(item); // Remove the item from the UI if its count is 0
            }
            else
            {
                inventoryUI.UpdateSlot(item, currentItemCount - amountToRemove); // Update the UI slot
            }
        }
        else
        {
            Debug.Log(string.Format("Cannot remove item: {0}. Item was not found in inventory", item.ItemName));
        }
    }

    // Property to get or set the list of items
    public List<InventoryItemWrapper> Items
    {
        get => items;
        set => items = value;
    }

    // Property to get or set the item count map
    public Dictionary<InventoryItem, int> ItemCountMap
    {
        get => itemCountMap;
        set => itemCountMap = value;
    }

    // Load inventory data from saved game data
    public void LoadData(GameData data)
    {
        var inventoryMap = new Dictionary<InventoryItem, int>();

        // Populate the inventory map with saved data
        foreach (var keyValuePair in data.InventoryData.InventoryMap)
        {
            inventoryMap.Add(keyValuePair.Key, keyValuePair.Value);
        }

        ItemCountMap = inventoryMap; // Set the item count map to the loaded data
    }

    // Save inventory data to game data
    public void SaveData(GameData data)
    {
        data.InventoryData.ResetBeforeSave(); // Clear the inventory data before saving

        // Save the current item count map to the game data
        foreach (var keyValuePair in ItemCountMap)
        {
            data.InventoryData.InventoryMap.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}