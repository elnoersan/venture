using UnityEngine;

/// <summary>
/// Manages combat encounters in the game, including encounter rates, enemy spawns, and enemy levels.
/// This class is a persistent singleton and implements the IDataPersistence interface for saving and loading data.
/// </summary>
public class CombatEncounterManager : PersistentSingleton<CombatEncounterManager>, IDataPersistence
{
    // The encounter rate for the current area, determining how often combat encounters occur.
    [SerializeField] int areaEncounterRate = 15;

    // The base number of enemies to spawn during a combat encounter.
    [SerializeField] private int amountOfEnemiesToSpawn = 1;

    // The level of the enemies to spawn during a combat encounter.
    [SerializeField] private int enemyLvl = 1;

    // Updating encounter stats logic here.

    /// <summary>
    /// Gets or sets the encounter rate for the current area.
    /// </summary>
    public int AreaEncounterRate
    {
        get => areaEncounterRate;
        set => areaEncounterRate = value;
    }

    /// <summary>
    /// Gets or sets the number of enemies to spawn during a combat encounter.
    /// The getter returns a random number of enemies within the specified range.
    /// </summary>
    public int AmountOfEnemiesToSpawn
    {
        get
        {
            var randomSpawn = Random.Range(1, amountOfEnemiesToSpawn);
            return randomSpawn;
        }
        set => amountOfEnemiesToSpawn = value;
    }

    /// <summary>
    /// Gets or sets the level of the enemies to spawn during a combat encounter.
    /// </summary>
    public int EnemyLvl
    {
        get => enemyLvl;
        set => enemyLvl = value;
    }

    /// <summary>
    /// Loads combat encounter data from the provided GameData object.
    /// </summary>
    /// <param name="data">The GameData object containing the combat encounter data to load.</param>
    public void LoadData(GameData data)
    {
        areaEncounterRate = data.CombatEncounterData.AreaEncounterRate;
        amountOfEnemiesToSpawn = data.CombatEncounterData.AmountOfEnemiesToSpawn;
        enemyLvl = data.CombatEncounterData.EnemyLvl;
    }

    /// <summary>
    /// Saves combat encounter data to the provided GameData object.
    /// </summary>
    /// <param name="data">The GameData object to save the combat encounter data to.</param>
    public void SaveData(GameData data)
    {
        data.CombatEncounterData.AreaEncounterRate = areaEncounterRate;
        data.CombatEncounterData.AmountOfEnemiesToSpawn = amountOfEnemiesToSpawn;
        data.CombatEncounterData.EnemyLvl = enemyLvl;
    }
}