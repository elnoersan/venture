using UnityEngine;

public class UITooltipController : MonoBehaviour
{
    private static UITooltipController current; // Static reference to the current instance of the UITooltipController

    [SerializeField] private UITooltip tooltip; // Reference to the UITooltip component

    // Initialize the static reference to this instance
    private void Awake()
    {
        current = this;
    }

    // Shows the tooltip with the specified content
    public static void Show(string body, string subtitle = "", string title = "")
    {
        // Set the tooltip text and activate the tooltip
        current.tooltip.SetText(body, subtitle, title);
        current.tooltip.gameObject.SetActive(true);
    }

    // Hides the tooltip
    public static void Hide()
    {
        // Deactivate the tooltip
        current.tooltip.gameObject.SetActive(false);
    }
}