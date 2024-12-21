using System.Collections.Generic;
using UnityEngine;

public class UIQuestController : Singleton<UIQuestController>
{
    [SerializeField] private Transform questContainer; // The container for quest slots in the UI
    [SerializeField] private UIQuestSlot questSlotPrefab; // The prefab for individual quest slots

    private QuestManager questManager; // Reference to the QuestManager
    private Dictionary<Quest, UIQuestSlot> questMap = new Dictionary<Quest, UIQuestSlot>(); // Maps quests to their UI slots

    // Called when the object becomes enabled and active
    private void OnEnable()
    {
        questManager = FindObjectOfType<QuestManager>(); // Initialize the QuestManager
        InitQuestUI(); // Initialize the quest UI
    }

    // Initializes the quest UI by creating or updating slots for all active quests
    public void InitQuestUI()
    {
        // Iterate through all active quests and create or update their slots
        questManager.ActiveQuests.ForEach(quest =>
        {
            CreateOrUpdateSlot(quest);
        });
    }

    // Creates or updates a slot for a specific quest
    public void CreateOrUpdateSlot(Quest quest)
    {
        // If the quest is not already in the UI, create a new slot for it
        if (!questMap.ContainsKey(quest))
        {
            var slot = CreateSlot(quest);
            questMap.Add(quest, slot);
        }
        else
        {
            // If the quest is already in the UI, update its slot
            UpdateSlot(quest);
        }
    }

    // Creates a new slot for a quest
    private UIQuestSlot CreateSlot(Quest quest)
    {
        // Instantiate the slot prefab as a child of the quest container
        var slot = Instantiate(questSlotPrefab, questContainer);

        // Initialize the slot with the quest data
        slot.Init(quest);

        // Assign a callback to the slot's remove button to remove the quest
        slot.AssignRemoveCallback(() => QuestManager.Instance.RemoveQuest(quest));

        return slot;
    }

    // Updates the progress of an existing slot
    private void UpdateSlot(Quest quest)
    {
        questMap[quest].UpdateProgressOnUI(quest);
    }

    // Removes a quest from the UI
    public void RemoveQuestFromUI(Quest quest)
    {
        // Destroy the slot's GameObject and remove it from the map
        Destroy(questMap[quest].gameObject);
        questMap.Remove(quest);
    }
}