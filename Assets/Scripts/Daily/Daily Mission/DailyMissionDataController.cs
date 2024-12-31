using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailyMissionDataController : Singleton<DailyMissionDataController>
{
    private DailyMissionDataCollection dailyMissionDataCollection;
    [SerializeField] private List<DailyMissionDataSO> dailyMissionDatas;
    [SerializeField] private List<RewardSO> dailyMissionReward;
    public List<DailyMissionDataSO> Data { get { return dailyMissionDatas; } }
    public bool IsDailyMissionRewardClaimed { get { return dailyMissionDataCollection.IsRewardClaimed; } }
    public List<RewardSO> DailyMissionReward { get { return dailyMissionReward; } }

    private void OnEnable()
    {
        InitDailyMissionDataController();
        InitEvent();
    }

    private void OnDestroy()
    {
        RemoveEvents();
        SaveData();
    }

    private void InitDailyMissionDataController()
    {
        LoadData();

        foreach (DailyMissionDataSO dmd in dailyMissionDatas)
        {
            dailyMissionDataCollection.AddDailyMissionData(dmd.Id);
            dmd.GetData(dailyMissionDataCollection.GetDailyMissionDTO(dmd.Id));
        }

        SaveData();
    }

    private DailyMissionDataSO GetDailyMissionDataSO(int id)
    {
        return dailyMissionDatas.Find(dmd => dmd.Id.Equals(id));
    }

    private void UpdateMissionData(DailyMissionType type, int amount)
    {
        dailyMissionDatas
            .Where(dmd => dmd.Type.Equals(type))
            .ToList()
            .ForEach(dmd => UpdateMissionData(dmd.Id, amount));
    }

    private void UpdateMissionData(int id, int amount)
    {
        DailyMissionDataSO dmd = GetDailyMissionDataSO(id);

        if (dmd == null) return;

        dmd.AddProgress(amount);
        dailyMissionDataCollection.UpdateMissionData(dmd);

        SaveData();
    }

    private void ClaimMissionReward(int id)
    {
        Debug.Log($"Claim Mission Id {id}");
        DailyMissionDataSO dmd = GetDailyMissionDataSO(id);

        if (dmd == null) return;

        GetDailyMissionDataSO(id).ClaimMissionReward();
        dailyMissionDataCollection.UpdateMissionData(GetDailyMissionDataSO(id));

        Debug.Log(GetDailyMissionDataSO(id).IsClaimed);

        SaveData();
    }

    private void ClaimDailyMissionReward()
    {
        dailyMissionDataCollection.ClaimDailyMissionReward();
        foreach (var rw in dailyMissionReward)
            rw.ClaimReward();
        SaveData();
    }

    private void OnNewDay()
    {
        dailyMissionDataCollection.ResetMission();
        dailyMissionDataCollection.DailyMissionDTOs.ForEach(data => dailyMissionDatas.Find(data2 => data2.Id.Equals(data.Id)).GetData(data));
        SaveData();
    }

    #region Events

    //TODO: Init Events
    private void InitEvent()
    {
        DailyMissionEvent.OnUpdateMissionData.AddListener(UpdateMissionData);
        DailyMissionEvent.OnClaimMissionReward.AddListener(ClaimMissionReward);
        DailyMissionEvent.OnClaimDailyMissionReward.AddListener(ClaimDailyMissionReward);
        UIEvent.OnNewDay.AddListener(OnNewDay);
    }

    //TODO: Remove all events when disable
    private void RemoveEvents()
    {
        DailyMissionEvent.OnUpdateMissionData.RemoveListener(UpdateMissionData);
        DailyMissionEvent.OnClaimMissionReward.RemoveListener(ClaimMissionReward);
        DailyMissionEvent.OnClaimDailyMissionReward.RemoveListener(ClaimDailyMissionReward);
        UIEvent.OnNewDay.RemoveListener(OnNewDay);
    }

    #endregion

    #region Save & Load

    private void LoadData()
    {
        SaveSystem.LoadData(GameConst.DAILY_MISSION_FILE, ref dailyMissionDataCollection);
    }

    private void SaveData()
    {
        SaveSystem.SaveData(GameConst.DAILY_MISSION_FILE, dailyMissionDataCollection);
    }

    #endregion
}
