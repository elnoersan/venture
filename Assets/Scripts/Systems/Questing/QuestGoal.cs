using System;
using UnityEngine;

// A serializable class that represents a quest goal in the game
[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest Goal")]
public class QuestGoal : ScriptableObject
{
    [SerializeField] protected string description; // The description of the quest goal
    [SerializeField] private int currentAmount; // The current progress towards completing the goal
    [SerializeField] protected int requiredAmount; // The amount required to complete the goal
    [SerializeField] private bool completed; // Whether the goal has been completed
    [SerializeField] private QuestGoalType questGoalType; // The type of quest goal (e.g., Combat, Collection)
    private Quest attachedQuest; // The quest to which this goal is attached

    // Initializes the quest goal
    public virtual void Init(Quest quest)
    {
        completed = false; // Reset the completion status
        currentAmount = 0; // Reset the current progress

        attachedQuest = quest; // Set the attached quest

        // Subscribe to the appropriate event based on the quest goal type
        if (questGoalType == QuestGoalType.Combat)
        {
            GameEvents.Instance.onCombatVictory += OnCombatVictory;
            Debug.Log("Subscribed to CombatVictory");
        }
    }

    // Callback method for when a combat victory occurs
    private void OnCombatVictory()
    {
        Debug.Log("Quest Goal func callback");
        // Increment the current amount and clamp it to the required amount
        currentAmount = Mathf.Clamp(currentAmount++, currentAmount, requiredAmount);
    }

    // Checks if the quest goal is complete
    public bool IsComplete()
    {
        return currentAmount >= requiredAmount; // Returns true if the current amount meets or exceeds the required amount
    }

    // Property to get or set the description of the quest goal
    public string Description
    {
        get => description;
        set => description = value;
    }

    // Property to get or set the current amount of progress towards the goal
    public int CurrentAmount
    {
        get => currentAmount;
        set => currentAmount = value;
    }

    // Property to get or set the required amount to complete the goal
    public int RequiredAmount
    {
        get => requiredAmount;
        set => requiredAmount = value;
    }
}