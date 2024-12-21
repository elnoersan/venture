using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

/// <summary>
/// Controls the player's input for UI navigation during combat, including action, skill, and target selection.
/// </summary>
public class UIPlayerInputController : MonoBehaviour
{
    private CombatSystem combatSystem;
    private UICombatLog combatLog;

    // TODO UI highlighting should be refactored to Ui Animation Controller
    [Header("Action Select")]
    [SerializeField] private GameObject attackAction;
    [SerializeField] private GameObject defendAction;
    [SerializeField] private GameObject supportAction;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject defendPanel;
    [SerializeField] private GameObject supportPanel;
    [SerializeField] private Sprite defaultAttackBG;
    [SerializeField] private Sprite selectedAttackBG;
    [SerializeField] private Sprite defaultDefendBG;
    [SerializeField] private Sprite selectedDefendBG;
    [SerializeField] private Sprite defaultSupportBG;
    [SerializeField] private Sprite selectedSupportBG;
    [SerializeField] private float xActionCursorOffset;
    [SerializeField] private float yActionCursorOffset;
    [SerializeField] private float selectedActionScale = 1;

    [Header("Skill Select")]
    [SerializeField] private float selectorMoveDistance;
    [SerializeField] private RectTransform selector;
    [SerializeField] private float scrollOffset;
    [SerializeField] private RectTransform contentRectTransform;
    private Vector2 defaultRectTransformOffset;

    [Header("Target Select")]
    [SerializeField] private Transform cursor;
    [SerializeField] private Sprite cursorIdle;
    [SerializeField] private Sprite cursorOnSelect;
    [SerializeField] private float xCursorOffset;
    [SerializeField] private float yCursorOffset;
    [SerializeField] private float pointerClickAnimationTime = 1;
    private bool playerHasChosenATarget;

    // General selecting
    private Vector2 inputDirection;

    // Action selecting
    private Vector2 defaultActionCursorPosition;
    private List<GameObject> actionButtonsGameObjects; // used for targetting etc
    private List<GameObject> activeActionButtons; // used to disable action in SelectAction()
    private List<Vector2> selectableActionPositions;
    private CombatAction activeAction;
    private Vector2 lastActionPosition;
    private int actionIndex;
    private int maxActionIndex = 2;

    // Skill selecting
    private UISkillLoader skillLoader;
    private Vector2 defaultSkillSelectorPosition;
    private Vector2 lastSkillSelectorPosition;
    private int selectorPosition;
    private int selectorScrollCap = 3;
    private int skillIndex;
    private int maxSkillIndex;

    // Target selecting
    private Vector2 defaultTargetCursorPosition;
    private int targetIndex;
    private int maxTargetIndex;
    private List<Vector2> selectableTargetPositions;

    /// <summary>
    /// Initializes references to other components and sets up the action select lists.
    /// </summary>
    void Awake()
    {
        skillLoader = FindObjectOfType<UISkillLoader>();
        combatLog = FindObjectOfType<UICombatLog>();

        InitiateActionSelectLists();

        selectableTargetPositions = new List<Vector2>();
    }

    /// <summary>
    /// Initializes the combat system and sets up the initial positions for selection.
    /// </summary>
    void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();

        SetActiveSelectPositions();
        InitiateSkillSelect();
        UpdateTargetablePositions();

        cursor.position = defaultActionCursorPosition;
        defaultSkillSelectorPosition = selector.position;
        lastSkillSelectorPosition = defaultSkillSelectorPosition;
    }

    /// <summary>
    /// Initializes the lists for action selection.
    /// </summary>
    private void InitiateActionSelectLists()
    {
        actionButtonsGameObjects = new List<GameObject>();
        selectableActionPositions = new List<Vector2>();
        activeActionButtons = new List<GameObject>();

        // Load default action buttons
        actionButtonsGameObjects.Add(attackAction);
        actionButtonsGameObjects.Add(defendAction);
        actionButtonsGameObjects.Add(supportAction);

        // Load default into active
        actionButtonsGameObjects.ForEach(buttonGO => activeActionButtons.Add(buttonGO));
    }

    /// <summary>
    /// Sets the active positions for choosing an action.
    /// </summary>
    private void SetActiveSelectPositions()
    {
        selectableActionPositions.Clear();

        foreach (GameObject gameObject in actionButtonsGameObjects)
        {
            Vector2 modifiedTransform = gameObject.transform.position;
            modifiedTransform.x += xActionCursorOffset;
            modifiedTransform.y += yActionCursorOffset;
            selectableActionPositions.Add(modifiedTransform);
        }

        defaultActionCursorPosition = selectableActionPositions[0];
        actionIndex = 0;
        maxActionIndex = selectableActionPositions.Count - 1;
    }

    /// <summary>
    /// Resets the action select UI to its default state.
    /// </summary>
    public void ResetActionSelectUI()
    {
        activeActionButtons.Clear();
        actionButtonsGameObjects.ForEach(buttonGO =>
        {
            activeActionButtons.Add(buttonGO);
            buttonGO.GetComponent<Image>().color = Color.white;
        });

        ResetActionSelectPanels();
        MoveCursorToDefaultActionSelect();
    }

    /// <summary>
    /// Disables the chosen action by greying out the corresponding button.
    /// </summary>
    /// <param name="action">The action to disable.</param>
    public void DisableChosenAction(CombatAction action)
    {
        Debug.Log("Combat action chosen: " + action);
        GameObject actionButtonGO = activeActionButtons.Find(buttonGO =>
            buttonGO.name
                .ToLower()
                .Contains(
                    action.ToString().ToLower()
                )
        );

        activeActionButtons.Remove(actionButtonGO);
        actionButtonGO.gameObject.GetComponent<Image>().color = Color.black;
    }

    /// <summary>
    /// Checks if the specified action is disabled.
    /// </summary>
    /// <param name="action">The action to check.</param>
    /// <returns>True if the action is disabled, false otherwise.</returns>
    public bool IsActionDisabled(CombatAction action)
    {
        return !activeActionButtons.Find(buttonGO =>
            buttonGO.name
                .ToLower()
                .Contains(
                    action.ToString().ToLower()
                )
        );
    }

    /// <summary>
    /// Initializes the skill selection UI.
    /// </summary>
    private void InitiateSkillSelect()
    {
        defaultSkillSelectorPosition = selector.position;
        defaultRectTransformOffset = contentRectTransform.offsetMax;
        skillIndex = 0;
    }

    /// <summary>
    /// Updates the positions of the targetable enemies.
    /// </summary>
    public void UpdateTargetablePositions()
    {
        selectableTargetPositions.Clear();

        List<GameObject> activeEnemies = combatSystem.GetActiveEnemies();
        if (activeEnemies.Count <= 0) return;

        activeEnemies.ForEach(enemyObject =>
        {
            Vector2 modifiedTransform = enemyObject.transform.position;
            modifiedTransform.x += xCursorOffset;
            modifiedTransform.y += yCursorOffset;
            selectableTargetPositions.Add(modifiedTransform);
        });

        defaultTargetCursorPosition = selectableTargetPositions[0];
        targetIndex = 0;
        maxTargetIndex = selectableTargetPositions.Count - 1;

    }

    /// <summary>
    /// Handles the movement input from the user and moves the appropriate UI element.
    /// </summary>
    /// <param name="value">The input value.</param>
    void OnMoveSelector(InputValue value)
    {
        inputDirection = value.Get<Vector2>();

        if (combatSystem.State == CombatState.PLAYER_ACTION_SELECT)
        {
            MoveActionCursor();
        }

        if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            MoveSkillSelector();
        }

        if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            MoveTargetCursor();
        }
    }

    /// <summary>
    /// Handles the submit input from the user, either confirming a selection or moving to the next UI element.
    /// </summary>
    void OnSubmit()
    {
        if (combatSystem.State == CombatState.PLAYER_ACTION_SELECT)
        {
            SelectAction();
        }
        else if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            SelectSkill();
        }
        else if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            StartCoroutine(AnimateCursorClick());
            playerHasChosenATarget = true;
            SelectTarget();
        }
    }

    /// <summary>
    /// Handles the cancel input from the user, returning to the previous UI element.
    /// </summary>
    void OnCancel()
    {
        if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            combatSystem.State = CombatState.PLAYER_ACTION_SELECT;
            cursor.position = lastActionPosition;
            ResetActionSelectPanels();
        }

        if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            if (playerHasChosenATarget) return;
            FindObjectOfType<UIStatDisplayHighlighter>().ResetHighlights();
            combatSystem.State = CombatState.PLAYER_SKILL_SELECT;
            selector.position = lastSkillSelectorPosition;
        }
    }

    /// <summary>
    /// Moves the action cursor based on the input direction.
    /// </summary>
    private void MoveActionCursor()
    {

        if (inputDirection.Equals(Vector2.right))
        {
            if (actionIndex < maxActionIndex)
            {
                cursor.position = selectableActionPositions[actionIndex + 1];
                actionIndex++;
            }
            else
            {
                actionIndex = 0;
                cursor.position = selectableActionPositions[0];
            }
        }

        if (inputDirection.Equals(Vector2.left))
        {
            if (actionIndex > 0)
            {
                cursor.position = selectableActionPositions[actionIndex - 1];
                actionIndex--;
            }
            else
            {
                actionIndex = maxActionIndex;
                cursor.position = selectableActionPositions[maxActionIndex];
            }
        }
    }

    /// <summary>
    /// Moves the skill selector based on the input direction.
    /// </summary>
    private void MoveSkillSelector()
    {
        var transformPosition = selector.transform.position;

        if (inputDirection.Equals(Vector2.up))
        {
            if (skillIndex > 0)
            {
                if (selectorPosition > 0)
                {
                    transformPosition.y += selectorMoveDistance;
                    selectorPosition--;
                }
                else if (selectorPosition == 0)
                {
                    this.contentRectTransform.offsetMax -= new Vector2(0, scrollOffset);
                }

                skillIndex--;
            }
        }

        if (inputDirection.Equals(Vector2.down))
        {
            if (skillIndex < maxSkillIndex)
            {
                if (selectorPosition < selectorScrollCap)
                {
                    transformPosition.y -= selectorMoveDistance;
                    selectorPosition++;
                }
                else if (selectorPosition == selectorScrollCap)
                {
                    this.contentRectTransform.offsetMax += new Vector2(0, scrollOffset);
                }

                skillIndex++;
            }

        }

        selector.position = transformPosition;
    }

    /// <summary>
    /// Moves the target cursor based on the input direction.
    /// </summary>
    private void MoveTargetCursor()
    {
        // If only one enemy, do not move cursor
        if (maxTargetIndex < 1) return;

        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.left))
        {
            if (targetIndex > 0)
            {
                cursor.position = selectableTargetPositions[targetIndex - 1];
                targetIndex--;
            }
            else
            {
                targetIndex = maxTargetIndex;
                cursor.position = selectableTargetPositions[maxTargetIndex];
            }
        }

        if (inputDirection.Equals(Vector2.down) || inputDirection.Equals(Vector2.right))
        {
            if (targetIndex < maxTargetIndex)
            {
                cursor.position = selectableTargetPositions[targetIndex + 1];
                targetIndex++;
            }
            else
            {
                targetIndex = 0;
                cursor.position = selectableTargetPositions[0];
            }
        }
    }

    /// <summary>
    /// Selects the current action based on the cursor position.
    /// </summary>
    void SelectAction()
    {
        switch (actionIndex)
        {
            case 0:
                if (IsActionDisabled(CombatAction.ATTACK)) return;

                activeAction = CombatAction.ATTACK;
                attackPanel.GetComponent<Image>().sprite = selectedAttackBG;
                ZoomSelectedActionIcon(attackAction);
                break;
            case 1:
                if (IsActionDisabled(CombatAction.DEFEND)) return;

                activeAction = CombatAction.DEFEND;
                defendPanel.GetComponent<Image>().sprite = selectedDefendBG;
                ZoomSelectedActionIcon(defendAction);
                break;
            case 2:
                if (IsActionDisabled(CombatAction.SUPPORT)) return;

                activeAction = CombatAction.SUPPORT;
                supportPanel.GetComponent<Image>().sprite = selectedSupportBG;
                ZoomSelectedActionIcon(supportAction);
                break;
        }

        lastActionPosition = cursor.position;

        // Reset Skill Selector
        maxSkillIndex = skillLoader.InitiateCombatMoves(activeAction);
        selector.position = defaultSkillSelectorPosition;
        selectorPosition = 0;
        skillIndex = 0;
        contentRectTransform.offsetMax = defaultRectTransformOffset;

        combatSystem.State = CombatState.PLAYER_SKILL_SELECT;

    }

    /// <summary>
    /// Selects the current skill based on the selector position.
    /// </summary>
    void SelectSkill()
    {
        CombatMove chosenSkill = skillLoader.GetSkill(skillIndex);

        if (chosenSkill.GetCooldownTracker().isMoveOnCooldown())
        {
            combatLog.MoveIsOnCooldown(chosenSkill);
            return;
        }

        if (combatSystem.Player.CombatEffectsManager.IsEffectActive(CombatEffectType.Silence))
        {
            combatLog.UnitIsSilenced(chosenSkill, combatSystem.Player);
            return;
        }

        // TODO IF USER IS SILENCED, CANNOT CHOOSE MAGIC SKILL

        // Save selector position
        // Set target cursor position to default or last selected target.
        lastSkillSelectorPosition = selector.position;

        cursor.position = defaultTargetCursorPosition; // Should be last selected target
        targetIndex = 0; // When above is last selected target, do not reset index.

        combatSystem.OnSkillSelect(chosenSkill);
    }

    /// <summary>
    /// Selects the current target based on the cursor position.
    /// </summary>
    void SelectTarget()
    {
        combatSystem.OnTargetSelect(targetIndex);
        ResetActionSelectPanels();
    }

    /// <summary>
    /// Zooms in the selected action icon.
    /// </summary>
    /// <param name="action">The action GameObject to zoom.</param>
    private void ZoomSelectedActionIcon(GameObject action)
    {
        action.GetComponent<RectTransform>().localScale = new Vector3(selectedActionScale, selectedActionScale);
    }

    /// <summary>
    /// Animates the cursor click by changing the sprite and waiting for a specified time.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator AnimateCursorClick()
    {
        cursor.GetComponentInChildren<SpriteRenderer>().sprite = cursorOnSelect;
        yield return new WaitForSeconds(pointerClickAnimationTime);
        cursor.GetComponentInChildren<SpriteRenderer>().sprite = cursorIdle;
    }

    /// <summary>
    /// Resets the action select panels to their default state.
    /// </summary>
    void ResetActionSelectPanels()
    {
        attackPanel.GetComponent<Image>().sprite = defaultAttackBG;
        defendPanel.GetComponent<Image>().sprite = defaultDefendBG;
        supportPanel.GetComponent<Image>().sprite = defaultSupportBG;

        attackAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        defendAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        supportAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
    }

    /// <summary>
    /// Moves the cursor to the default action select position.
    /// </summary>
    public void MoveCursorToDefaultActionSelect()
    {
        actionIndex = 0;
        cursor.position = defaultActionCursorPosition;
    }

    /// <summary>
    /// Property to get or set whether the player has chosen a target.
    /// </summary>
    public bool PlayerHasChosenATarget
    {
        get => playerHasChosenATarget;
        set => playerHasChosenATarget = value;
    }

    /// <summary>
    /// Property to get or set the current target index.
    /// </summary>
    public int TargetIndex
    {
        get => targetIndex;
        set => targetIndex = value;
    }

    /// <summary>
    /// Property to get or set the maximum target index.
    /// </summary>
    public int MaxTargetIndex
    {
        get => maxTargetIndex;
        set => maxTargetIndex = value;
    }
}