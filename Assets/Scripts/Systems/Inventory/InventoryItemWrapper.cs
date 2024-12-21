using System;
using UnityEngine;

// A serializable class that wraps an inventory item and its count
[Serializable]
public class InventoryItemWrapper
{
    [SerializeField] private InventoryItem item; // The inventory item being wrapped
    [SerializeField] private int count;          // The number of this item in the inventory

    // Property to get or set the inventory item
    public InventoryItem Item
    {
        get => item;
        set => item = value;
    }

    // Property to get or set the count of the item
    public int Count
    {
        get => count;
        set => count = value;
    }
}