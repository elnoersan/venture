using UnityEngine;

/// <summary>
/// Handles the loading of the player's position and facing direction when transitioning to the exploration scene.
/// This class ensures that the player resumes from the correct position and facing direction after a scene transition.
/// </summary>
public class ExplorationLoader : MonoBehaviour
{
    /// <summary>
    /// Initializes the player's position and facing direction based on the saved data in the GameManager.
    /// </summary>
    void Start()
    {
        // Find the PlayerController and GameManager in the scene.
        PlayerController player = FindObjectOfType<PlayerController>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        // Set the player's position to the saved position from the GameManager.
        player.transform.position = gameManager.PlayerData.position;

        // Set the player's facing direction to the saved direction from the GameManager.
        player.SetPlayerFacing(gameManager.PlayerData.playerFacingDirection);
    }
}