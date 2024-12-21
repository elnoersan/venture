using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/// <summary>
/// Controls the player's movement, interaction, and animations in the exploration scene.
/// This class handles player input, movement logic, and interactions with the environment.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed; // The speed at which the player moves.
    [SerializeField] private bool isMoving; // Whether the player is currently moving.
    [SerializeField] private LayerMask blockingLayer; // The layer mask for blocking objects.
    [SerializeField] private LayerMask waterLayer; // The layer mask for water areas.
    [SerializeField] private LayerMask combatLayer; // The layer mask for combat encounter areas.
    [SerializeField] private LayerMask interactablesLayer; // The layer mask for interactable objects.

    private Animator animator; // The Animator component for controlling animations.
    private UIExplController uiController; // The UI controller for managing exploration UI.
    private CombatEncounterManager combatEncounterManager; // The manager for combat encounter settings.
    private GameManager gameManager; // The game manager for handling game state and data.
    private DialogueManager dialogueManager; // The dialogue manager for handling dialogue interactions.

    private Vector2 inputDirection; // The input direction from the player.
    private PlayerFacing playerFacingDirection; // The current facing direction of the player.

    // Cached animator references
    private static readonly int MoveX = Animator.StringToHash("moveX"); // The hash for the moveX animation parameter.
    private static readonly int MoveY = Animator.StringToHash("moveY"); // The hash for the moveY animation parameter.
    private static readonly int IsMoving = Animator.StringToHash("isMoving"); // The hash for the isMoving animation parameter.

    /// <summary>
    /// Initializes the player controller by finding necessary components and setting the player's position.
    /// </summary>
    private IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        uiController = FindObjectOfType<UIExplController>();
        combatEncounterManager = FindObjectOfType<CombatEncounterManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();

        yield return new WaitForEndOfFrame();

        // Set the player's position to the saved position from the GameManager.
        transform.position = FindObjectOfType<GameManager>().PlayerData.position;
    }

    /// <summary>
    /// Updates the player's movement and interactions based on the current game state.
    /// </summary>
    void Update()
    {
        if (GameManager.Instance.ExplorationState != ExplorationState.Explore) return;
        CheckTargetPosAndMove();
    }

    /// <summary>
    /// Checks the target position and moves the player if the target position is walkable.
    /// </summary>
    private void CheckTargetPosAndMove()
    {
        animator.SetBool(IsMoving, isMoving);

        if (isMoving) return;
        if (inputDirection.Equals(Vector2.zero)) return;

        var targetPosition = transform.position;
        animator.SetFloat(MoveX, inputDirection.x);
        animator.SetFloat(MoveY, inputDirection.y);

        if (inputDirection.Equals(Vector2.left) || inputDirection.Equals(Vector2.right))
        {
            targetPosition.x += inputDirection.x;
            UpdatePlayerFacing();
        }

        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.down))
        {
            targetPosition.y += inputDirection.y;
            UpdatePlayerFacing();
        }

        if (IsTargetPositionWalkable(targetPosition)) StartCoroutine(MovePlayer(targetPosition));
    }

    /// <summary>
    /// Updates the player's facing direction based on the input direction.
    /// </summary>
    public void UpdatePlayerFacing()
    {
        if (inputDirection.x != 0)
        {
            playerFacingDirection = inputDirection.x > 0 ? PlayerFacing.East : PlayerFacing.West;
        }
        else if (inputDirection.y != 0)
        {
            playerFacingDirection = inputDirection.y > 0 ? PlayerFacing.North : PlayerFacing.South;
        }
    }

    /// <summary>
    /// Sets the player's facing direction based on the specified direction.
    /// </summary>
    /// <param name="direction">The direction to set the player's facing to.</param>
    public void SetPlayerFacing(PlayerFacing direction)
    {
        switch (direction)
        {
            case PlayerFacing.North:
                animator.SetFloat(MoveX, 0);
                animator.SetFloat(MoveY, 1);
                break;
            case PlayerFacing.South:
                animator.SetFloat(MoveX, 0);
                animator.SetFloat(MoveY, -1);
                break;
            case PlayerFacing.East:
                animator.SetFloat(MoveX, 1);
                animator.SetFloat(MoveY, 0);
                break;
            case PlayerFacing.West:
                animator.SetFloat(MoveX, -1);
                animator.SetFloat(MoveY, 0);
                break;
        }
    }

    /// <summary>
    /// Handles the movement input from the player.
    /// </summary>
    /// <param name="input">The input value containing the movement direction.</param>
    void OnMove(InputValue input)
    {
        inputDirection = input.Get<Vector2>();
    }

    /// <summary>
    /// Toggles the character stats UI.
    /// </summary>
    void OnOpenCharacterStats()
    {
        uiController.ToggleCharacterStats();
    }

    /// <summary>
    /// Toggles the inventory UI.
    /// </summary>
    void OnOpenInventory()
    {
        uiController.ToggleInventory();
    }

    /// <summary>
    /// Toggles the quests UI.
    /// </summary>
    void OnOpenQuests()
    {
        uiController.ToggleQuests();
    }

    /// <summary>
    /// Toggles the skills UI.
    /// </summary>
    void OnOpenSkills()
    {
        uiController.ToggleSkills();
    }

    /// <summary>
    /// Toggles the talents menu UI.
    /// </summary>
    void OnOpenTalents()
    {
        uiController.ToggleTalentsMenu();
    }

    /// <summary>
    /// Hides the UI.
    /// </summary>
    void OnCancel()
    {
        uiController.HideUI();
    }

    /// <summary>
    /// Handles the interaction input from the player.
    /// </summary>
    void OnInteract()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.ExplorationState == ExplorationState.Explore)
        {
            Interact();
        }
        else if (gameManager.ExplorationState == ExplorationState.Dialog)
        {
            FindObjectOfType<DialogueManager>().NextLine();
        }
    }

    /// <summary>
    /// Interacts with an object or NPC in front of the player.
    /// </summary>
    private void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactablesLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    /// <summary>
    /// Moves the player to the target position over time.
    /// </summary>
    /// <param name="targetPosition">The target position to move the player to.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

        FindObjectOfType<GameManager>().SavePositionBeforeCombat();
        CheckForCombat();
    }

    /// <summary>
    /// Checks if the target position is walkable.
    /// </summary>
    /// <param name="targetPosition">The target position to check.</param>
    /// <returns>True if the target position is walkable; otherwise, false.</returns>
    private bool IsTargetPositionWalkable(Vector3 targetPosition)
    {
        return !Physics2D.OverlapCircle(targetPosition, 0.15f, blockingLayer | waterLayer | interactablesLayer);
    }

    /// <summary>
    /// Checks if the player has entered a combat encounter area.
    /// </summary>
    private void CheckForCombat()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, combatLayer) != null)
        {
            if ((Random.Range(1, 101) <= combatEncounterManager.AreaEncounterRate))
            {
                Debug.Log("Encountered combat!");
                FindObjectOfType<GameManager>().SavePositionBeforeCombat();
                FindObjectOfType<SceneTransition>().LoadScene(SceneIndexType.Combat);
            }
        }
    }

    /// <summary>
    /// Gets or sets the player's facing direction.
    /// </summary>
    public PlayerFacing PlayerFacingDirection
    {
        get => playerFacingDirection;
        set => playerFacingDirection = value;
    }
}