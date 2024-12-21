using UnityEngine;

// A singleton class that manages the overall state of the game
public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    [SerializeField] private PlayerData playerData; // Tracks global player-related data such as experience, quests, and items
    [SerializeField] private SettingsData settingsData; // Tracks settings such as volume, difficulty, etc.
    [SerializeField] private SkillData skillData; // Tracks all active player skills and cooldowns
    //private TalentData talentData; // Tracks and handles player talents (commented out)

    // References to other game systems
    private PlayerController playerControllerUnit;
    private LevelUpManager levelUpManager;
    private DialogueManager dialogueManager;

    // Exploration state of the game
    [SerializeField] private ExplorationState explorationState;

    // Initialize the GameManager
    private void Start()
    {
        playerControllerUnit = FindObjectOfType<PlayerController>(); // Find the player controller
        levelUpManager = FindObjectOfType<LevelUpManager>(); // Find the level-up manager
        dialogueManager = FindObjectOfType<DialogueManager>(); // Find the dialogue manager
        explorationState = ExplorationState.Explore; // Set the initial exploration state to Explore

        // Subscribe to game events for dialog state changes
        GameEvents.Instance.onShowDialog += () =>
        {
            explorationState = ExplorationState.Dialog; // Set the exploration state to Dialog when a dialog starts
        };
        GameEvents.Instance.onCloseDialog += () =>
        {
            if (explorationState == ExplorationState.Dialog) explorationState = ExplorationState.Explore; // Set the exploration state back to Explore when a dialog ends
        };
    }

    // Check if the player should level up after completing a quest
    public void CheckForLevelUpAfterQuest()
    {
        if (levelUpManager.PlayerShouldLevelUp(playerData.exp, playerData.nextLvLExp)) levelUpManager.LevelUp();
    }

    // Update player data after a combat encounter
    public void UpdatePlayerDataAfterCombat(CombatResult result)
    {
        playerData.currentHp = result.PlayerCurrentHp; // Update the player's current HP
        playerData.exp += result.XpGained; // Add experience points gained from combat
        if (levelUpManager.PlayerShouldLevelUp(playerData.exp, playerData.nextLvLExp)) levelUpManager.LevelUp(); // Check if the player should level up
    }

    // Save the player's position and facing direction before entering combat
    public void SavePositionBeforeCombat()
    {
        playerControllerUnit = FindObjectOfType<PlayerController>(); // Find the player controller

        playerData.position = playerControllerUnit.transform.position; // Save the player's position
        playerData.playerFacingDirection = playerControllerUnit.PlayerFacingDirection; // Save the player's facing direction
    }

    // Add a stat point to the player's stats
    public void AddStatPoint(StatType type)
    {
        switch (type)
        {
            case StatType.Strength:
                PlayerData.unitBase.Strength++; // Increase strength
                break;
            case StatType.Agility:
                PlayerData.unitBase.Agility++; // Increase agility
                break;
            case StatType.Intellect:
                PlayerData.unitBase.Intellect++; // Increase intellect
                break;
        }

        playerData.remainingStatPoints--; // Decrease the remaining stat points
    }

    // Property to get the player data
    public PlayerData PlayerData
    {
        get => playerData;
    }

    // Load game data from the saved data
    public void LoadData(GameData data)
    {
        playerData = data.PlayerData;
        settingsData = data.SettingsData;
        skillData = data.SkillData;
    }

    // Save game data to the saved data
    public void SaveData(GameData data)
    {
        data.PlayerData = playerData;
        data.SettingsData = settingsData;
        data.SkillData = skillData;
    }

    // Property to get or set the exploration state
    public ExplorationState ExplorationState
    {
        get => explorationState;
        set => explorationState = value;
    }
}