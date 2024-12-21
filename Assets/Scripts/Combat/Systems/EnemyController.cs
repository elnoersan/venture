using UnityEngine;

// This class controls the behavior of an enemy in a game
public class EnemyController : MonoBehaviour
{
    // A serialized field to allow the attack move to be set in the Unity Inspector
    [SerializeField] private CombatMove attackMove;

    // This method is used to make the enemy use its skill (attack move)
    public CombatMove UseSkill()
    {
        // Return the attack move that the enemy will use
        return attackMove;
    }
}