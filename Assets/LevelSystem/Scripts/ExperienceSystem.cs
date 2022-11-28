using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceSystem : MonoBehaviour
{
    [Header("- - - Starting Stats - - -")]
    [Tooltip("Starting level for the player")]
    public int level;
    [Tooltip("How much experience the player starts with")]
    public float currentExp;
    [Tooltip("How much experience the player needs to level up at the start. The code will automatically calculate the experience for the other levels")]
    public float requiredExp;

    private float lerpTimer;
    private float delayTimer;

    [Header("- - - UI Images - - -")]
    [Tooltip("The image that will be used to show how much experience a player has goes here")]
    public Image frontExpBar;
    [Tooltip("The image that will appear before the front experience goes here")]
    public Image backExpBar;

    [Header("- - - Text - - -")]
    [Tooltip("The text object that states what level you are at goes here")]
    public TextMeshProUGUI levelText;
    [Tooltip("The text object that states how much experience you have goes here")]
    public TextMeshProUGUI expText;

    [Header("- - - Experience Scaling Multipliers - - - ")]
    [Range(1f, 300f)]
    [Tooltip("The base value that will be added to the level then multiplied by. Increasing this will make you need more experience for each level")]
    public float additionMultipler = 300;
    [Range(2f, 4f)]
    [Tooltip("The exponent value that the equation will multiply by. Increasing this will make you need more experience for each level")]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    [Tooltip("How much the total experience will be divided by at the end of the equation. Increasing this will make the player need less experience for each level")]
    public float divisionMultiplier = 7;

    void Start()
    {
        frontExpBar.fillAmount = currentExp / requiredExp;
        backExpBar.fillAmount = currentExp / requiredExp;
        requiredExp = CalculateRequiredExp();
        levelText.text = "Level " + level;
    }

    void Update()
    {
        UpdateExperienceUI();
        //Manually Increase Experience
        if (Input.GetKeyDown(KeyCode.Equals))
            GainExperienceFlatRate(20);
        //If XP reaches max bar, level up
        if (currentExp > requiredExp)
            LevelUp();
    }

    public void UpdateExperienceUI()
    {
        float expFraction = currentExp / requiredExp;
        float frontExp = frontExpBar.fillAmount;

        if(frontExp < expFraction)
        {
            delayTimer += Time.deltaTime;
            backExpBar.fillAmount = expFraction;
            if(delayTimer > 1)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontExpBar.fillAmount = Mathf.Lerp(frontExp, backExpBar.fillAmount, percentComplete);
            }
        }
        expText.text = currentExp + "/" + requiredExp;
    }
    public void GainExperienceFlatRate(float expGained)
    {
        currentExp += expGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }
    private void LevelUp()
    {
        level++;
        frontExpBar.fillAmount = 0f;
        backExpBar.fillAmount = 0f;
        currentExp = Mathf.RoundToInt(currentExp - requiredExp);
        GetComponent<PlayerHealth>().IncreaseHealth(level);
        requiredExp = CalculateRequiredExp();
        levelText.text = "Level " + level;
    }

    private int CalculateRequiredExp()
    {
        //Runescape Experience Math Equation
        int solveForRequiredExp = 0;
        for(int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredExp += (int)Mathf.Floor(levelCycle + additionMultipler * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredExp / 4;
    }
}
