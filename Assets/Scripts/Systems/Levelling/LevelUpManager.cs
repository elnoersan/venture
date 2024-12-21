using UnityEngine;

public class LevelUpManager : PersistentSingleton<LevelUpManager>, IDataPersistence
{
    private GameManager gameManager; // Reference to the GameManager

    [SerializeField] private int statPointsPrLevel = 3; // Number of stat points gained per level
    [SerializeField] private int capBaseGrowth = 15; // Base growth value for the next level's experience requirement
    [SerializeField] private int capGrowthRate = 10; // Growth rate for increasing the experience requirement
    [SerializeField] private float capMultiplier = 1.1f; // Multiplier for the growth rate

    // Initialize the GameManager reference
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Check if the player should level up based on current experience and the next level's experience requirement
    public bool PlayerShouldLevelUp(int currentExp, int nextLvlExp)
    {
        Debug.Log("XP: " + currentExp + ", " + nextLvlExp);
        return currentExp >= nextLvlExp; // Return true if the player has enough experience to level up
    }

    // Perform the level-up process
    public void LevelUp()
    {
        Debug.Log("levelling up");

        // Increase the player's remaining stat points
        gameManager.PlayerData.remainingStatPoints += statPointsPrLevel;

        // Increase the experience requirement for the next level
        gameManager.PlayerData.nextLvLExp += capBaseGrowth;

        // Increase the player's level
        gameManager.PlayerData.level++;

        // Update the growth rate and base growth for the next level
        capGrowthRate = Mathf.RoundToInt(capGrowthRate * capMultiplier);
        capBaseGrowth += capGrowthRate;
    }

    // Load level-up data from saved game data
    public void LoadData(GameData data)
    {
        statPointsPrLevel = data.LevelUpData.StatPointsPrLevel;
        capBaseGrowth = data.LevelUpData.CapBaseGrowth;
        capGrowthRate = data.LevelUpData.CapGrowthRate;
        capMultiplier = data.LevelUpData.CapMultiplier;
    }

    // Save level-up data to game data
    public void SaveData(GameData data)
    {
        data.LevelUpData.StatPointsPrLevel = statPointsPrLevel;
        data.LevelUpData.CapBaseGrowth = capBaseGrowth;
        data.LevelUpData.CapGrowthRate = capGrowthRate;
        data.LevelUpData.CapMultiplier = capMultiplier;
    }
}