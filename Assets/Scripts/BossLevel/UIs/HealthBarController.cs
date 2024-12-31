using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;
    [SerializeField] Slider damagedEaseBar;
    [SerializeField] Slider healedEaseBar;
    private float lerpSpeed = 0.05f;
    private bool isDamaged = true;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    private void Update()
    {
        if (healthBar.value != damagedEaseBar.value && isDamaged)
        {
            damagedEaseBar.value = Mathf.Lerp(damagedEaseBar.value, healthBar.value, lerpSpeed);
        }
        if (healthBar.value != healedEaseBar.value && !isDamaged)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, healedEaseBar.value, lerpSpeed);
        }
    }

    public void SetupHealthBar(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        damagedEaseBar.maxValue = maxHealth;
        healedEaseBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        damagedEaseBar.value = maxHealth;
        healedEaseBar.value = maxHealth;
    }

    public void UpdateHealthBar(float currentHp)
    {
        if (healthBar.value > currentHp) //Damaged
        {
            isDamaged = true;
            healthBar.value = Mathf.Clamp(currentHp, 0f, healthBar.maxValue);
            healedEaseBar.value = healthBar.value;
        }
        else //Healed
        {
            isDamaged = false;
            healedEaseBar.value = Mathf.Clamp(currentHp, 0f, healedEaseBar.maxValue);
            damagedEaseBar.value = healedEaseBar.value;
        }
    }
}
