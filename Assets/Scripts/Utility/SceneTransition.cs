using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator crossfadeAnimator; // Reference to the Animator component for crossfade animations
    private float animationSpeed = 1f; // Speed of the crossfade animation
    private Dictionary<SceneIndexType, int> indexToType; // Dictionary mapping SceneIndexType to build indices

    // Initialize the crossfade animator and the scene index mapping
    private void Start()
    {
        // Get the Animator component from the child GameObject
        crossfadeAnimator = GetComponentInChildren<Animator>();

        // Initialize the dictionary with SceneIndexType keys and corresponding build indices
        indexToType = new Dictionary<SceneIndexType, int>
        {
            {SceneIndexType.MainMenu, 0},
            {SceneIndexType.Exploration, 1},
            {SceneIndexType.Combat, 2},
        };
    }

    // Loads a scene with a crossfade animation
    public void LoadScene(SceneIndexType scene)
    {
        // Start the coroutine to animate the scene transition
        StartCoroutine(AnimateSceneTransition(indexToType[scene]));
    }

    // Coroutine to animate the scene transition
    IEnumerator AnimateSceneTransition(int buildIndex)
    {
        // Trigger the "Start" animation in the crossfade animator
        crossfadeAnimator.SetTrigger("Start");

        // Wait for the duration of the animation
        yield return new WaitForSeconds(animationSpeed);

        // Load the specified scene by build index
        SceneManager.LoadScene(buildIndex);
    }
}