using System.Collections.Generic;
using UnityEngine;

// A singleton class that manages quests in the game
public class QuestManager : Singleton<QuestManager>, IDataPersistence
{
    [SerializeField] private List<Quest> activeQuests; // List of currently active quests

    // Adds a quest to the active quests list
    public void AddQuest(Quest quest)
    {
        quest.Initialize(); // Initialize the quest and its goals
        activeQuests.Add(quest); // Add the quest to the active quests list
        UIQuestController.Instance.CreateOrUpdateSlot(quest); // Update the UI to reflect the new quest
    }

    // Removes a quest from the active quests list
    public void RemoveQuest(Quest quest)
    {
        activeQuests.Remove(quest); // Remove the quest from the active quests list
        UIQuestController.Instance.RemoveQuestFromUI(quest); // Update the UI to remove the quest
    }

    // Completes a quest and handles its reward
    public void CompleteQuest(Quest quest)
    {
        Debug.Log("Completing quest: " + quest.info.Name);

        // Handle other types of rewards here.
        if (quest.reward.type == RewardType.Experience)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.PlayerData.exp += quest.reward.amount; // Add experience points to the player
            gameManager.CheckForLevelUpAfterQuest(); // Check if the player levels up after gaining experience
        }

        RemoveQuest(quest); // Remove the quest from the active quests list
    }

    // Property to get or set the list of active quests
    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }

    // Loads quest data from the saved game data
    public void LoadData(GameData data)
    {
        data.QuestData.ActiveQuests.ForEach(quest =>
        {
            quest.Goals.ForEach(goal =>
            {
                goal.Init(quest); // Initialize each goal with a reference to the quest
                goal.CurrentAmount = data.QuestData.QuestGoalProgress[goal]; // Set the current progress for each goal
            });

            ActiveQuests.Add(quest); // Add the quest to the active quests list
        });
    }

    // Saves quest data to the game data
    public void SaveData(GameData data)
    {
        data.QuestData.ResetBeforeSave(); // Reset the quest data before saving

        ActiveQuests.ForEach(quest =>
        {
            quest.Goals.ForEach(goal =>
            {
                if (data.QuestData.QuestGoalProgress.ContainsKey(goal))
                {
                    data.QuestData.QuestGoalProgress[goal] = goal.CurrentAmount; // Update the progress for each goal
                }
                else
                {
                    data.QuestData.QuestGoalProgress.Add(goal, goal.CurrentAmount); // Add the progress for each goal
                }
            });

            if (!data.QuestData.ActiveQuests.Contains(quest)) data.QuestData.ActiveQuests.Add(quest); // Add the quest to the saved data
        });
    }
}