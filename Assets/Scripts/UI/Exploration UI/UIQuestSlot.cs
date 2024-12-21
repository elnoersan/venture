using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title; // Text component for displaying the quest title
    [SerializeField] private Transform questGoalContainer; // Container for quest goals
    [SerializeField] private GameObject questCompletedIndicator; // Indicator for completed quests
    [SerializeField] private Button abandonQuestButton; // Button for abandoning the quest
    [SerializeField] private GameObject questGoalPrefab; // Prefab for individual quest goals

    // Initializes the quest slot with the given quest data
    public void Init(Quest quest)
    {
        // Show or hide the quest completed indicator based on the quest's completion status
        questCompletedIndicator.SetActive(quest.IsQuestComplete());

        // Set the quest title
        title.text = quest.info.Name;

        // Initialize each quest goal in the quest
        quest.Goals.ForEach(goal => InitQuestGoal(goal));
    }

    // Assigns a callback to the abandon quest button
    public void AssignRemoveCallback(Action onClickCallback)
    {
        abandonQuestButton.onClick.AddListener(() => onClickCallback());
    }

    // Initializes a quest goal in the UI
    private void InitQuestGoal(QuestGoal goal)
    {
        // Instantiate the quest goal prefab as a child of the quest goal container
        var questGoal = Instantiate(questGoalPrefab, questGoalContainer);

        // Get the text components in the quest goal prefab
        var texts = questGoal.GetComponentsInChildren<TextMeshProUGUI>();

        // Set the goal description and progress text
        texts[1].text = goal.IsComplete() ? $"<s>{goal.Description}</s>" : goal.Description;
        texts[2].text = $"{goal.CurrentAmount}/{goal.RequiredAmount}";
    }

    // Updates the progress of the quest in the UI
    public void UpdateProgressOnUI(Quest quest)
    {
        // Show or hide the quest completed indicator based on the quest's completion status
        questCompletedIndicator.SetActive(quest.IsQuestComplete());

        // Update the progress text for each quest goal
        for (int i = 0; i < questGoalContainer.childCount; i++)
        {
            var texts = questGoalContainer.transform.GetChild(i)
                .GetComponentsInChildren<TextMeshProUGUI>();

            texts[2].text = $"{quest.Goals[i].CurrentAmount}/{quest.Goals[i].RequiredAmount}";
        }
    }
}