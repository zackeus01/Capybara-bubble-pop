using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Lv;
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI Def;
    [SerializeField] private TextMeshProUGUI Res;
    [SerializeField] private TextMeshProUGUI Atk;
    [SerializeField] private TextMeshProUGUI Crit;
    [SerializeField] private TextMeshProUGUI ElDame;
    [SerializeField] private TextMeshProUGUI Mana;
    [SerializeField] private TextMeshProUGUI CoinUplv;
    [SerializeField] private PlayerBaseStatus baseStatus;
    [SerializeField] private PlayerUpgradeMultiplier upgradeMultiplier;
   

    private int currentLv;
    private float currentHp;
    private float currentDef;
    private float currentRes;
    private float currentAtk;
    private float currentCrit;
    private float currentElDame;
    private float currentMana;
    private int currentCoin = 100;

    private bool isEnoughCoin = false;
    private Color coinOriginalColor;

    private void Start()
    {
        UIEvent.OnCurrencyChanged.AddListener(CheckEnoughCoin);
        coinOriginalColor = CoinUplv.color;

        displayStatus();
        CheckEnoughCoin();
        this.gameObject.SetActive(false);
    }

    private void CheckEnoughCoin()
    {
        isEnoughCoin = (BankSystem.Instance.Coin - currentCoin) >= 0;
        if (!isEnoughCoin)
        {
            CoinUplv.color = Color.red;
        }
        else
        {
            CoinUplv.color = coinOriginalColor;
        }
    }

    private void displayStatus()
    {
        if (!PlayerPrefs.HasKey("currentLvPlayer"))
        {
            PlayerPrefs.SetInt("currentLvPlayer", 1);
        }
        currentLv = Mathf.Max(PlayerPrefs.GetInt("currentLvPlayer"), 1);

        if (currentLv == 1)
        {
            Lv.text = PlayerPrefs.GetInt("currentLvPlayer").ToString() + "/" + baseStatus.MaxLv;
            currentHp = baseStatus.BaseHp;
            currentHp = baseStatus.BaseDef;
            currentHp = baseStatus.BaseRes;
            currentHp = baseStatus.BaseAtk;
            currentHp = baseStatus.BaseCrit;
            currentHp = baseStatus.BaseElDame;
            currentHp = baseStatus.BaseMana;
            currentHp = baseStatus.BaseCoin;
        }
        else
        {
            UpdatePlayerStats();
        }

        StatUpdate(currentLv);
        SaveStatsToData();
    }

    private void SaveStatsToData()
    {
        PlayerSavecontroller.Instance.SaveDataPlayer(currentLv, currentHp, currentAtk, currentDef, currentElDame, currentRes, currentCrit, currentMana);
    }

    public void LevelUp()
    {
        currentLv = PlayerPrefs.GetInt("currentLvPlayer");
        if (isEnoughCoin && currentLv < baseStatus.MaxLv)
        {
            BankSystem.Instance.WithdrawCoin(currentCoin);
            currentLv++;
            PlayerPrefs.SetInt("currentLvPlayer", currentLv);
            PlayerPrefs.Save();
            StatUpdate(currentLv);
        }
        else
        {
            Debug.Log("khong du tien");
            CoinUplv.color = Color.red;
        }
        CheckEnoughCoin();
    }

    private void StatUpdate(float currentLv)
    {
        UpdatePlayerStats();
        UpdateStatTexts();
        SaveStatsToData();
    }

    private void UpdatePlayerStats()
    {
        currentHp = baseStatus.BaseHp;
        currentDef = baseStatus.BaseDef;
        currentRes = baseStatus.BaseRes;
        currentAtk = baseStatus.BaseAtk;
        currentCoin = baseStatus.BaseCoin;
        currentCrit = baseStatus.BaseCrit;
        currentMana = baseStatus.BaseMana;
        currentElDame = baseStatus.BaseElDame;

        if (currentLv > 1)
        {
            for (int i = 2; i <= currentLv; i++)
            {
                currentHp += i * upgradeMultiplier.HpMultiplier;
                currentAtk += i * upgradeMultiplier.AttackMultiplier;
                currentDef += i * upgradeMultiplier.DefenseMultiplier;
                currentElDame += i * upgradeMultiplier.EleAtkMultiplier;
                currentRes += i * upgradeMultiplier.EleResMultiplier;
                currentCoin += i  * upgradeMultiplier.CoinMultiplier;
                currentCrit += i * upgradeMultiplier.CritMultiplier;
            }
            currentMana += (currentLv - 1) * upgradeMultiplier.ManaMultiplier;
        }

        Lv.text = PlayerPrefs.GetInt("currentLvPlayer").ToString() + "/" + baseStatus.MaxLv;
    }

    private void UpdateStatTexts()
    {
        Hp.text = currentHp.ToString();
        Def.text = currentDef.ToString();
        Res.text = currentRes.ToString();
        Atk.text = currentAtk.ToString();
        Mana.text = currentMana.ToString();
        Crit.text = currentCrit.ToString();
        ElDame.text = currentElDame.ToString();
        CoinUplv.text = currentCoin.ToString();
    }

}
