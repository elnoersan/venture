using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplay : MonoBehaviour
{
    [SerializeField] private GameObject activeEffectPrefab; // Prefab for displaying active effects
    [SerializeField] private GameObject activeEffectsContainer; // Container for active effects in the UI
    [SerializeField] private List<ActiveEffectSO> activeEffectTypes; // List of active effect types with their icons

    private CombatUnit connectedUnit; // The combat unit this UI is displaying stats for
    private Slider hpSlider; // Slider to display the unit's HP
    private TextMeshProUGUI[] infoText; // Array of TextMeshProUGUI components for displaying unit info
    private Image unitPortrait; // Image component for displaying the unit's portrait
    private List<CombatEffect> activeEffectsReferences; // List of active effects currently displayed in the UI
    private bool isConnectedUnitActive; // Flag to check if the connected unit is active

    // Initialize the UI components and set up the initial display
    private void Start()
    {
        // Get all TextMeshProUGUI components in children
        infoText = GetComponentsInChildren<TextMeshProUGUI>();

        // Set the unit's name and level in the UI
        infoText[0].text = ConnectedUnit.UnitName;
        infoText[1].text = "Lv" + ConnectedUnit.Level;

        // Set the unit's portrait in the UI
        GetComponentsInChildren<Image>()[0].sprite = connectedUnit.IdleSprite;

        // Initialize the HP slider
        hpSlider = GetComponentInChildren<Slider>();
        hpSlider.minValue = 0;

        // Initialize the list of active effects
        activeEffectsReferences = new List<CombatEffect>();
    }

    // Update the UI every frame
    void Update()
    {
        // Update the HP slider to reflect the unit's current HP
        hpSlider.maxValue = connectedUnit.MaxHp;
        hpSlider.value = connectedUnit.CurrentHp;

        // Update the active effects displayed in the UI
        UpdateActiveEffects();
    }

    // Property to get or set the connected combat unit
    public CombatUnit ConnectedUnit
    {
        get => connectedUnit;
        set => connectedUnit = value;
    }

    // Updates the active effects displayed in the UI
    public void UpdateActiveEffects()
    {
        List<GameObject> expiredObjects = new List<GameObject>(); // List of expired effect UI objects
        List<CombatEffect> expiredReferences = new List<CombatEffect>(); // List of expired effect references

        List<Transform> activeEffectTransforms = new List<Transform>(); // List of active effect transforms in the UI

        // Populate the list of active effect transforms
        foreach (Transform child in activeEffectsContainer.transform)
        {
            activeEffectTransforms.Add(child);
        }

        // Loop through all displayed effects in the UI for this unit
        activeEffectTransforms.ForEach(effectTransform =>
        {
            // Get the CombatEffect reference from the UI object
            CombatEffect activeEffectInUI = effectTransform.gameObject.GetComponent<ActiveEffectRef>().Reference;

            // Loop through all effects actually active on the unit
            activeEffectsReferences.ForEach(effectReferenceFromUnit =>
            {
                // If the effect displayed in the UI matches an active effect on the unit, update it
                if (effectReferenceFromUnit == activeEffectInUI)
                {
                    // Handle effects with a turn duration
                    if (effectReferenceFromUnit.HasTurnDuration)
                    {
                        if (effectReferenceFromUnit.DurationTracker.isEffectActive())
                        {
                            // Update the duration text in the UI
                            effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                                = (effectReferenceFromUnit.DurationTracker.GetRemainingDuration()).ToString();
                        }
                        else
                        {
                            // Mark the effect as expired if it's no longer active
                            expiredObjects.Add(effectTransform.gameObject);
                            expiredReferences.Add(effectReferenceFromUnit);
                        }
                    }
                    // Handle effects without a duration (e.g., Block, Mitigation)
                    else
                    {
                        // Update the UI for Block effects
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.Block)
                        {
                            if (connectedUnit.CurrentPhysicalBlock > 0)
                            {
                                // Display the current block value in yellow
                                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                                    = connectedUnit.CurrentPhysicalBlock.ToString();
                            }
                            else
                            {
                                // Mark the effect as expired if the block value is 0
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);
                            }
                        }
                        // Handle Physical Mitigation effects
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.PhysMitigation)
                        {
                            if (connectedUnit.CurrentPhysicalMitigation <= 0)
                            {
                                // Mark the effect as expired if the mitigation value is 0
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);
                            }
                        }
                        // Handle Magical Mitigation effects
                        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.MagicMitigation)
                        {
                            if (connectedUnit.CurrentMagicalMitigation <= 0)
                            {
                                // Mark the effect as expired if the mitigation value is 0
                                expiredObjects.Add(effectTransform.gameObject);
                                expiredReferences.Add(effectReferenceFromUnit);
                            }
                        }
                    }
                }
            });
        });

        // Remove expired effects from the UI and the reference list
        for (int i = expiredObjects.Count; i > 0; i--)
        {
            expiredReferences.RemoveAt(i - 1);
            Destroy(expiredObjects[i - 1]);
        }
    }

    // Updates the display for effects without a duration (e.g., Block, Mitigation)
    private void UpdateDisplayForEffectsWithoutDuration(CombatEffect effectReferenceFromUnit, Transform effectTransform)
    {
        // Handle Block effects
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.Block)
        {
            if (connectedUnit.CurrentPhysicalBlock > 0)
            {
                // Display the current block value in yellow
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentPhysicalBlock.ToString();
            }
        }
        // Handle Physical Mitigation effects
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.PhysMitigation)
        {
            if (connectedUnit.CurrentPhysicalMitigation > 0)
            {
                // Display the current mitigation value in yellow
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentPhysicalMitigation.ToString() + "%";
            }
        }
        // Handle Magical Mitigation effects
        if (effectReferenceFromUnit.CombatEffectType == CombatEffectType.MagicMitigation)
        {
            if (connectedUnit.CurrentMagicalMitigation > 0)
            {
                // Display the current mitigation value in yellow
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                effectTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = connectedUnit.CurrentMagicalMitigation.ToString() + "%";
            }
        }
    }

    // Adds a new active effect to the UI
    public void AddActiveEffect(CombatEffect effect)
    {
        // Instantiate the active effect prefab and set its parent
        GameObject activeEffectDisplay = Instantiate(activeEffectPrefab, activeEffectsContainer.transform);

        // Set the CombatEffect reference in the prefab
        activeEffectDisplay.GetComponent<ActiveEffectRef>().Reference = effect;

        // Set the icon for the active effect
        activeEffectDisplay.GetComponentInChildren<Image>().sprite = GetMatchingTypeSprite(effect);

        // Add the effect to the list of active effects
        activeEffectsReferences.Add(effect);
    }

    // Returns the sprite for the given CombatEffect type
    public Sprite GetMatchingTypeSprite(CombatEffect effect)
    {
        // Find the matching sprite for the effect type
        var matchingTypeSprite = activeEffectTypes.Find(effectType => effectType.Type == effect.CombatEffectType).Icon;
        return matchingTypeSprite;
    }
}