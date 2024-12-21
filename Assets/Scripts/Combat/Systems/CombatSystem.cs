using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the core combat system, including state management, player and enemy turns, and skill execution.
/// This class handles the flow of combat, spawning units, and updating the UI and combat log.
/// </summary>
public class CombatSystem : MonoBehaviour
{
    // Variables
    private CombatState state; // The current state of the combat system.
    private CombatMove chosenSkill; // The skill chosen by the player.
    private int spawnedEnemies; // The number of enemies spawned in the current combat.

    // Systems
    private UICombatLog combatLog; // The UI component for displaying combat logs.
    private UIPlayerInputController uiInputController; // The UI controller for player input.
    private CombatLoader combatLoader; // The loader responsible for spawning units.
    private TurnManager turnManager; // The manager responsible for handling turn order.
    private EnemyController enemyController; // The controller for enemy AI.
    private SkillManager skillManager; // The manager for handling skills and cooldowns.
    private SkillExecutor skillExecutor; // The executor for applying skills and effects.

    // Player
    [Header("Player")]
    [SerializeField] private Transform playerStation; // The transform where the player is spawned.
    [SerializeField] private int maxPlayerActions = 2; // The maximum number of actions the player can take per turn.
    [SerializeField] private float playerCombatAnimationSpeed = 1.5f; // The speed of player combat animations.
    private CombatUnit player; // The player's CombatUnit.
    private GameObject playerGO; // The GameObject representing the player.
    private int remainingPlayerActions = 0; // The remaining number of actions the player can take.

    // Enemies
    [Header("Enemies")]
    [SerializeField] private Transform topEnemyStation; // The transform for the top enemy position.
    [SerializeField] private Transform centerEnemyStation; // The transform for the center enemy position.
    [SerializeField] private Transform bottomEnemyStation; // The transform for the bottom enemy position.
    [SerializeField] private Transform frontTopEnemyStation; // The transform for the front top enemy position.
    [SerializeField] private Transform frontBottomEnemyStation; // The transform for the front bottom enemy position.
    private CombatUnit topEnemy; // The CombatUnit for the top enemy.
    private CombatUnit centerEnemy; // The CombatUnit for the center enemy.
    private CombatUnit bottomEnemy; // The CombatUnit for the bottom enemy.
    private CombatUnit frontTopEnemy; // The CombatUnit for the front top enemy.
    private CombatUnit frontBottomEnemy; // The CombatUnit for the front bottom enemy.
    private List<GameObject> enemyGameObjects; // A list of GameObjects representing the enemies.

    /// <summary>
    /// Initializes the combat system by finding necessary components and starting the combat setup.
    /// </summary>
    void Awake()
    {
        combatLog = FindObjectOfType<UICombatLog>();
        combatLoader = FindObjectOfType<CombatLoader>();
        uiInputController = FindObjectOfType<UIPlayerInputController>();
        enemyController = FindObjectOfType<EnemyController>();
        skillManager = FindObjectOfType<SkillManager>();
        skillExecutor = FindObjectOfType<SkillExecutor>();
        enemyGameObjects = new List<GameObject>();
        StartCoroutine(SetupCombat());
    }

    /*
     * Sets up the combat scene by spawning players, enemies, and initializing systems.
     * Then calls SetNextState to begin combat.
     */
    IEnumerator SetupCombat()
    {
        state = CombatState.START;

        SetupPlayer();
        SetupEnemies();
        SetupTurnManager();

        combatLog.Clear();
        combatLog.StartOfCombat();

        yield return new WaitForSecondsRealtime(1);

        SetNextState();
    }

    /// <summary>
    /// Sets up the turn manager with the active units (player and enemies).
    /// </summary>
    private void SetupTurnManager()
    {
        List<GameObject> activeEnemies = GetActiveEnemies();
        activeEnemies.Add(playerGO);
        turnManager = new TurnManager(activeEnemies);
    }

    /// <summary>
    /// Spawns the player in the combat scene and initializes the player's CombatUnit.
    /// </summary>
    void SetupPlayer()
    {
        playerGO = combatLoader.SpawnPlayer(playerStation);
        player = playerGO.GetComponent<CombatUnit>();
    }

    /// <summary>
    /// Spawns enemies in the combat scene based on predefined patterns and the number of enemies to spawn.
    /// </summary>
    void SetupEnemies()
    {
        // Get amount of enemies from GameManager reference here. Currently hardcoded.

        /*
         * This code spawns enemies in predefined patterns, based on the amount of enemies.
         * GameObjects are added to the list for UI selections.
         * Enemy scripts are added to variables for easier reference for calculations.
         */
        GameObject topEnemyGO;
        GameObject centerEnemyGO;
        GameObject bottomEnemyGO;
        GameObject frontTopEnemyGO;
        GameObject frontBottomEnemyGO;

        CombatEncounterManager combatEncounterManager = FindObjectOfType<CombatEncounterManager>();
        int amountToSpawn = combatEncounterManager.AmountOfEnemiesToSpawn;
        spawnedEnemies = amountToSpawn;
        int levelOfEnemies = combatEncounterManager.EnemyLvl;

        switch (amountToSpawn)
        {
            case 1:
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);

                topEnemyStation.gameObject.SetActive(false);
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
                bottomEnemyStation.gameObject.SetActive(false);
                frontTopEnemyStation.gameObject.SetActive(false);
                frontBottomEnemyStation.gameObject.SetActive(false);

                break;
            case 2:
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);

                topEnemyStation.gameObject.SetActive(false);
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemyStation.gameObject.SetActive(false);
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();

                break;
            case 3:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);

                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemyStation.gameObject.SetActive(false);
                frontBottomEnemyStation.gameObject.SetActive(false);

                break;
            case 4:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);

                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();

                break;
            case 5:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);

                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();

                break;
        }
    }

    /*
     * Updates the CombatState depending on the state of the scene.
     * This is the core of state management for combat.
     */
    private void SetNextState()
    {
        if (player.isActiveAndEnabled)
        {
            if (GetActiveEnemies().Count > 0)
            {
                if (remainingPlayerActions > 0)
                {
                    NextPlayerAction();
                    return;
                }

                CombatUnit nextToAct = turnManager.GetNextTurn();

                if (nextToAct.UnitType == UnitType.PLAYER)
                {
                    NewPlayerTurn();
                }
                else if (nextToAct.UnitType == UnitType.ENEMY)
                {
                    NewEnemyTurn(nextToAct);
                }
            }
            else
            {
                state = CombatState.VICTORY;
                StartCoroutine(VictorySequence());
            }
        }
        else
        {
            state = CombatState.DEFEAT;
        }

        Debug.Log("Current state: " + state);
    }

    /*
     * Checks and returns all enemies and their stations.
     * Disable all inactive stations.
     */
    public List<GameObject> GetActiveEnemies()
    {
        List<CombatUnit> enemyReferences = GetAllEnemyReferences();
        List<Transform> enemyStations = GetAllEnemyStations();
        List<GameObject> activeEnemies = new List<GameObject>();

        for (int i = 0; i < enemyReferences.Count; i++)
        {
            if (shouldAddToList(enemyReferences[i]))
            {
                activeEnemies.Add(enemyReferences[i].gameObject);
            }
            else
            {
                enemyStations[i].gameObject.SetActive(false);
            }
        }

        return activeEnemies;
    }

    /// <summary>
    /// Returns a list of all enemy stations.
    /// </summary>
    /// <returns>A list of enemy station transforms.</returns>
    private List<Transform> GetAllEnemyStations()
    {
        return new List<Transform>
        {
            topEnemyStation,
            frontTopEnemyStation,
            centerEnemyStation,
            frontBottomEnemyStation,
            bottomEnemyStation
        };
    }

    /// <summary>
    /// Returns a list of active adjacent enemies to the target.
    /// </summary>
    /// <param name="target">The target CombatUnit.</param>
    /// <returns>A list of active adjacent CombatUnits.</returns>
    public List<CombatUnit> GetTargetAndActiveAdjacentPosition(CombatUnit target)
    {
        List<CombatUnit> allEnemyReferences = GetAllEnemyReferences();
        var indexOfTarget = allEnemyReferences.FindIndex(position => position == (target));
        List<CombatUnit> targetAndActiveAdjacentTargets = new List<CombatUnit> { target };

        if (indexOfTarget < 3)
        {
            if (shouldAddToList(allEnemyReferences[indexOfTarget + 1]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget + 1]);
            }
            else if (shouldAddToList(allEnemyReferences[indexOfTarget + 2]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget + 2]);
            }
        }
        else
        {
            if (shouldAddToList(allEnemyReferences[indexOfTarget - 1]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget - 1]);
            }
            else if (shouldAddToList(allEnemyReferences[indexOfTarget - 2]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget - 2]);
            }
        }

        return targetAndActiveAdjacentTargets;
    }

    /*
     * Resets respective game elements for the next player action.
     * Called just before the player gets the action by SetNextState.
     */
    void NextPlayerAction()
    {
        combatLog.NextPlayerAction(remainingPlayerActions);
        uiInputController.MoveCursorToDefaultActionSelect();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }

    /*
     * Resets respective game elements for the player's turn.
     * Called just before the player's turn by SetNextState.
     */
    void NewPlayerTurn()
    {
        player.GetComponent<CombatEffectManager>().ProcessActiveEffects(true);
        remainingPlayerActions = maxPlayerActions;
        skillManager.DecreaseCooldowns();
        combatLog.PlayerTurn();
        uiInputController.ResetActionSelectUI();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }

    /*
     * Resets respective game elements for the enemy's turn.
     * Called just before the enemy's turn by SetNextState.
     */
    void NewEnemyTurn(CombatUnit enemy)
    {
        enemy.GetComponent<CombatEffectManager>().ProcessActiveEffects(true);
        state = CombatState.ENEMY_TURN;
        StartCoroutine(ProcessEnemyTurn(enemy));
    }

    /*
     * When the player selects a skill.
     * Called by InputController.
     */
    public void OnSkillSelect(CombatMove move)
    {
        if (state != CombatState.PLAYER_SKILL_SELECT) return;

        chosenSkill = move;

        // If the move needs targets, go to target select; otherwise, execute the move.
        if (DoesMoveNeedTargets(chosenSkill))
        {
            state = CombatState.PLAYER_TARGET_SELECT;
        }
        else
        {
            UpdateRemainingActions();
            StartCoroutine(UsePlayerSkillWithoutTargetSelection(move));
        }
    }

    /// <summary>
    /// Checks if the selected move requires a target.
    /// </summary>
    /// <param name="move">The selected CombatMove.</param>
    /// <returns>True if the move requires a target; otherwise, false.</returns>
    private bool DoesMoveNeedTargets(CombatMove move)
    {
        if (move.GetTargets().Equals(CombatMoveTargets.Self) || move.GetTargets().Equals(CombatMoveTargets.Global))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /*
     * When the player selects a target (if the skill is not self or globally targeted).
     * Called by InputController.
     */
    public void OnTargetSelect(int targetIndex)
    {
        if (state != CombatState.PLAYER_TARGET_SELECT) return;

        UpdateRemainingActions();

        StartCoroutine(UsePlayerSkillWithTarget(chosenSkill, GetActiveEnemies()[targetIndex].GetComponent<CombatUnit>()));
    }

    /// <summary>
    /// Returns a list of all enemy references.
    /// </summary>
    /// <returns>A list of CombatUnit references for all enemy positions.</returns>
    private List<CombatUnit> GetAllEnemyReferences()
    {
        List<CombatUnit> allPositions = new List<CombatUnit>();

        allPositions.Add(topEnemy);
        allPositions.Add(frontTopEnemy);
        allPositions.Add(centerEnemy);
        allPositions.Add(frontBottomEnemy);
        allPositions.Add(bottomEnemy);
        return allPositions;
    }

    /// <summary>
    /// Checks if the given enemy should be added to the list of active enemies.
    /// </summary>
    /// <param name="enemy">The CombatUnit to check.</param>
    /// <returns>True if the enemy is active and enabled; otherwise, false.</returns>
    private bool shouldAddToList(CombatUnit enemy)
    {
        return enemy && enemy.isActiveAndEnabled;
    }

    /// <summary>
    /// Updates the remaining player actions and disables the chosen action in the UI.
    /// </summary>
    private void UpdateRemainingActions()
    {
        if (remainingPlayerActions > 0)
        {
            remainingPlayerActions--;
            uiInputController.DisableChosenAction(chosenSkill.GetActionType());
        }
    }

    /// <summary>
    /// Checks if a unit has died and removes it from the scene.
    /// </summary>
    /// <param name="result">The TakeDamageResult containing the unit's status.</param>
    public void CheckForDeath(TakeDamageResult result)
    {
        if (result.IsUnitDead)
        {
            Destroy(result.Unit.gameObject);
            if (result.Unit.UnitType == UnitType.PLAYER) playerStation.gameObject.SetActive(false);

            combatLog.PrintToLog(result.Unit.UnitName + " died!");

            uiInputController.UpdateTargetablePositions();
            turnManager.RemoveFromActiveUnits(result.Unit);
        }
    }

    /// <summary>
    /// Executes a player skill without target selection.
    /// </summary>
    /// <param name="move">The CombatMove to execute.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator UsePlayerSkillWithoutTargetSelection(CombatMove move)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        List<TakeDamageResult> results = skillExecutor.ExecuteMove(move, player, player, GetActiveEnemies());

        yield return new WaitForSeconds(playerCombatAnimationSpeed); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'

        results?.ForEach(CheckForDeath);

        player.CombatEffectsManager.ProcessActiveEffects(false);
        SetNextState();
    }

    /// <summary>
    /// Executes a player skill with target selection.
    /// </summary>
    /// <param name="move">The CombatMove to execute.</param>
    /// <param name="target">The target CombatUnit.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator UsePlayerSkillWithTarget(CombatMove move, CombatUnit target)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        List<TakeDamageResult> results = skillExecutor.ExecuteMove(move, player, target, GetActiveEnemies());

        yield return new WaitForSeconds(playerCombatAnimationSpeed); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'

        results?.ForEach(CheckForDeath);

        player.CombatEffectsManager.ProcessActiveEffects(false);
        SetNextState();
    }

    /*
     * Simple test implementation of enemy AI.
     * Will be delegated to EnemyController.
     */
    private IEnumerator ProcessEnemyTurn(CombatUnit enemy)
    {
        TakeDamageResult result = skillExecutor.ExecuteMoveOnTarget(skillManager.GetTestMove(), enemy, player);
        combatLog.PrintToLog("New player HP: " + player.CurrentHp);

        yield return new WaitForSeconds(1f);

        enemy.CombatEffectsManager.ProcessActiveEffects(false);
        CheckForDeath(result);
        SetNextState();
    }

    /// <summary>
    /// Handles the victory sequence after the player wins the combat.
    /// </summary>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    private IEnumerator VictorySequence()
    {
        combatLog.PlayerWon();

        FindObjectOfType<GameManager>().UpdatePlayerDataAfterCombat(new CombatResult(10, player.CurrentHp));
        FindObjectOfType<GameEvents>().CombatVictoryInvoke();

        yield return new WaitForSeconds(2f);

        FindObjectOfType<SceneTransition>().LoadScene(SceneIndexType.Exploration);
    }

    // Properties
    public CombatState State
    {
        get => state;
        set => state = value;
    }

    public int RemainingPlayerActions
    {
        get => remainingPlayerActions;
        set => remainingPlayerActions = value;
    }

    public CombatUnit Player
    {
        get => player;
    }

    public int SpawnedEnemies
    {
        get => spawnedEnemies;
        set => spawnedEnemies = value;
    }
}