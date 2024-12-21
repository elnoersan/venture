using System;
using UnityEngine;

// A serializable class that represents data for a combat encounter
[Serializable]
public class CombatEncounterData : SaveData
{
    [SerializeField] private int areaEncounterRate; // The encounter rate for this area (e.g., how often enemies appear)
    [SerializeField] private int amountOfEnemiesToSpawn; // The number of enemies to spawn in this encounter
    [SerializeField] private int enemyLvl; // The level of the enemies in this encounter

    // Property to get or set the area encounter rate
    public int AreaEncounterRate
    {
        get => areaEncounterRate;
        set => areaEncounterRate = value;
    }

    // Property to get or set the amount of enemies to spawn
    public int AmountOfEnemiesToSpawn
    {
        get => amountOfEnemiesToSpawn;
        set => amountOfEnemiesToSpawn = value;
    }

    // Property to get or set the enemy level
    public int EnemyLvl
    {
        get => enemyLvl;
        set => enemyLvl = value;
    }

    // Method to reset data before saving (not implemented yet)
    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}