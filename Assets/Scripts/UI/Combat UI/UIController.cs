using UnityEngine;

// A class that manages the UI during combat
public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject skillSelectUI; // The UI panel for selecting skills
    [SerializeField] private GameObject actionSelectUI; // The UI panel for selecting actions (attack, defend, support)
    [SerializeField] private GameObject playerCursor; // The player cursor used for targeting
    private CombatAction activeAction; // The currently active combat action
    private CombatSystem _combatSystem; // Reference to the CombatSystem for managing combat state

    // Initialize the UIController
    private void Awake()
    {
        _combatSystem = FindObjectOfType<CombatSystem>(); // Find the CombatSystem in the scene
    }

    // Update the UI based on the current combat state
    private void Update()
    {
        switch (_combatSystem.State)
        {
            case CombatState.PLAYER_ACTION_SELECT:
                // Show the action selection UI and hide the skill selection UI and player cursor
                actionSelectUI.SetActive(true);
                skillSelectUI.SetActive(false);
                playerCursor.SetActive(true);
                break;
            case CombatState.PLAYER_SKILL_SELECT:
                // Show both the action selection and skill selection UIs, and hide the player cursor
                actionSelectUI.SetActive(true);
                skillSelectUI.SetActive(true);
                playerCursor.SetActive(false);
                break;
            case CombatState.PLAYER_TARGET_SELECT:
                // Show the action selection UI and player cursor, and hide the skill selection UI
                actionSelectUI.SetActive(true);
                skillSelectUI.SetActive(false);
                playerCursor.SetActive(true);
                break;
            default:
                // Hide all UIs and the player cursor for other combat states
                actionSelectUI.SetActive(false);
                skillSelectUI.SetActive(false);
                playerCursor.SetActive(false);
                break;
        }
    }
}