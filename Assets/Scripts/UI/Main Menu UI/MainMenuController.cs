using UnityEngine;

public class MainMenuController : PersistentSingleton<MainMenuController>
{
    private bool isNewGame; // Flag to determine if the game is starting as a new game

    // Starts a new game
    public void NewGame()
    {
        isNewGame = true; // Set the flag to true for a new game
        StartGame(); // Call the StartGame method
    }

    // Continues an existing game
    public void ContinueGame()
    {
        isNewGame = false; // Set the flag to false for continuing a game
        StartGame(); // Call the StartGame method
    }

    // Exits the game
    public void ExitGame()
    {
        Application.Quit(); // Quit the application
    }

    // Returns whether the game is starting as a new game and destroys the MainMenuController instance
    public bool IsNewGame()
    {
        Destroy(this.gameObject); // Destroy the MainMenuController instance
        return isNewGame; // Return the value of the isNewGame flag
    }

    // Starts the game by loading the exploration scene
    public void StartGame()
    {
        FindObjectOfType<SceneTransition>().LoadScene(SceneIndexType.Exploration); // Load the exploration scene
    }
}