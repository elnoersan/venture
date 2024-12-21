using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplayHighlighter : MonoBehaviour
{
    [SerializeField] private float highlightZoom = 1.2f; // The zoom level for highlighting a targeted enemy

    private CombatSystem combatSystem; // Reference to the CombatSystem
    private UIPlayerInputController inputController; // Reference to the UIPlayerInputController
    private int referencedCount = -1; // Tracks the number of active enemies to avoid unnecessary updates
    private List<UIStatDisplay> allEnemyStatDisplays; // List of all enemy stat displays
    private List<UIStatDisplay> activeStatDisplays; // List of active (alive) enemy stat displays

    // Initialize references and populate the list of enemy stat displays
    private void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        inputController = FindObjectOfType<UIPlayerInputController>();

        // Find all UIStatDisplay components in the scene
        allEnemyStatDisplays = FindObjectsOfType<UIStatDisplay>().ToList();

        // Remove the player's stat display from the list
        var playerStatDisplay = allEnemyStatDisplays.Find(statDisplay => statDisplay.ConnectedUnit.IsPlayerUnit);
        allEnemyStatDisplays.Remove(playerStatDisplay);
    }

    // Update the highlights and inactive displays during the PLAYER_TARGET_SELECT state
    void Update()
    {
        if (combatSystem.State != CombatState.PLAYER_TARGET_SELECT) return;

        UpdateHighlights();
        UpdateInactiveDisplays();
    }

    // Updates the highlights for the targeted enemy
    private void UpdateHighlights()
    {
        // If the number of active enemies hasn't changed, only update the highlight for the currently targeted enemy
        if (inputController.MaxTargetIndex == referencedCount)
        {
            int indexOfEnemyToHighlight = inputController.TargetIndex;
            HighlightTargetedEnemy(indexOfEnemyToHighlight);
        }
        else
        {
            // If the number of active enemies has changed, update the list of active stat displays
            referencedCount = FindActiveStatDisplays();
        }
    }

    // Updates the appearance of inactive (dead) enemy stat displays
    private void UpdateInactiveDisplays()
    {
        allEnemyStatDisplays.ForEach(statDisplay =>
        {
            // Skip if the connected unit is still alive or if the UI element doesn't have enough children
            if (statDisplay.ConnectedUnit.IsAlive) return;
            if (statDisplay.gameObject.GetComponentsInChildren<RectTransform>().Length < 4) return;

            // Disable the fourth child (assumed to be a highlight or overlay) and gray out the display
            statDisplay.gameObject.GetComponentsInChildren<RectTransform>()[3].gameObject.SetActive(false);
            statDisplay.gameObject.GetComponentInChildren<Image>().color = Color.grey;
            statDisplay.transform.localScale = new Vector3(1, 1);
        });
    }

    // Finds and returns the number of active (alive) enemy stat displays
    private int FindActiveStatDisplays()
    {
        // Find all UIStatDisplay components in the scene and filter by alive units
        activeStatDisplays = FindObjectsOfType<UIStatDisplay>().ToList().FindAll(statDisplay => statDisplay.ConnectedUnit.IsAlive);
        Debug.Log("Active stat displays: " + FindObjectsOfType<UIStatDisplay>());

        // Remove the player's stat display from the list
        var playerStatDisplay = activeStatDisplays.Find(statDisplay => statDisplay.ConnectedUnit.IsPlayerUnit);
        activeStatDisplays.Remove(playerStatDisplay);

        // Reverse the list to match the expected order
        activeStatDisplays.Reverse();

        // Return the maximum index of active stat displays
        return activeStatDisplays.Count - 1;
    }

    // Highlights the targeted enemy by zooming in its stat display
    private void HighlightTargetedEnemy(int indexOfEnemyToHighlight)
    {
        Debug.Log("Active stat displays: " + activeStatDisplays.Count);

        // Loop through all active stat displays
        for (int i = 0; i < activeStatDisplays.Count; i++)
        {
            if (i == indexOfEnemyToHighlight)
            {
                // Zoom in the targeted enemy's stat display
                activeStatDisplays[i].transform.localScale = new Vector3(highlightZoom, highlightZoom);
            }
            else
            {
                // Reset the scale for other stat displays
                activeStatDisplays[i].transform.localScale = new Vector3(1, 1);
            }
        }
    }

    // Resets the highlights for all active stat displays
    public void ResetHighlights()
    {
        activeStatDisplays.ForEach(activeDisplay => activeDisplay.transform.localScale = new Vector3(1, 1));
    }
}