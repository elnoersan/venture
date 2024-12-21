using System.Collections.Generic;
using TMPro;
using UnityEngine;

// A class that manages the combat log UI
public class UICombatLog : MonoBehaviour
{
    [SerializeField] private GameObject combatLoGameObject; // The parent GameObject containing the combat log text elements
    private List<TextMeshProUGUI> combatLog; // A list of TextMeshProUGUI elements representing the combat log lines

    // Initialize the combat log
    private void Awake()
    {
        combatLog = new List<TextMeshProUGUI>();

        // Get all TextMeshProUGUI components in the children of the combat log GameObject
        TextMeshProUGUI[] textLineItems = combatLoGameObject.GetComponentsInChildren<TextMeshProUGUI>();

        // Add each TextMeshProUGUI component to the combat log list
        foreach (var textMeshProUGUI in textLineItems)
        {
            combatLog.Add(textMeshProUGUI);
        }
    }

    /*
     * Printing to the combat log and moving the lines in the UI.
     * Functionality methods
     */

    // Print a message to the combat log
    public void PrintToLog(string msg)
    {
        // Shift all existing log lines up by one position
        for (int i = 0; i < combatLog.Count - 1; i++)
        {
            combatLog[i].text = combatLog[i + 1].text;
        }

        // Add the new message to the last line of the combat log
        combatLog[combatLog.Count - 1].text = msg;
    }

    // Clear the combat log
    public void Clear()
    {
        combatLog.ForEach(textObject => textObject.text = "");
    }

    /*
     * Event specific text:
     */

    // Print the start of combat message
    public void StartOfCombat()
    {
        string line = "Enemies have appeared. Speed determines turn order.";
        PrintToLog(line);
    }

    // Print the remaining player actions message
    public void NextPlayerAction(int remainingActions)
    {
        string line = "";
        if (remainingActions > 1)
        {
            line = "You have " + remainingActions + " remaining actions left.";
        }
        else
        {
            line = "You have another action left.";
        }

        PrintToLog(line);
    }

    // Print the player's turn message
    public void PlayerTurn()
    {
        string line = "Player's turn! Choose an action.";
        PrintToLog(line);
    }

    // Print the enemy's turn message
    public void EnemyTurn(CombatUnit enemy)
    {
        string line = enemy.UnitName + "'s turn to act:";
        PrintToLog(line);
    }

    // Print the player won message
    public void PlayerWon()
    {
        string line = "All enemies defeated. You won!";
        PrintToLog(line);
    }

    // Print the player healed message
    public void PlayerHealed(CombatMove move)
    {
        string line = "Your " + move.GetName() + " healed for " + move.GetPower();
        PrintToLog(line);
    }

    // Print the player applied block message
    public void PlayerAppliedBlock(CombatMove move)
    {
        string line = "You gained " + move.GetPower() + " physical block!";
        PrintToLog(line);
    }

    // Print the unit applied effect message
    public void UnitAppliedEffect(CombatUnit unit, CombatMove move)
    {
        string line = unit.UnitName + " gained " + move.GetName() + ".";
        PrintToLog(line);
    }

    // Print the used offensive combat move message
    public void UsedOffensiveCombatMove(CombatMove move, CombatUnit attacker, CombatUnit target, float damage)
    {
        string line = attacker.UnitName + " used " + move.GetName() +
                      " on " + target.UnitName;
        PrintToLog(line);
        line = ">> It hit for " + damage + " " + move.GetType() + " damage.";
        PrintToLog(line);
    }

    // Print the damage player message
    public void DamagePlayer(CombatUnit enemy, CombatMove move, float incomingDamage)
    {
        string line = enemy.UnitName + " used " + move.GetName() +
                      ". It hit for " + incomingDamage + " " + move.GetType() + " damage.";
        PrintToLog(line);
    }

    // Print the move is on cooldown message
    public void MoveIsOnCooldown(CombatMove move)
    {
        string line = move.GetName() + " is currently on cooldown.";
        PrintToLog(line);
    }

    // Print the unit is silenced message
    public void UnitIsSilenced(CombatMove move, CombatUnit unit)
    {
        string line = move.GetName() + " is currently on cooldown.";
        PrintToLog(line);
    }
}