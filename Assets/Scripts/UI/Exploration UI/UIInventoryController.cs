using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer; // The container for inventory slots
    [SerializeField] private UIInventorySlot itemSlotPrefab; // The prefab for individual inventory slots

    private InventoryManager inventoryManager; // Reference to the InventoryManager
    private Dictionary<InventoryItem, UIInventorySlot> itemSlotMap = new Dictionary<InventoryItem, UIInventorySlot>(); // Maps items to their UI slots

    // Initialize the InventoryManager and set up the inventory UI
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        InitInventoryUI();
    }

    // Initializes the inventory UI by creating or updating slots for all items in the inventory
    public void InitInventoryUI()
    {
        var itemsMap = inventoryManager.ItemCountMap; // Get the item count map from the InventoryManager

        // Iterate through all items in the inventory and create or update their slots
        foreach (var kvp in itemsMap)
        {
            CreateOrUpdateSlot(kvp.Key, kvp.Value);
        }
    }

    // Creates or updates a slot for a specific item
    public void CreateOrUpdateSlot(InventoryItem item, int amount)
    {
        // If the amount is less than or equal to 0, return (no need to display the item)
        if (amount <= 0) return;

        // If the item is not already in the UI, create a new slot for it
        if (!itemSlotMap.ContainsKey(item))
        {
            var slot = CreateSlot(item, amount);
            itemSlotMap.Add(item, slot);
        }
        else
        {
            // If the item is already in the UI, update its slot with the new amount
            UpdateSlot(item, amount);
        }
    }

    // Creates a new slot for an item
    private UIInventorySlot CreateSlot(InventoryItem item, int amount)
    {
        // Instantiate the slot prefab as a child of the items container
        var slot = Instantiate(itemSlotPrefab, itemsContainer);

        // Initialize the slot's visuals (sprite and count)
        slot.InitSlotVisuals(item.ItemSprite, amount);

        // Set the tooltip for the slot
        slot.SetTooltip(item.TooltipInfo);

        // Assign a callback to the slot's button to use the item
        slot.AssignSlotButtonCallback(() => inventoryManager.UseItem(item));

        return slot;
    }

    // Updates the count of an existing slot
    public void UpdateSlot(InventoryItem item, int itemCount)
    {
        itemSlotMap[item].UpdateSlotCount(itemCount);
    }

    // Destroys a slot for a specific item
    public void DestroySlot(InventoryItem item)
    {
        // Destroy the slot's GameObject and remove it from the map
        Destroy(itemSlotMap[item].gameObject);
        itemSlotMap.Remove(item);
    }
}