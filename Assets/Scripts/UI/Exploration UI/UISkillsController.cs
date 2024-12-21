using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillsController : MonoBehaviour
{
    // Load skills into the UI from the SkillManager
    [SerializeField] private GameObject skillItemPrefab; // Prefab for individual skill items
    [SerializeField] private RectTransform contentRectTransform; // Container for skill items in the UI

    private SkillManager skillManager; // Reference to the SkillManager

    // Initialize the SkillManager and set up the UI
    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();

        InitUI();
    }

    // Initializes the UI by adding all active combat moves to the skill list
    private void InitUI()
    {
        // Iterate through all active combat moves and add them to the UI
        skillManager.GetActiveCombatMoves().ForEach(AddSkillToUI);
    }

    // Adds a combat move to the UI
    private void AddSkillToUI(CombatMove combatMove)
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
    }
}