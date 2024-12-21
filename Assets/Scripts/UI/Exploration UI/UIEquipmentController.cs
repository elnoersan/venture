using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentController : MonoBehaviour
{
    [SerializeField] private UIEquipmentSlot headSlot; // Slot for the head equipment
    [SerializeField] private UIEquipmentSlot chestSlot; // Slot for the chest equipment
    [SerializeField] private UIEquipmentSlot waistSlot; // Slot for the waist equipment
    [SerializeField] private UIEquipmentSlot feetSlot; // Slot for the feet equipment
    [SerializeField] private UIEquipmentSlot neckSlot; // Slot for the neck equipment
    [SerializeField] private UIEquipmentSlot weaponSlot; // Slot for the weapon equipment
    [SerializeField] private UIEquipmentSlot shieldSlot; // Slot for the shield equipment

    private EquipmentManager equipmentManager; // Reference to the EquipmentManager
    private List<UIEquipmentSlot> allSlots; // List of all equipment slots

    // Initialize the EquipmentManager and set up the UI slots
    private void Start()
    {
        equipmentManager = FindObjectOfType<EquipmentManager>();

        // Populate the list of all equipment slots
        allSlots = new List<UIEquipmentSlot>
        {
            headSlot,
            chestSlot,
            waistSlot,
            feetSlot,
            neckSlot,
            weaponSlot,
            shieldSlot
        };

        // Initially hide all equipment icons
        allSlots.ForEach(uiSlot => uiSlot.EquipmentIcon.gameObject.SetActive(false));

        // Load and display the currently equipped items
        LoadEquipment();
    }

    // Loads and displays all currently equipped items in the UI
    private void LoadEquipment()
    {
        // Iterate through all equipped items and update the UI slots
        equipmentManager.GetAllEquippedItems().ForEach(equippedItem =>
        {
            if (equippedItem != null)
            {
                UpdateSelectedSlotOnEquip(equippedItem);
            }
        });
    }

    // Updates the UI slot when an item is equipped
    public void UpdateSelectedSlotOnEquip(EquipmentItem item)
    {
        // Iterate through all UI slots
        allSlots.ForEach(uiSlot =>
        {
            // If the slot type matches the item type, update the slot
            if (uiSlot.EquipmentType != item.EquipmentType) return;

            // Show the equipment icon and set its sprite
            ToggleUISlot(uiSlot, true);
            SetTooltip(uiSlot, item.TooltipInfo);
            uiSlot.EquipmentIcon.GetComponent<Image>().sprite = item.ItemSprite;

            // Add a button listener to handle unequipping the item
            GameObject iconGO = uiSlot.EquipmentIcon;
            if (iconGO.GetComponent<Button>() != null)
            {
                Button button = iconGO.GetComponent<Button>();

                // Remove existing listeners and add a new one for unequipping
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UnequipItemAction(uiSlot, item));
            }
            else
            {
                // Add a new button component if it doesn't exist
                uiSlot.EquipmentIcon.AddComponent<Button>().onClick.AddListener(() => UnequipItemAction(uiSlot, item));
            }
        });
    }

    // Handles the unequipping of an item
    private void UnequipItemAction(UIEquipmentSlot uiSlot, EquipmentItem item)
    {
        // Remove the button component from the equipment icon
        Destroy(uiSlot.EquipmentIcon.GetComponent<Button>());

        // Hide the equipment icon and show the unequipped icon
        ToggleUISlot(uiSlot, false);

        // Add the item back to the inventory and unassign it from the equipment manager
        FindObjectOfType<InventoryManager>().AddItem(item, 1);
        FindObjectOfType<EquipmentManager>().UnassignEquipmentItem(uiSlot.EquipmentType);
    }

    // Toggles the visibility of the equipment icon and unequipped icon
    private void ToggleUISlot(UIEquipmentSlot uiSlot, bool hasEquipmentInSlot)
    {
        uiSlot.UnequippedIcon.SetActive(!hasEquipmentInSlot);
        uiSlot.EquipmentIcon.SetActive(hasEquipmentInSlot);
    }

    // Sets the tooltip for the equipment slot
    public void SetTooltip(UIEquipmentSlot slot, TooltipInfo content)
    {
        // Add a UITooltipTrigger component to the equipment icon and set its content
        UITooltipTrigger uiTooltipTrigger = slot.EquipmentIcon.gameObject.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = content.Title;
        uiTooltipTrigger.subtitle = content.Subtitle;
        uiTooltipTrigger.body = content.Body;
    }
}

// Listener on equipped item refers to an item that has been destroyed. The listener should be recreated at the start.