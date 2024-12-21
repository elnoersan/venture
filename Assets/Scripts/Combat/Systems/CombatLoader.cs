using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the loading and spawning of combat-related elements, such as players, enemies, and UI elements.
/// This class is responsible for setting up the combat scene, including spawning units and configuring the UI.
/// </summary>
public class CombatLoader : MonoBehaviour
{
    // Positions for spawning
    [Header("Player")]
    [SerializeField] private UnitBase player; // The base data for the player unit.
    [SerializeField] private GameObject playerPrefab; // The prefab for the player unit.

    [Header("Enemies")]
    [SerializeField] private List<UnitBase> enemies; // A list of possible enemy unit bases.
    [SerializeField] private GameObject enemyPrefab; // The prefab for enemy units.

    [Header("Background")]
    [SerializeField] private GameObject backgroundGO; // The background GameObject.
    [SerializeField] private List<Sprite> backgroundSprites; // A list of possible background sprites.

    [Header("UI Elements")]
    [SerializeField] private GameObject statDisplayPrefab; // The prefab for the stat display UI.
    [SerializeField] private GameObject playerStatDisplayContainer; // The container for the player's stat display.
    [SerializeField] private GameObject statDisplayContainer; // The container for enemy stat displays.
    [SerializeField] private float basicOffset = 60; // The basic offset for UI elements.
    [SerializeField] private float widthPrStatDisplay = 110; // The width per stat display.

    private GameManager gameManager; // Reference to the GameManager.

    /// <summary>
    /// Initializes the CombatLoader by setting up the background and finding the GameManager.
    /// </summary>
    private void Awake()
    {
        SetupBackground();
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Resizes the stat display container based on the number of spawned enemies.
    /// </summary>
    public void ResizeStatDisplayContainer()
    {
        int amountOfDisplays = FindObjectOfType<CombatSystem>().SpawnedEnemies;
        float width = 500 - basicOffset - (widthPrStatDisplay * (amountOfDisplays - 1));
        statDisplayContainer.GetComponent<RectTransform>().offsetMax = new Vector2(-width, 0);
    }

    /// <summary>
    /// Adds a stat display for the player unit.
    /// </summary>
    /// <param name="unit">The player's CombatUnit.</param>
    public void AddStatDisplayForPlayer(CombatUnit unit)
    {
        Instantiate(statDisplayPrefab, playerStatDisplayContainer.transform)
            .GetComponent<UIStatDisplay>().ConnectedUnit = unit;
        ResizeStatDisplayContainer();
    }

    /// <summary>
    /// Adds a stat display for an enemy unit.
    /// </summary>
    /// <param name="unit">The enemy's CombatUnit.</param>
    public void AddStatDisplayForEnemyUnit(CombatUnit unit)
    {
        Instantiate(statDisplayPrefab, statDisplayContainer.transform)
            .GetComponent<UIStatDisplay>().ConnectedUnit = unit;
        ResizeStatDisplayContainer();
    }

    /// <summary>
    /// Spawns the player unit in the combat scene.
    /// </summary>
    /// <param name="playerStation">The transform where the player should be spawned.</param>
    /// <returns>The spawned player GameObject.</returns>
    public GameObject SpawnPlayer(Transform playerStation)
    {
        UnitBase playerBase = gameManager.PlayerData.unitBase;
        var spawnCombatUnit = SpawnCombatUnit(playerBase, playerPrefab, playerStation, 1);
        AddStatDisplayForPlayer(spawnCombatUnit.GetComponent<CombatUnit>());
        spawnCombatUnit.GetComponent<CombatUnit>().CurrentHp = gameManager.PlayerData.currentHp;
        return spawnCombatUnit;
    }

    /// <summary>
    /// Spawns an enemy unit in the combat scene.
    /// </summary>
    /// <param name="station">The transform where the enemy should be spawned.</param>
    /// <param name="level">The level of the enemy to spawn.</param>
    /// <returns>The spawned enemy GameObject.</returns>
    public GameObject SpawnEnemy(Transform station, int level)
    {
        UnitBase randomEnemyBase = GetRandomEnemyBase();

        var spawnCombatUnit = SpawnCombatUnit(randomEnemyBase, enemyPrefab, station, level);
        spawnCombatUnit.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(randomEnemyBase.SpriteLocalScale, randomEnemyBase.SpriteLocalScale);
        AddStatDisplayForEnemyUnit(spawnCombatUnit.GetComponent<CombatUnit>());

        return spawnCombatUnit;
    }

    /// <summary>
    /// Spawns a combat unit (player or enemy) in the combat scene.
    /// </summary>
    /// <param name="unitBase">The base data for the unit to spawn.</param>
    /// <param name="unitPrefab">The prefab for the unit.</param>
    /// <param name="station">The transform where the unit should be spawned.</param>
    /// <param name="level">The level of the unit to spawn.</param>
    /// <returns>The spawned unit GameObject.</returns>
    public GameObject SpawnCombatUnit(UnitBase unitBase, GameObject unitPrefab, Transform station, int level)
    {
        GameObject spawnedUnit = Instantiate(unitPrefab, station);

        float offsetAdjustedY = station.position.y - unitBase.SpriteVerticalOffset;
        Debug.Log("Adjusted Y result: " + station.position.y + " - " + unitBase.SpriteVerticalOffset + " = " + offsetAdjustedY);
        Vector3 offsetAdjustedPosition = new Vector3(station.position.x, offsetAdjustedY);
        spawnedUnit.transform.position = offsetAdjustedPosition;

        spawnedUnit.GetComponentInChildren<SpriteRenderer>().sprite = unitBase.IdleSprite;
        spawnedUnit.GetComponent<CombatUnit>().InitiateUnit(unitBase, level);

        return spawnedUnit;
    }

    /// <summary>
    /// Selects a random enemy base from the list of possible enemies.
    /// </summary>
    /// <returns>A random UnitBase representing an enemy.</returns>
    private UnitBase GetRandomEnemyBase()
    {
        int randomEnemyIndex = Random.Range(0, enemies.Count);
        return enemies[randomEnemyIndex];
    }

    /// <summary>
    /// Sets up the background for the combat scene by selecting a random background sprite.
    /// </summary>
    void SetupBackground()
    {
        int randomIndex = Random.Range(0, backgroundSprites.Count);
        backgroundGO.GetComponentsInChildren<SpriteRenderer>()[0].sprite = backgroundSprites[randomIndex];
    }
}