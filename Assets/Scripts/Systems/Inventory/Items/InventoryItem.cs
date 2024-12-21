using System;
using UnityEngine;

// A serializable abstract class that serves as the base class for all inventory items
[Serializable]
public abstract class InventoryItem : ScriptableObject
{
    [SerializeField] protected Sprite itemSprite; // The sprite representing the item in the UI
    [SerializeField] protected string itemName;  // The name of the item
    [SerializeField] protected TooltipInfo tooltipInfo; // Information displayed in the tooltip when hovering over the item
    protected ItemType itemType; // The type of the item (e.g., Equipment, Consumable)

    // Abstract method that must be implemented by derived classes to define how the item is used
    public abstract void UseItem();

    // Property to get or set the item's sprite
    public Sprite ItemSprite
    {
        get => itemSprite;
        set => itemSprite = value;
    }

    // Property to get or set the item's name
    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    // Property to get or set the item's tooltip information
    public TooltipInfo TooltipInfo
    {
        get => tooltipInfo;
        set => tooltipInfo = value;
    }

    // Property to get or set the item's type
    public ItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }
}