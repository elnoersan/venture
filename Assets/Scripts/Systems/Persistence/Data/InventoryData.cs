using System;
using UnityEngine;

// A serializable class that represents inventory data for saving and loading
[Serializable]
public class InventoryData : SaveData
{
    [SerializeField] private SerializableDictionary<InventoryItem, int> inventoryMap; // A dictionary mapping inventory items to their counts

    // Constructor to initialize the inventory data
    public InventoryData(SerializableDictionary<InventoryItem, int> inventoryMap)
    {
        this.inventoryMap = inventoryMap;
    }

    // Property to get or set the inventory map
    public SerializableDictionary<InventoryItem, int> InventoryMap
    {
        get => inventoryMap;
        set => inventoryMap = value;
    }

    // Method to reset the inventory data before saving
    public void ResetBeforeSave()
    {
        inventoryMap = new SerializableDictionary<InventoryItem, int>(); // Clear the inventory map
    }
}