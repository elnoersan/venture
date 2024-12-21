using UnityEngine;

public class ConsumableManager : PersistentSingleton<ConsumableManager>
{
    /*
     * Currently items are only enabled in exploring mode
     * When enabling items in combat, make sure to change controller calls
     *  (e.g. GameManager for exploring mode) based on the current scene
     */
    public void UseItem(ConsumableItem item)
    {
        // Determine the type of consumable item and handle it accordingly
        switch (item.Type)
        {
            case ConsumableType.HpRecovery:
                // Find the GameManager in the scene to access player data
                GameManager gameManager = FindObjectOfType<GameManager>();

                // Get the current HP and max HP of the player
                var currentHp = gameManager.PlayerData.currentHp;
                var maxHp = gameManager.PlayerData.unitBase.MaxHp;

                // Check if using the item would exceed the player's max HP
                if ((currentHp + item.Value) > maxHp)
                {
                    // If so, set the player's HP to max HP
                    gameManager.PlayerData.currentHp = maxHp;
                }
                else
                {
                    // Otherwise, add the item's value to the player's current HP
                    currentHp += item.Value;
                    gameManager.PlayerData.currentHp = Mathf.Round(currentHp); // Round the HP to avoid decimals
                }

                break;
        }
    }
}