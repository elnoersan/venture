using System.Collections.Generic;
using UnityEngine;

// A singleton class that manages combat moves in the game
public class SkillManager : PersistentSingleton<SkillManager>
{
    [SerializeField] private List<CombatMoveBase> combatMoveBases; // A comprehensive list of all combat moves in the game to draw from
    [SerializeField] private CombatMoveBase testMove; // A test combat move for debugging purposes
    private List<CombatMove> activeCombatMoves; // The list of currently active combat moves

    // Initialize the SkillManager
    void Start()
    {
        // Temporary loader:
        activeCombatMoves = new List<CombatMove>();
        combatMoveBases.ForEach(baze => activeCombatMoves.Add(new CombatMove(baze))); // Create instances of combat moves from the base list
    }

    // Get a test combat move (for debugging purposes)
    public CombatMove GetTestMove()
    {
        /*
        Random rnd = new Random(10);
        return activeCombatMoves[rnd.NextInt(0, activeCombatMoves.Count)];
        */
        return new CombatMove(testMove); // Return a new instance of the test move
    }

    // Put a combat move on cooldown
    public void PutCombatMoveOnCooldown(CombatMove move)
    {
        Debug.Log("Trying to put move on CD: " + move.GetName());
        // Find the corresponding combat move in the active list and put it on cooldown
        activeCombatMoves.Find(combatMove => combatMove.Equals(move)).GetCooldownTracker().PutMoveOnCooldown(move.GetCooldown());
    }

    // Decrease the cooldowns of all active combat moves
    public void DecreaseCooldowns()
    {
        activeCombatMoves.ForEach(move => move.GetCooldownTracker().DecreaseCooldownCounter());
    }

    // Get the list of currently active combat moves
    public List<CombatMove> GetActiveCombatMoves()
    {
        return activeCombatMoves;
    }
}