using EasyUI.PickerWheelUI;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyLoginDataCollection
{
    public List<DailyLoginData> DailyLoginDataList;
    [SerializeField] private int currentDay;

    public int CurrentDay { get { return currentDay; } private set { currentDay = value; } }

    public DailyLoginDataCollection()
    {
        DailyLoginDataList = new List<DailyLoginData>();
        for (int i = 1; i <= 7; ++i) DailyLoginDataList.Add(new DailyLoginData(i));

        currentDay = 0;
    }

    public void NewDay()
    {
        //Debug.Log(currentDay);

        ++currentDay;
        if (currentDay > 7)
        {
            currentDay = 1;
            foreach (DailyLoginData dld in DailyLoginDataList)
            {
                dld.ResetDailyLogin();
            }
        }
        //Debug.Log(currentDay);
        DailyLoginDataList[currentDay - 1].UnlockDailyLogin();
    }

    public void ClaimReward(int day)
    {
        Debug.Log(day);
        DailyLoginDataList[day - 1].ClaimDailyLogin();
    }
}
