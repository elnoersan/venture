using System;
using UnityEngine;

// A serializable class that represents player data for saving and loading
[Serializable]
public class PlayerData : SaveData
{
    // Progress stats
    [SerializeField] public int level; // The player's current level
    [SerializeField] public int exp; // The player's current experience points
    [SerializeField] public int nextLvLExp; // The experience required to reach the next level
    [SerializeField] public int remainingStatPoints; // The number of stat points the player has remaining

    // Exploration stats
    [SerializeField] public int sceneIndex; // The index of the current scene the player is in
    [SerializeField] public Vector3 position; // The player's current position in the world
    [SerializeField] public PlayerFacing playerFacingDirection; // The direction the player is facing

    // Combat Stats
    [SerializeField] public UnitBase unitBase; // The player's base stats and attributes
    [SerializeField] public float currentHp; // The player's current health points
    //private CombatEffectManager effectManager; // (Commented out) Manager for combat effects

    // Constructor to initialize the player data
    public PlayerData(int level, int exp, int nextLvLExp, int remainingStatPoints, int sceneIndex, Vector3 position, PlayerFacing playerFacingDirection, UnitBase unitBase, float currentHp)
    {
        this.level = level;
        this.exp = exp;
        this.nextLvLExp = nextLvLExp;
        this.remainingStatPoints = remainingStatPoints;
        this.sceneIndex = sceneIndex;
        this.position = position;
        this.playerFacingDirection = playerFacingDirection;
        this.unitBase = unitBase;
        this.currentHp = currentHp;
    }

    // Method to reset data before saving (not implemented yet)
    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}