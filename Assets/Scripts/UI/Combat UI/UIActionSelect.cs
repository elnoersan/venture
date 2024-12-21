using UnityEngine.UI;
using UnityEngine;

// A class that manages the UI for selecting combat actions
public class UIActionSelect : MonoBehaviour
{
    [SerializeField] private Button attackButton; // Button for selecting the attack action
    [SerializeField] private Button defendButton; // Button for selecting the defend action
    [SerializeField] private Button supportButton; // Button for selecting the support action
    private CombatAction lastAction; // The last selected combat action
    private UISkillLoader _uiSkillLoader; // Reference to the UISkillLoader for displaying skills
    private CombatSystem _combatSystem; // Reference to the CombatSystem for managing combat state

    // Initialize references
    private void Awake()
    {
        _uiSkillLoader = FindObjectOfType<UISkillLoader>(); // Find the UISkillLoader in the scene
        _combatSystem = FindObjectOfType<CombatSystem>(); // Find the CombatSystem in the scene
    }

    // Called when the attack button is selected
    public void OnAttackSelect()
    {
        lastAction = CombatAction.ATTACK; // Set the last action to attack
        _uiSkillLoader.InitiateCombatMoves(lastAction); // Initialize the combat moves for the attack action
        ShowSkillSelect(); // Show the skill selection UI
    }

    // Called when the defend button is selected
    public void OnDefendSelect()
    {
        lastAction = CombatAction.DEFEND; // Set the last action to defend
        _uiSkillLoader.InitiateCombatMoves(lastAction); // Initialize the combat moves for the defend action
        ShowSkillSelect(); // Show the skill selection UI
    }

    // Called when the support button is selected
    public void OnSupportSelect()
    {
        lastAction = CombatAction.SUPPORT; // Set the last action to support
        _uiSkillLoader.InitiateCombatMoves(lastAction); // Initialize the combat moves for the support action
        ShowSkillSelect(); // Show the skill selection UI
    }

    // Show the skill selection UI
    private void ShowSkillSelect()
    {
        _combatSystem.State = CombatState.PLAYER_SKILL_SELECT; // Set the combat state to player skill selection
    }

    // Set the primary button based on the last selected action
    public void SetPrimaryButton()
    {
        if (lastAction == CombatAction.DEFEND)
        {
            defendButton.Select(); // Select the defend button
        }
        else if (lastAction == CombatAction.SUPPORT)
        {
            supportButton.Select(); // Select the support button
        }
        else
        {
            attackButton.Select(); // Select the attack button
        }
    }
}