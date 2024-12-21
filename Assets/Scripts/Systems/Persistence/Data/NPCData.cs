using System;
using UnityEngine;

// A serializable class that represents NPC data for saving and loading
[Serializable]
public class NPCData : SaveData
{
    [SerializeField] public SerializableDictionary<string, bool> npcQuestTurnedInMap; // A dictionary mapping NPC quest IDs to whether they have been turned in

    // Constructor to initialize the NPC data
    public NPCData(SerializableDictionary<string, bool> npcQuestTurnedInMap)
    {
        this.npcQuestTurnedInMap = npcQuestTurnedInMap;
    }

    // Method to reset the NPC data before saving
    public void ResetBeforeSave()
    {
        npcQuestTurnedInMap = new SerializableDictionary<string, bool>(); // Clear the NPC quest turned-in map
    }
}