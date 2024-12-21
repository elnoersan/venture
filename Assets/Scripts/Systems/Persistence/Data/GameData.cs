using System;
using UnityEngine;

// A serializable class that represents the entire game's data for saving and loading
[Serializable]
public class GameData
{
    [SerializeField] public PlayerData playerData; // Data related to the player
    [SerializeField] private SettingsData settingsData; // Data related to game settings
    [SerializeField] private SkillData skillData; // Data related to skills
    [SerializeField] private LevelUpData levelUpData; // Data related to level-up mechanics
    [SerializeField] private InventoryData inventoryData; // Data related to the player's inventory
    [SerializeField] private EquipmentData equipmentData; // Data related to the player's equipment
    [SerializeField] private CombatEncounterData combatEncounterData; // Data related to combat encounters
    [SerializeField] private NPCData npcData; // Data related to NPCs
    [SerializeField] private QuestData questData; // Data related to quests

    // Constructor to initialize all game data
    public GameData(PlayerData playerData, SettingsData settingsData, SkillData skillData, LevelUpData levelUpData, InventoryData inventoryData, EquipmentData equipmentData, CombatEncounterData combatEncounterData, NPCData npcData, QuestData questData)
    {
        this.playerData = playerData;
        this.settingsData = settingsData;
        this.skillData = skillData;
        this.levelUpData = levelUpData;
        this.inventoryData = inventoryData;
        this.equipmentData = equipmentData;
        this.combatEncounterData = combatEncounterData;
        this.npcData = npcData;
        this.questData = questData;
    }

    // Property to get or set the player data
    public PlayerData PlayerData
    {
        get => playerData;
        set => playerData = value;
    }

    // Property to get or set the settings data
    public SettingsData SettingsData
    {
        get => settingsData;
        set => settingsData = value;
    }

    // Property to get or set the skill data
    public SkillData SkillData
    {
        get => skillData;
        set => skillData = value;
    }

    // Property to get or set the level-up data
    public LevelUpData LevelUpData
    {
        get => levelUpData;
        set => levelUpData = value;
    }

    // Property to get or set the inventory data
    public InventoryData InventoryData
    {
        get => inventoryData;
        set => inventoryData = value;
    }

    // Property to get or set the equipment data
    public EquipmentData EquipmentData
    {
        get => equipmentData;
        set => equipmentData = value;
    }

    // Property to get or set the combat encounter data
    public CombatEncounterData CombatEncounterData
    {
        get => combatEncounterData;
        set => combatEncounterData = value;
    }

    // Property to get or set the NPC data
    public NPCData NpcData
    {
        get => npcData;
        set => npcData = value;
    }

    // Property to get or set the quest data
    public QuestData QuestData
    {
        get => questData;
        set => questData = value;
    }
}