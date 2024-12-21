using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A serializable class that represents a quest in the game
[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest")]
public class Quest : ScriptableObject
{
    [Header("Info")]
    [SerializeField] public Info info; // Information about the quest
    [Header("Reward")]
    [SerializeField] public Reward reward; // The reward for completing the quest
    [SerializeField] private List<QuestGoal> goals; // The list of goals required to complete the quest
    [SerializeField] private string questId; // A unique identifier for the quest

    // Initializes the quest by initializing all its goals
    public void Initialize()
    {
        foreach (QuestGoal goal in goals)
        {
            goal.Init(this); // Initialize each goal with a reference to the quest
        }
    }

    // Checks if all goals in the quest are complete
    public bool IsQuestComplete()
    {
        return goals.All(goal => goal.IsComplete()); // Returns true if all goals are complete
    }

    // A struct that holds information about the quest
    [Serializable]
    public struct Info
    {
        [SerializeField] public string Name; // The name of the quest
        [SerializeField] public Sprite Icon; // The icon representing the quest
        [SerializeField] public string Description; // The description of the quest
    }

    // A struct that holds information about the reward for completing the quest
    [Serializable]
    public struct Reward
    {
        [SerializeField] public RewardType type; // The type of reward (e.g., gold, experience)
        [SerializeField] public int amount; // The amount of the reward
    }

    // Property to get or set the list of goals for the quest
    public List<QuestGoal> Goals
    {
        get => goals;
        set => goals = value;
    }
}