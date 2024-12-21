using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a dialogue in the game, containing a list of lines to be displayed.
/// This class is used to store and manage dialogue content for interactions between characters or with the player.
/// </summary>
[Serializable]
public class Dialogue
{
    [SerializeField] private List<string> lines; // The list of lines in the dialogue.

    /// <summary>
    /// Gets or sets the list of lines in the dialogue.
    /// </summary>
    public List<string> Lines
    {
        get => lines;
        set => lines = value;
    }
}