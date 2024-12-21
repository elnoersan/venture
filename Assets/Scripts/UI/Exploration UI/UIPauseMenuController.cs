using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenuController : MonoBehaviour
{
    [SerializeField] private Button settingsButton; // Button for opening the settings menu
    [SerializeField] private Button saveGameButton; // Button for saving the game
    [SerializeField] private Button exitGameButton; // Button for exiting the game

    // Initialize the pause menu and set up button listeners
    private void Start()
    {
        // Disable the settings button (assuming it's not implemented yet)
        settingsButton.gameObject.SetActive(false);

        // Add a listener to the save game button
        saveGameButton.onClick.AddListener(() => SaveGame());

        // Add a listener to the exit game button
        exitGameButton.onClick.AddListener(() => ExitGame());
    }

    // Saves the game using the DataPersistenceManager
    private void SaveGame()
    {
        FindObjectOfType<DataPersistenceManager>().SaveGame();
    }

    // Exits the game
    private void ExitGame()
    {
        Application.Quit();
    }
}