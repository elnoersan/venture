using System;
using UnityEngine;

/// <summary>
/// Represents a consumable item in the inventory system.
/// This class inherits from InventoryItem and adds properties specific to consumable items, such as type and value.
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Items/Create Consumable Item")]
[Serializable]
public class ConsumableItem : InventoryItem
{
    [SerializeField] private ConsumableType type; // The type of the consumable item.
    [SerializeField] private int value; // The value or effect of the consumable item.

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsumableItem"/> class.
    /// Sets the item type to Consumable.
    /// </summary>
    public ConsumableItem()
    {
        itemType = ItemType.Consumable;
    }

    /// <summary>
    /// Uses the consumable item by invoking the ConsumableManager's UseItem method.
    /// </summary>
    public override void UseItem()
    {
        FindObjectOfType<ConsumableManager>().UseItem(this);
    }

    /// <summary>
    /// Gets or sets the type of the consumable item.
    /// </summary>
    public ConsumableType Type
    {
        get => type;
        set => type = value;
    }

    /// <summary>
    /// Gets or sets the value or effect of the consumable item.
    /// </summary>
    public int Value
    {
        get => this.value;
        set => this.value = value;
    }
}