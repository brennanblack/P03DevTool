using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceSystem : MonoBehaviour
{
    public int level;
    public float currentExp;
    public float requiredExp;

    private float lerpTimer;
    private float delayTimer;

    [Header("UI Images")]
    public Image frontExpBar;
    public Image backExpBar;

    [Header("Text")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;

    [Header("Experience Scaling Multipliers")]
    [Range(1f, 300f)]
    public float additionMultipler = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
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
