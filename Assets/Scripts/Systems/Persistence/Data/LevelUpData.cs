using System;
using UnityEngine;

// A serializable class that represents level-up data for saving and loading
[Serializable]
public class LevelUpData : SaveData
{
    [SerializeField] private int statPointsPrLevel; // Number of stat points gained per level
    [SerializeField] private int capBaseGrowth; // Base growth value for the next level's experience requirement
    [SerializeField] private int capGrowthRate; // Growth rate for increasing the experience requirement
    [SerializeField] private float capMultiplier; // Multiplier for the growth rate

    // Property to get or set the number of stat points gained per level
    public int StatPointsPrLevel
    {
        get => statPointsPrLevel;
        set => statPointsPrLevel = value;
    }

    // Property to get or set the base growth value for the next level's experience requirement
    public int CapBaseGrowth
    {
        get => capBaseGrowth;
        set => capBaseGrowth = value;
    }

    // Property to get or set the growth rate for increasing the experience requirement
    public int CapGrowthRate
    {
        get => capGrowthRate;
        set => capGrowthRate = value;
    }

    // Property to get or set the multiplier for the growth rate
    public float CapMultiplier
    {
        get => capMultiplier;
        set => capMultiplier = value;
    }

    // Method to reset data before saving (not implemented yet)
    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}