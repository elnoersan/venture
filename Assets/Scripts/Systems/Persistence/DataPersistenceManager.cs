using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Change filehandler to interface (facade)
 */
public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
{
    [Header("New Game Settings")]
    [SerializeField] private UnitBase playerUnitBase; // The base stats for the player unit in a new game

    [Header("Persistence configuration")]
    [SerializeField] private string fileName; // The name of the file where game data will be saved

    private List<IDataPersistence> persistenceObjects; // List of objects that implement IDataPersistence
    private GameData gameData; // The game data object that holds all save data
    private GameManager gameManager; // Reference to the GameManager
    private IFileHandler fileHandler; // The file handler for saving and loading data

    // Initialize the DataPersistenceManager
    private void Start()
    {
        this.persistenceObjects = FindAllDataPersistenceObjects(); // Find all objects that implement IDataPersistence
        fileHandler = new FileHandler(Application.persistentDataPath, fileName); // Initialize the file handler
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        LoadGame(); // Load the game data
    }

    // Create a new game with default data
    private void NewGame()
    {
        gameData = new GameData(
            new PlayerData(
                1, // Player level
                0, // Player experience
                10, // Experience required for the next level
                0, // Remaining stat points
                0, // Scene index
                new Vector3(-12.5f, 3.75f), // Player position
                PlayerFacing.South, // Player facing direction
                playerUnitBase, // Player unit base stats
                playerUnitBase.MaxHp // Player current HP (starts at max HP)
            ),
            new SettingsData(), // Default settings data
            new SkillData(), // Default skill data
            new LevelUpData(), // Default level-up data
            new InventoryData(new SerializableDictionary<InventoryItem, int>()), // Empty inventory data
            new EquipmentData(null, null, null, null, null, null, null), // No equipment data
            new CombatEncounterData(), // Default combat encounter data
            new NPCData(new SerializableDictionary<string, bool>()), // Empty NPC data
            new QuestData(
                new List<Quest>(), // Empty list of active quests
                new SerializableDictionary<QuestGoal, int>() // Empty quest goal progress
            )
        );

        // Test save (commented out)
        //gameData.playerData.chestsCollected.Add("chest1", true);

        gameManager.LoadData(gameData); // Load the new game data into the GameManager
    }

    // Load the game data from a file
    private void LoadGame()
    {
        gameData = fileHandler.Load(); // Load the game data from the file
        bool isNewGame = false;

        // Check if the MainMenuController is present and if it's a new game
        if (FindObjectOfType<MainMenuController>() != null)
        {
            isNewGame = FindObjectOfType<MainMenuController>().IsNewGame();
        }

        // If no data is found or it's a new game, initialize to default
        if (this.gameData == null || isNewGame)
        {
            Debug.Log("No data found. Initializing to default");
            NewGame();
        }
        else
        {
            // Load the game data into all persistence objects
            persistenceObjects.ForEach(obj => obj.LoadData(gameData));
            Debug.Log("Loaded data successfully.");
        }
    }

    // Save the game data to a file
    public void SaveGame()
    {
        // Save the game data from all persistence objects
        FindAllDataPersistenceObjects().ForEach(obj => obj.SaveData(gameData));
        fileHandler.Save(gameData); // Save the game data to the file
        Debug.Log("Game saved");
    }

    /*
     private void OnApplicationQuit()
    {
        SaveGame();
    } 
     */

    // Find all objects in the scene that implement IDataPersistence
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}