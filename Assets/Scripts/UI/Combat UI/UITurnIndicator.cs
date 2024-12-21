using UnityEngine;
using UnityEngine.UI;

public class UITurnIndicator : MonoBehaviour
{
    [SerializeField] private Image leftIndicator; // The left indicator image in the UI
    [SerializeField] private Image rightIndicator; // The right indicator image in the UI
    [SerializeField] private Sprite greyIndicator; // Sprite for the grey (inactive) indicator
    [SerializeField] private Sprite greenIndicator; // Sprite for the green (active) indicator

    private CombatSystem combatSystem; // Reference to the CombatSystem

    // Initialize the CombatSystem reference
    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
    }

    // Update the UI indicators based on the remaining player actions
    private void Update()
    {
        // Switch statement to handle the different cases for remaining player actions
        switch (combatSystem.RemainingPlayerActions)
        {
            // Case 0: No actions remaining
            case 0:
                leftIndicator.sprite = greyIndicator; // Set both indicators to grey
                rightIndicator.sprite = greyIndicator;
                break;

            // Case 1: One action remaining
            case 1:
                leftIndicator.sprite = greyIndicator; // Set the left indicator to grey
                rightIndicator.sprite = greenIndicator; // Set the right indicator to green
                break;

            // Case 2: Two actions remaining
            case 2:
                leftIndicator.sprite = greenIndicator; // Set both indicators to green
                rightIndicator.sprite = greenIndicator;
                break;
        }
    }
}