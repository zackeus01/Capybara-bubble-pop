using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShieldBarController : MonoBehaviour
{
    private Slider shieldBar;
    [SerializeField] Slider easeBar;
    private float lerpSpeed = 0.05f;

    private void Awake()
    {
        shieldBar = GetComponent<Slider>();
    }

    private void Update()
    {
        if (shieldBar.value != easeBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, shieldBar.value, lerpSpeed);
        }
    }

    public void SetupShieldBar(float maxHealth)
    {
        shieldBar.maxValue = maxHealth;
        easeBar.maxValue = maxHealth;
        shieldBar.value = 0;
        easeBar.value = 0;
    }

    public void UpdateShieldBar(float currentHp)
    {
        shieldBar.value = Mathf.Clamp(currentHp, 0f, shieldBar.maxValue);
    }
}
