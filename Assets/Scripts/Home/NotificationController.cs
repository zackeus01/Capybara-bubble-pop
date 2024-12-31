using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    private const string DailyTimer = "DailyLastTimeBuy";
    private const string WeeklyTimer = "WeeklyLastTimeBuy";
    private const string PopUpTimer = "PopUpTime";
    [SerializeField] private GameObject Border;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject IconDaily;
    [SerializeField] private GameObject IconWeekly;
    private void Start()
    {
        checkpop();
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
    public void Confirmation()
    {
        PlayerPrefs.SetString(PopUpTimer, DateTime.Now.ToString());
    }
    public void CallPopUpOpenAndEnableSameIcon()
    {
        if (CheckPopUp() && PlayerPrefs.HasKey("DoneTutorial"))
        {
            if (CheckDaily() && !CheckWeekly())
            {
                BG.SetActive(true);
                Border.SetActive(true);
                IconDaily.SetActive(true);
            }
            if (!CheckDaily() && CheckWeekly())
            {
                BG.SetActive(true);
                Border.SetActive(true);
                IconWeekly.SetActive(true);
            }
            if (CheckDaily() && CheckWeekly())
            {
                BG.SetActive(true);
                Border.SetActive(true);
                IconDaily.SetActive(true);
                IconWeekly.SetActive(true);
            }
        }
    }
    public bool CheckPopUp()
    {
        if (PlayerPrefs.HasKey(PopUpTimer))
        {
            string lastSpinTimeString = PlayerPrefs.GetString(PopUpTimer);
            DateTime lastSpinTime = DateTime.Parse(lastSpinTimeString);
            if (DateTime.Now >= lastSpinTime.AddHours(6))
            {
                return true;
            }
            return false;
        }
        return true;
    }
    private void checkpop()
    {
        int currentLevel = PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 0);
        if (currentLevel != 10 )
        {
            CallPopUpOpenAndEnableSameIcon();
        }
       
    }
}
