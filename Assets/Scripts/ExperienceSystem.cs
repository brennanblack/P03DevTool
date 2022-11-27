using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        frontExpBar.fillAmount = currentExp / requiredExp;
        backExpBar.fillAmount = currentExp / requiredExp;
    }

    void Update()
    {
        UpdateExperienceUI();
        if (Input.GetKeyDown(KeyCode.Equals))
            GainExperienceFlatRate(20);
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
    }
    public void GainExperienceFlatRate(float expGained)
    {
        currentExp += expGained;
        lerpTimer = 0f;
    }
}
