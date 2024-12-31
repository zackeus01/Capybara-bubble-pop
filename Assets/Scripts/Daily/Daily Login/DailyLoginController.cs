using System.Collections.Generic;
using UnityEngine;

public class DailyLoginController : MonoBehaviour
{
    [SerializeField] private List<DailyButtonUI> dailyButtons;
    [SerializeField] private MenuManager menuManager;

    private void OnEnable()
    {
        InitDailyLoginData();
    }

    private void InitDailyLoginData()
    {
        int currentDay = DailyLoginDataController.Instance.DailyLoginDataCollection.CurrentDay - 1;
        //Debug.Log(currentDay);

        for (int i = 0; i < 7; ++i)
        {
            //Debug.Log($"{menuManager.DailyLoginDataCollection.DailyLoginDataList[i].Id} {menuManager.DailyLoginDataCollection.DailyLoginDataList[i].IsClaimed} {menuManager.DailyLoginDataCollection.DailyLoginDataList[i].IsUnlocked}");

            dailyButtons[i].SetupButton(DailyLoginDataController.Instance.DailyLoginDataCollection.DailyLoginDataList[i].Id,
                DailyLoginDataController.Instance.DailyLoginDataCollection.DailyLoginDataList[i].IsClaimed);

            dailyButtons[currentDay]
                .SetHighlight(DailyLoginDataController.Instance.DailyLoginDataCollection.DailyLoginDataList[currentDay].IsClaimed);
        }
    }
}
