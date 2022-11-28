using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health = 100f;
    private float lerpTimer;

    [Header("- - -  Base Stats - - -")]
    [Tooltip("How much health the player starts with")]
    public float maxHealth = 100f;
    [Tooltip("How fast the health bars catch up to eachother when taking damage or gaining health back")]
    public float chipSpeed = 5f;

    [Header("- - - UI Images - - - ")]
    [Tooltip("The image that will be used to show how much health a player has goes here. This will be the main displayed health bar.")]
    public Image frontHealthBar;
    [Tooltip("The image that will be used to show how damage the player takes. This will be the health bar that chips away from the main health bar.")]
    public Image backHealthBar;

    [Header("- - - Text - - - ")]
    [Tooltip("The text object that states the players health goes here. Use this as a placeholder for where you want the amount of health the player has to be")]
    public TextMeshProUGUI healthText;
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        //Reduce Health
        if(fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.gray;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }
        //Restore Health
        if(fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
    public void IncreaseHealth(int level)
    {
        maxHealth += (health * 0.01f) * ((100 - level) * 0.1f);
        health = maxHealth;
    }
}
