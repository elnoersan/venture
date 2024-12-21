using System;
using System.Collections.Generic;
using UnityEngine;

// A serializable dictionary class that can be saved and loaded in Unity
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>(); // List to store dictionary keys
    [SerializeField] private List<TValue> values = new List<TValue>(); // List to store dictionary values

    // Called before serialization (saving)
    public void OnBeforeSerialize()
    {
        keys.Clear(); // Clear the keys list
        values.Clear(); // Clear the values list

        // Iterate through the dictionary and add keys and values to the lists
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Called after deserialization (loading)
    public void OnAfterDeserialize()
    {
        this.Clear(); // Clear the dictionary

        // Check if the number of keys and values match
        if (keys.Count != values.Count)
        {
            Debug.Log("Error when attempting to deserialize SerializableDictionary. " +
                      "Amount of keys (" + keys.Count + ") " +
                      "does not match amount of values (" + values.Count + ")");
        }

        // Rebuild the dictionary from the keys and values lists
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}