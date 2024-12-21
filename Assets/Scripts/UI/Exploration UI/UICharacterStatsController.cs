using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterStatsController : MonoBehaviour
{
    private GameManager gameManager; // Reference to the GameManager

    [Header("Header Panel")]
    [SerializeField] private TextMeshProUGUI unitName; // Text for the unit's name
    [SerializeField] private TextMeshProUGUI level; // Text for the unit's level
    [SerializeField] private TextMeshProUGUI currentExp; // Text for the current experience
    [SerializeField] private TextMeshProUGUI nextLvlExp; // Text for the experience required for the next level
    [SerializeField] private TextMeshProUGUI currentHp; // Text for the current HP
    [SerializeField] private TextMeshProUGUI maxHp; // Text for the maximum HP
    [SerializeField] private Slider hpSlider; // Slider to display the HP

    [Header("Stat Panel")]
    [SerializeField] private TextMeshProUGUI strength; // Text for strength
    [SerializeField] private TextMeshProUGUI agility; // Text for agility
    [SerializeField] private TextMeshProUGUI intellect; // Text for intellect
    [SerializeField] private TextMeshProUGUI attackPower; // Text for attack power
    [SerializeField] private TextMeshProUGUI abilityPower; // Text for ability power
    [SerializeField] private TextMeshProUGUI physCritChance; // Text for physical crit chance
    [SerializeField] private TextMeshProUGUI magicCritChance; // Text for magical crit chance
    [SerializeField] private TextMeshProUGUI physicalDefense; // Text for physical defense
    [SerializeField] private TextMeshProUGUI magicalDefense; // Text for magical defense
    [SerializeField] private TextMeshProUGUI blockPower; // Text for block power
    [SerializeField] private TextMeshProUGUI dodgeChance; // Text for dodge chance
    [SerializeField] private TextMeshProUGUI speed; // Text for speed

    [Header("Level Up UI")]
    [SerializeField] private GameObject levelUpText; // Text indicating a level-up
    [SerializeField] private TextMeshProUGUI statPoints; // Text for remaining stat points
    [SerializeField] private GameObject strengthUp; // Button to increase strength
    [SerializeField] private GameObject agilityUp; // Button to increase agility
    [SerializeField] private GameObject intellectUp; // Button to increase intellect

    [SerializeField] private CombatUnit unit; // Reference to the CombatUnit

    // Coroutine to initialize the GameManager and update stats after the first frame
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        gameManager = FindObjectOfType<GameManager>();
        UpdateStats();
        ToggleLevelUpButtons(false);
    }

    // Update the stats and toggle level-up buttons every frame
    private void Update()
    {
        UpdateStats();

        // If there are no remaining stat points, hide the level-up buttons
        if (gameManager.PlayerData.remainingStatPoints <= 0)
        {
            ToggleLevelUpButtons(false);
            return;
        }

        // Otherwise, show the level-up buttons
        ToggleLevelUpButtons(true);
    }

    // Updates the UI with the current player stats
    public void UpdateStats()
    {
        // Exit if the component is not active or enabled
        if (!this.isActiveAndEnabled) return;

        // Find the GameManager if it's not already set
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();

        // Get the player data and unit base from the GameManager
        PlayerData playerData = gameManager.PlayerData;
        UnitBase unitBase = playerData.unitBase;

        // Initialize the CombatUnit if it's not already set
        if (unit == null)
        {
            unit = this.AddComponent<CombatUnit>(); // TODO: Get this from GameManager instead of adding a new component
        }

        // Initialize the unit with the unit base and level
        unit.InitiateUnit(unitBase, playerData.level);

        // Update the header panel
        unitName.text = unit.UnitName;
        level.text = "Lv" + playerData.level;
        currentExp.text = playerData.exp.ToString();
        nextLvlExp.text = playerData.nextLvLExp.ToString();
        currentHp.text = playerData.currentHp.ToString();
        maxHp.text = unit.MaxHp.ToString();
        hpSlider.minValue = 0;
        hpSlider.maxValue = unit.MaxHp;
        hpSlider.value = playerData.currentHp;

        // Update the stat panel
        statPoints.text = playerData.remainingStatPoints.ToString();

        strength.text = unit.Strength.ToString();
        agility.text = unit.Agility.ToString();
        intellect.text = unit.Intellect.ToString();
        attackPower.text = unit.AttackPower.ToString();
        abilityPower.text = unit.AbilityPower.ToString();
        physCritChance.text = Math.Round(unit.PhysicalCritChance * 100) + "%";
        magicCritChance.text = Math.Round(unit.MagicalCritChance * 100) + "%";
        physicalDefense.text = unit.PhysicalDefense.ToString();
        magicalDefense.text = unit.MagicalDefense.ToString();
        blockPower.text = unit.PhysicalBlockPower.ToString();
        dodgeChance.text = Math.Round(unit.DodgeChance * 100) + "%";
        speed.text = unit.Speed.ToString();
    }

    // Toggles the visibility of the level-up buttons
    private void ToggleLevelUpButtons(bool isActive)
    {
        levelUpText.SetActive(isActive);
        statPoints.gameObject.SetActive(isActive);
        strengthUp.SetActive(isActive);
        agilityUp.SetActive(isActive);
        intellectUp.SetActive(isActive);
    }

    // Adds a stat point to the specified stat type
    public void AddStatPoint(int typeIndex)
    {
        gameManager.AddStatPoint((StatType)typeIndex);
    }
}