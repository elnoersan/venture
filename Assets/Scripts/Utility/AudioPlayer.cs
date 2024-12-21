using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip shootingSFX; // Audio clip for shooting sound effects
    [SerializeField][Range(0f, 1f)] private float shootingFXVolume = 0.25f; // Volume for shooting sound effects

    private bool isAudioEnabled = true; // Flag to enable or disable audio

    // Static instance version of Singleton pattern
    private static AudioPlayer instance;

    // Initialize the audio player and manage the Singleton pattern
    private void Awake()
    {
        // Play the audio if it's enabled
        if (isAudioEnabled) GetComponent<AudioSource>().Play();

        // Manage the Singleton pattern to ensure only one instance exists
        ManageSingleton();
    }

    // Manages the Singleton pattern
    void ManageSingleton()
    {
        // Check if an instance already exists and audio is enabled
        if (instance != null && isAudioEnabled)
        {
            // Disable and destroy the current instance
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            // Set the current instance as the Singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    // Toggles audio playback
    public void SetAudio()
    {
        // Toggle the audio enabled flag
        isAudioEnabled = !isAudioEnabled;

        // Get the AudioSource component
        AudioSource theme = GetComponent<AudioSource>();

        // If audio is playing, stop it; otherwise, play it
        if (theme.isPlaying)
        {
            theme.Stop();
        }
        else
        {
            theme.Play();
        }
    }

    /* Used for Static version of Singleton pattern
    public AudioPlayer GetInstance()
    {
        return instance;
    }
    */

    /*
    public void PlayShootingFX()
    {
        // Play the shooting sound effect at the camera's position with the specified volume
        if (shootingSFX != null && isAudioEnabled && isLaserSoundsEnabled)
        {
            AudioSource.PlayClipAtPoint(
                shootingSFX, 
                Camera.main.transform.position, 
                shootingFXVolume
            );
        }
    }
    */
}