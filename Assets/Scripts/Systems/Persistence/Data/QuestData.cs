using System;
using System.Collections.Generic;
using UnityEngine;

// A serializable class that represents quest data for saving and loading
[Serializable]
public class QuestData : SaveData
{
    [SerializeField] private List<Quest> activeQuests; // A list of currently active quests
    [SerializeField] private SerializableDictionary<QuestGoal, int> questGoalProgress; // A dictionary mapping quest goals to their progress

    // Constructor to initialize the quest data
    public QuestData(List<Quest> activeQuests, SerializableDictionary<QuestGoal, int> questGoalProgress)
    {
        this.activeQuests = activeQuests;
        this.questGoalProgress = questGoalProgress;
    }

    // Property to get or set the list of active quests
    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }

    // Property to get or set the quest goal progress dictionary
    public SerializableDictionary<QuestGoal, int> QuestGoalProgress
    {
        get => questGoalProgress;
        set => questGoalProgress = value;
    }

    // Method to reset the quest data before saving
    public void ResetBeforeSave()
    {
        activeQuests = new List<Quest>(); // Clear the list of active quests
        questGoalProgress = new SerializableDictionary<QuestGoal, int>(); // Clear the quest goal progress dictionary
    }
}