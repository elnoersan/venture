using UnityEngine;

// Base class for creating a static instance of a MonoBehaviour
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    // Public property to access the instance
    public static T Instance { get; private set; }

    // Set the instance to this object in Awake
    protected virtual void Awake() => Instance = this as T;

    // Clear the instance and destroy the object when the application quits
    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

// Base class for creating a Singleton pattern
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    // Override Awake to ensure only one instance exists
    protected override void Awake()
    {
        // If an instance already exists, destroy this object
        if (Instance != null) Destroy(gameObject);

        // Call the base Awake method to set the instance
        base.Awake();
    }
}

// Base class for creating a Persistent Singleton pattern
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    // Override Awake to ensure the Singleton persists across scenes
    protected override void Awake()
    {
        // If an instance already exists, destroy this object
        if (Instance != null) Destroy(gameObject);

        // Mark the object to persist across scenes
        DontDestroyOnLoad(gameObject);

        // Ensure only one instance exists by destroying any additional objects of the same type
        T[] allObjectsOfType = FindObjectsOfType<T>();
        if (allObjectsOfType.Length > 1)
        {
            Destroy(allObjectsOfType[1].gameObject);
        }

        // Call the base Awake method to set the instance
        base.Awake();
    }
}