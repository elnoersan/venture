using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage; // The image component for displaying the item's sprite
    [SerializeField] private TextMeshProUGUI countText; // The text component for displaying the item's count
    [SerializeField] private Button slotButton; // The button component for interacting with the slot

    // Initializes the slot's visuals (sprite and count)
    public void InitSlotVisuals(Sprite itemSprite, int count)
    {
        itemImage.sprite = itemSprite; // Set the item's sprite
        UpdateSlotCount(count); // Update the item's count
    }

    // Updates the count text of the slot
    public void UpdateSlotCount(int count)
    {
        countText.text = count.ToString(); // Set the count text to the provided value
    }

    // Assigns a callback to the slot's button
    public void AssignSlotButtonCallback(System.Action onClickCallback)
    {
        slotButton.onClick.AddListener(() => onClickCallback()); // Add a listener to the button's onClick event
    }

    // Sets the tooltip for the slot
    public void SetTooltip(TooltipInfo content)
    {
        // Add a UITooltipTrigger component to the slot and set its content
        UITooltipTrigger uiTooltipTrigger = this.gameObject.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = content.Title;
        uiTooltipTrigger.subtitle = content.Subtitle;
        uiTooltipTrigger.body = content.Body;
    }
}