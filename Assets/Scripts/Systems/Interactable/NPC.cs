using System;
using UnityEngine;

/// <summary>
/// Represents a Non-Player Character (NPC) in the game that can be interacted with.
/// This class implements the Interactable and IDataPersistence interfaces to define interaction behavior and save/load functionality.
/// </summary>
[Serializable]
public class NPC : MonoBehaviour, Interactable, IDataPersistence
{
    [SerializeField] private Dialogue dialogue; // The dialogue associated with the NPC.
    [SerializeField] private Quest quest; // The quest associated with the NPC.
    [SerializeField] private bool questHasBeenTurnedIn; // Whether the quest has been turned in by the player.
    [SerializeField] private string npcId; // The unique identifier for the NPC.
    private QuestManager questManager; // The QuestManager instance for handling quests.

    /// <summary>
    /// Defines the interaction behavior when the player interacts with the NPC.
    /// </summary>
    public void Interact()
    {
        Debug.Log("Interacting with NPC");

        if (questHasBeenTurnedIn)
        {
            // Play post-turning in quest dialogue here.
            Debug.Log("This quest has already been turned in");
            return;
        }

        if (questManager == null) questManager = FindObjectOfType<QuestManager>();
        bool isQuestActive = questManager.ActiveQuests.Contains(quest);
        bool isQuestCompleted = quest.IsQuestComplete();

        if (!isQuestActive)
        {
            // Quest intro dialogue here
            FindObjectOfType<DialogueManager>().ShowDialog(dialogue);
            FindObjectOfType<QuestManager>().AddQuest(quest);
        }
        else if (isQuestActive && isQuestCompleted)
        {
            // Turn in the quest and get rewards:
            // Quest completion dialogue here
            Debug.Log("Completing quest");
            questManager.CompleteQuest(quest);
            questHasBeenTurnedIn = true;
        }
    }

    /// <summary>
    /// Loads the NPC's data from the provided GameData object.
    /// </summary>
    /// <param name="data">The GameData object containing the NPC's data.</param>
    public void LoadData(GameData data)
    {
        if (data.NpcData.npcQuestTurnedInMap.ContainsKey(npcId))
            this.questHasBeenTurnedIn = data.NpcData.npcQuestTurnedInMap[npcId];
    }

    /// <summary>
    /// Saves the NPC's data to the provided GameData object.
    /// </summary>
    /// <param name="data">The GameData object to save the NPC's data to.</param>
    public void SaveData(GameData data)
    {
        data.NpcData.ResetBeforeSave();
        data.NpcData.npcQuestTurnedInMap.Add(npcId, this.questHasBeenTurnedIn);
    }
}