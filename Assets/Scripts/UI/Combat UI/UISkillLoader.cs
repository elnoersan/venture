using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

/*
 * The UISkillLoader class is responsible for loading and displaying skills in the UI.
 * It interacts with the SkillManager and CombatSystem to retrieve and display active combat moves.
 * The active skills are tracked by the SkillManager, and this class only handles UI updates.
 */
public class UISkillLoader : MonoBehaviour
{
    [SerializeField] private GameObject skillItemPrefab; // Prefab for the skill list item (logo, name, etc.)
    [SerializeField] private RectTransform contentRectTransform; // Transform of the scrollable content area

    private SkillManager skillManager; // Reference to the SkillManager
    private CombatSystem combatSystem; // Reference to the CombatSystem
    private List<CombatMove> combatMovesInUI; // List to store the combat moves currently displayed in the UI

    // Initialize references to SkillManager and CombatSystem, and create an empty list for combat moves
    private void Awake()
    {
        skillManager = FindObjectOfType<SkillManager>();
        combatSystem = FindObjectOfType<CombatSystem>();
        combatMovesInUI = new List<CombatMove>();
    }

    /*
     * Initializes the combat moves in the UI based on the chosen action.
     * Clears the existing UI and populates it with combat moves that match the chosen action type.
     * Returns the maximum index of the displayed combat moves.
     */
    public int InitiateCombatMoves(CombatAction chosenAction)
    {
        ClearSkillUI(); // Clear the existing UI
        combatMovesInUI.Clear(); // Clear the list of displayed combat moves

        // Iterate through the active combat moves and add those that match the chosen action type
        skillManager.GetActiveCombatMoves().ForEach(combatMove =>
        {
            if (combatMove.GetActionType().Equals(chosenAction))
            {
                AddMoveToUI(combatMove);
            }
        });

        return GetMaxIndex(); // Return the maximum index of the displayed combat moves
    }

    // Adds a combat move to the UI
    private void AddMoveToUI(CombatMove combatMove)
    {
        // Instantiate the skill item prefab as a child of the content rect transform
        var item = Instantiate(skillItemPrefab, contentRectTransform);

        // Set the skill icon and other UI elements
        item.GetComponentsInChildren<Image>()[0].sprite = combatMove.getIconImage();
        item.GetComponentsInChildren<Image>()[1].sprite = combatMove.GetIcon();
        item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMove.GetName());
        item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMove.GetPower().ToString());
        item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldown().ToString());

        // Add a tooltip trigger to the skill item
        UITooltipTrigger uiTooltipTrigger = item.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = combatMove.GetName();
        uiTooltipTrigger.subtitle = combatMove.GetType().ToString();
        uiTooltipTrigger.body = combatMove.GetDescription();

        // Format the duration text (display "-" if duration is 0)
        var formattedDuration = combatMove.GetDuration() > 0 ? combatMove.GetDuration().ToString() : "-";
        item.GetComponentsInChildren<TextMeshProUGUI>()[3].SetText(formattedDuration);

        // Set the scale of the skill item to ensure it displays correctly
        item.transform.localScale = Vector2.one;

        // Add the combat move to the list of displayed combat moves
        combatMovesInUI.Add(combatMove);

        // Check if the move is on cooldown or if the player is silenced
        if (combatMove.GetCooldownTracker().isMoveOnCooldown() || combatSystem.Player.CombatEffectsManager.IsEffectActive(CombatEffectType.Silence))
        {
            // Dim the skill icon and highlight the cooldown text in red
            item.GetComponentsInChildren<Image>()[0].color = Color.black;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.red;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldownTracker().GetRemainingCooldown().ToString() + "/" + combatMove.GetCooldown().ToString());
        }
        else
        {
            // Keep the skill icon and cooldown text normal
            item.GetComponentsInChildren<Image>()[0].color = Color.white;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.white;
        }
    }

    // Clears the skill UI by destroying all child objects of the content rect transform
    public void ClearSkillUI()
    {
        foreach (Transform child in contentRectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    // Returns the maximum index of the displayed combat moves (or 0 if no moves are displayed)
    public int GetMaxIndex()
    {
        return combatMovesInUI.Count > 0 ? combatMovesInUI.Count - 1 : 0;
    }

    // Returns the combat move at the specified index
    public CombatMove GetSkill(int index)
    {
        return combatMovesInUI[index];
    }
}