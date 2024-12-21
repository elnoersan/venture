using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIExplController : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private Sprite activeUIButton; // Sprite for active UI buttons
    [SerializeField] private Sprite inactiveUIButton; // Sprite for inactive UI buttons

    [Header("Character Stat Screen")]
    [SerializeField] private GameObject characterStats; // Character stats screen
    [SerializeField] private Image characterStatsButtonImage; // Button image for character stats

    [Header("Skills Screen")]
    [SerializeField] private GameObject skills; // Skills screen
    [SerializeField] private Image skillsButtonImage; // Button image for skills

    [Header("Talents Screen")]
    [SerializeField] private GameObject talents; // Talents screen
    [SerializeField] private Image talentsButtonImage; // Button image for talents

    [Header("Quest Screen")]
    [SerializeField] private GameObject quests; // Quest screen
    [SerializeField] private Image questsButtonImage; // Button image for quests

    [Header("Inventory Screen")]
    [SerializeField] private GameObject inventory; // Inventory screen
    [SerializeField] private Image inventoryStatsButtonImage; // Button image for inventory

    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseMenu; // Pause menu screen
    [SerializeField] private Image pauseMenuButtonImage; // Button image for pause menu

    private bool areAllElementsHidden = true; // Tracks if all UI elements are hidden
    private bool firstSetup = true; // Tracks if this is the first setup
    private bool pauseGame; // Tracks if the game is paused

    private EventSystem eventSystem; // Reference to the EventSystem
    private List<GameObject> uiObjects; // List of all UI screens
    private List<Image> uiHUDIcons; // List of all UI button images

    // Initialize the UI and set up event listeners
    private void Start()
    {
        // Initialize the inventory UI
        inventory.GetComponent<UIInventoryController>().InitInventoryUI();

        // Get the EventSystem reference
        eventSystem = FindObjectOfType<EventSystem>();

        // Subscribe to the onShowDialog event to close UI when a dialog is shown
        GameEvents.Instance.onShowDialog += () =>
        {
            FindObjectOfType<UIExplController>().CloseUIOnDialog();
        };

        // Populate the list of UI screens and their corresponding button images
        uiObjects = new List<GameObject>
        {
            characterStats, inventory, quests, skills, pauseMenu, talents
        };
        uiHUDIcons = new List<Image>()
        {
            characterStatsButtonImage, inventoryStatsButtonImage, questsButtonImage, skillsButtonImage, pauseMenuButtonImage, talentsButtonImage
        };

        // Hide all UI elements initially
        HideUI();
    }

    // Hides or shows all UI elements
    public void HideUI()
    {
        // Handle the first setup
        if (firstSetup)
        {
            // Check if the number of UI objects matches the number of button images
            if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

            // Hide all UI elements and set their button images to inactive
            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].SetActive(false);
                uiHUDIcons[i].sprite = inactiveUIButton;
            }

            firstSetup = false;
        }
        else
        {
            // Check if all UI elements are hidden
            areAllElementsHidden = uiObjects.All(obj => !obj.activeSelf);

            // If all elements are hidden, show the pause menu and pause the game
            if (areAllElementsHidden)
            {
                FindObjectOfType<GameManager>().ExplorationState = ExplorationState.Pause;
                TogglePauseMenu();
            }
            else
            {
                // Check if the number of UI objects matches the number of button images
                if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

                // Hide all UI elements and set their button images to inactive
                for (int i = 0; i < uiObjects.Count; i++)
                {
                    uiObjects[i].SetActive(false);
                    uiHUDIcons[i].sprite = inactiveUIButton;
                }

                // Set the game state to exploration
                FindObjectOfType<GameManager>().ExplorationState = ExplorationState.Explore;
            }
        }
    }

    // Closes all UI elements when a dialog is shown
    public void CloseUIOnDialog()
    {
        // Check if the number of UI objects matches the number of button images
        if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

        // Hide all UI elements and set their button images to inactive
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(false);
            uiHUDIcons[i].sprite = inactiveUIButton;
        }
    }

    // Toggles the character stats screen
    public void ToggleCharacterStats()
    {
        characterStats.SetActive(!characterStats.activeSelf);
        characterStatsButtonImage.sprite = characterStats.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }

    // Toggles the inventory screen
    public void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
        inventoryStatsButtonImage.sprite = inventory.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }

    // Toggles the quests screen
    public void ToggleQuests()
    {
        quests.SetActive(!quests.activeSelf);
        questsButtonImage.sprite = quests.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }

    // Toggles the skills screen
    public void ToggleSkills()
    {
        skills.SetActive(!skills.activeSelf);
        skillsButtonImage.sprite = skills.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }

    // Toggles the pause menu
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseMenuButtonImage.sprite = pauseMenu.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }

    // Toggles the talents menu
    public void ToggleTalentsMenu()
    {
        talents.SetActive(!talents.activeSelf);
        talentsButtonImage.sprite = talents.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null); // Deselect any selected UI element
    }
}