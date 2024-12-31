using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{
    private Slider manaBar;

    private void Awake()
    {
        manaBar = GetComponent<Slider>();
    }

    public void SetupManaBar(float manaCap)
    {
        manaBar.maxValue = manaCap;
        manaBar.value = 0;
    }

    public void UpdateManaBar(float currentMana)
    {
        manaBar.value = Mathf.Clamp(currentMana, 0f, manaBar.maxValue);
    }
}
