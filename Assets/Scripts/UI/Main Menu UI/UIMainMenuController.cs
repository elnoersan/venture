using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private Button continueBtn; // Button for continuing the game
    [SerializeField] private Button settingsBtn; // Button for opening the settings menu
    [SerializeField] private string fileName = "data.game"; // Name of the save file
    public bool loadGameFound; // Flag to indicate if a save file is found
    private IFileHandler fileHandler; // Reference to the file handler interface

    // Initialize the main menu UI and check for save files
    private void Start()
    {
        // Initialize the file handler with the persistent data path and file name
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);

        // Load the game data from the save file
        GameData gd = fileHandler.Load();

        // Check if a save file was found
        loadGameFound = gd != null;

        // Enable or disable the continue button based on whether a save file was found
        continueBtn.gameObject.SetActive(loadGameFound);

        // Disable the settings button (assuming it's not implemented yet)
        settingsBtn.gameObject.SetActive(false);
    }
}