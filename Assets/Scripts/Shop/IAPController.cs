using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IAPController : MonoBehaviour
{
    public List<IAPPack> iapPackObjects;
    public List<IAPPack> dailyPack;

    [Header("Start pack")]
    [SerializeField] public IAPPack IAPStart;

    [Header("Icon Pack")]
    [SerializeField] private GameObject dailyPackonScreen;
    [SerializeField] private GameObject weeklyPackonScreen;
    [SerializeField] private GameObject parentPopUpDaily;
    [SerializeField] private GameObject parentPopUpWeekly;
    private const string DailyTimer = "DailyLastTimeBuy";
    private const string WeeklyTimer = "WeeklyLastTimeBuy";
    public void Start()
    {
        ActivateRandomDailyPack();
        CheckPlayerHasBuyWeeklyOrDailyPack();
    }
    public void CheckPlayerHasBuyWeeklyOrDailyPack()
    {
        if (CheckDaily())
        {
            dailyPackonScreen.SetActive(true);
        }
        if (CheckWeekly())
        {
            weeklyPackonScreen.SetActive(true);
        }
    }
    public bool CheckDaily()
    {
        if (PlayerPrefs.HasKey(DailyTimer))
        {
            string lastSpinTimeString = PlayerPrefs.GetString(DailyTimer);
            DateTime lastSpinTime = DateTime.Parse(lastSpinTimeString);
            if (DateTime.Now >= lastSpinTime.AddHours(24))
            {
                return true;
            }
            return false;
        }
        return true;
    }
    public bool CheckWeekly()
    {
        if (PlayerPrefs.HasKey(WeeklyTimer))
        {
            string lastSpinTimeString = PlayerPrefs.GetString(WeeklyTimer);
            DateTime lastSpinTime = DateTime.Parse(lastSpinTimeString);
            if (DateTime.Now >= lastSpinTime.AddHours(168))
            {
                return true;
            }
            return false;
        }
        return true;
    }

    public void OnIAPPackClick(int index)
    {
        if (index >= 0 && index < iapPackObjects.Count)
        {
            IAPPack pack = iapPackObjects[index];
            
            IAPPurchase iapPurchase = gameObject.GetComponent<IAPPurchase>();
            if (iapPurchase != null)
            {
                Debug.Log("Buy Here");
                iapPurchase.UpdateIAPPopup(pack, index);
                iapPurchase.BuyItem();
                if (pack.name.StartsWith("D"))
                {
                    PlayerPrefs.SetString(DailyTimer, DateTime.Now.ToString());
                }
                if (pack.name.StartsWith("w"))
                {
                    PlayerPrefs.SetString(WeeklyTimer, DateTime.Now.ToString());
                }
                StartCoroutine(disableTabOnComplete(pack, 4.5f));
            }

        }
    }
    private IEnumerator disableTabOnComplete(IAPPack pack, float time)
    {
        yield return new WaitForSeconds(time);
        if (pack.name.StartsWith("D"))
        {
            dailyPackonScreen.SetActive(false);
            parentPopUpDaily.SetActive(false);
        }
        if (pack.name.StartsWith("w"))
        {
            weeklyPackonScreen.SetActive(false);
            parentPopUpWeekly.SetActive(false);
        }
    }
    private void ActivateRandomDailyPack()
    {
        if (dailyPack != null && dailyPack.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, dailyPack.Count); 
            dailyPack[randomIndex].gameObject.SetActive(true);  
        }
    }

}
