using UnityEngine;

public class TalentManager : MonoBehaviour
{
    [Header("Blocking")] // Group related fields under a header in the Unity Inspector
    [SerializeField] private bool consumeBlock; // Whether block is consumed when damage is taken
    [SerializeField] private bool expireBlock;   // Whether block expires after a certain condition
    [SerializeField] private bool blockingCanBeSplit; // Whether leftover block can be saved

    [Header("Mitigation")] // Group related fields under a header in the Unity Inspector
    [SerializeField] private bool consumeMitigation; // Whether mitigation is consumed when damage is taken

    private CombatUnit unit; // Reference to the CombatUnit script attached to this GameObject

    // Initialize the reference to the CombatUnit script
    private void Awake()
    {
        unit = GetComponent<CombatUnit>();
    }

    // Calculate the remaining damage after applying block
    public float CalculateBlock(float damageAfterMitigation)
    {
        Debug.Log("Calculating Block");
        float totalPhysicalBlock = unit.CurrentPhysicalBlock + unit.PhysicalBlockPower; // Calculate total block
        Debug.Log(totalPhysicalBlock);

        // Clamp the damage after block to ensure it doesn't go below 0
        float damageAfterBlock = Mathf.Clamp((damageAfterMitigation - totalPhysicalBlock), 0, float.MaxValue);

        // Handle/reset block power based on talent settings
        if (ConsumeBlock)
        {
            if (damageAfterMitigation > totalPhysicalBlock)
            {
                // If damage exceeds block, consume all current block
                unit.CurrentPhysicalBlock = 0;
            }
            else // If damage is less than or equal to block
            {
                // If leftover block can be saved
                if (BlockingCanBeSplit)
                {
                    // Subtract only the used block
                    unit.CurrentPhysicalBlock -= damageAfterMitigation;
                }
                else
                {
                    // Otherwise, reset block to 0
                    unit.CurrentPhysicalBlock = 0;
                }
            }
        }

        return damageAfterBlock; // Return the remaining damage after block
    }

    // Calculate the remaining damage after applying physical mitigation
    public float CalculatePhysicalMitigation(float damage)
    {
        // Apply mitigation to reduce damage
        float damageAfterMitigation = damage * (1 - unit.CurrentPhysicalMitigation);

        // If mitigation is consumed, reset it to 0
        if (consumeMitigation)
        {
            unit.CurrentPhysicalMitigation = 0;
        }

        return damageAfterMitigation; // Return the remaining damage after mitigation
    }

    // Calculate the remaining damage after applying armor
    public float CalculateArmor(float damageAfterBlock)
    {
        // Subtract armor from the damage and clamp it to ensure it doesn't go below 0
        return Mathf.Clamp((damageAfterBlock - unit.CurrentPhysDef), 0, float.MaxValue);
    }

    // Properties for accessing and modifying talent settings
    public bool ConsumeBlock
    {
        get => consumeBlock; // Getter for consumeBlock
        set => consumeBlock = value; // Setter for consumeBlock
    }

    public bool ExpireBlock
    {
        get => expireBlock; // Getter for expireBlock
        set => expireBlock = value; // Setter for expireBlock
    }

    public bool BlockingCanBeSplit
    {
        get => blockingCanBeSplit; // Getter for blockingCanBeSplit
        set => blockingCanBeSplit = value; // Setter for blockingCanBeSplit
    }
}