using UnityEngine;

public class DailyLoginDataController : Singleton<DailyLoginDataController>
{
    [Header("Daily Login")]
    [SerializeField] private DailyLoginDataCollection dailyLoginCollection;
    public DailyLoginDataCollection DailyLoginDataCollection { get { return dailyLoginCollection; } }

    private void OnEnable()
    {
        InitDailyLoginData();
        InitEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void InitDailyLoginData()
    {
        LoadData();
        SaveData();
    }

    #region Events
    private void InitEvent()
    {
        UIEvent.OnClaimDailyReward.AddListener(OnClaimDailyReward);
        UIEvent.OnNewDay.AddListener(OnNewDay);
    }
    private void RemoveEvent()
    {
        UIEvent.OnClaimDailyReward.RemoveListener(OnClaimDailyReward);
        UIEvent.OnNewDay.AddListener(OnNewDay);
    }
    #endregion

    #region Save & Load
    private void LoadData()
    {
        SaveSystem.LoadData(GameConst.DAILY_LOGIN_FILE, ref dailyLoginCollection);
    }

    private void SaveData()
    {
        SaveSystem.SaveData(GameConst.DAILY_LOGIN_FILE, dailyLoginCollection);
    }

    #endregion

    private void OnNewDay()
    {
        dailyLoginCollection.NewDay();
        SaveData();
    }
    private void OnClaimDailyReward(int day)
    {
        dailyLoginCollection.ClaimReward(day);
        SaveData();
    }
}
