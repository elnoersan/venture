using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Multiline()] public string body; // The body text of the tooltip, using Multiline attribute for better editing in the Inspector
    public string subtitle; // The subtitle text of the tooltip
    public string title; // The title text of the tooltip

    // Called when the pointer (mouse cursor) enters the trigger area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the tooltip with the specified body, subtitle, and title
        UITooltipController.Show(body, subtitle, title);
    }

    // Called when the pointer (mouse cursor) exits the trigger area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip
        UITooltipController.Hide();
    }
}