using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//[ExecuteInEditMode()]
public class UITooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title; // Text component for the tooltip title
    [SerializeField] private TextMeshProUGUI subtitle; // Text component for the tooltip subtitle
    [SerializeField] private TextMeshProUGUI body; // Text component for the tooltip body
    [SerializeField] private LayoutElement layoutElement; // LayoutElement component to control wrapping
    [SerializeField] private int characterWrapLimit; // Maximum number of characters before wrapping

    // Deactivate the tooltip on start
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update the tooltip position and size every frame
    private void Update()
    {
        // Resize the tooltip in the editor
        if (Application.isEditor) ResizePreferredSize();

        // Get the mouse position in world coordinates
        Vector2 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Convert the world position to screen coordinates
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

        // Calculate the pivot point for the tooltip based on the screen position
        float pivotX = (screenPoint.x / Screen.width);
        float pivotY = (screenPoint.y / Screen.height);

        // Set the pivot and position of the tooltip
        this.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    // Resizes the tooltip based on the character wrap limit
    private void ResizePreferredSize()
    {
        // Get the length of the title, subtitle, and body text
        int titleLength = title.text.Length;
        int subtitleLength = subtitle.text.Length;
        int bodyLength = body.text.Length;

        // Enable or disable the layout element based on the character wrap limit
        layoutElement.enabled =
            (titleLength > characterWrapLimit || subtitleLength > characterWrapLimit || bodyLength > characterWrapLimit);
    }

    // Sets the text for the tooltip
    public void SetText(string body, string subtitle = "", string title = "")
    {
        // Hide the title if it's empty
        if (string.IsNullOrEmpty(title))
        {
            this.title.gameObject.SetActive(false);
        }
        else
        {
            // Show the title and set its text
            this.title.gameObject.SetActive(true);
            this.title.text = title;
        }

        // Hide the subtitle if it's empty
        if (string.IsNullOrEmpty(subtitle))
        {
            this.subtitle.gameObject.SetActive(false);
        }
        else
        {
            // Show the subtitle and set its text
            this.subtitle.gameObject.SetActive(true);
            this.subtitle.text = subtitle;
        }

        // Set the body text
        this.body.text = body;

        // Resize the tooltip based on the text length
        ResizePreferredSize();
    }
}