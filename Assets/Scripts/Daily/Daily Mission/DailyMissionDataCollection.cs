using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyMissionDataCollection
{
    [SerializeField] private List<DailyMissionDTO> dailyMissionDTOs;
    [SerializeField] private bool isRewardClaimed;

    public List<DailyMissionDTO> DailyMissionDTOs => dailyMissionDTOs;

    public DailyMissionDataCollection()
    {
        dailyMissionDTOs = new List<DailyMissionDTO>();
    }

    public bool IsRewardClaimed { get { return isRewardClaimed; } }

    public DailyMissionDTO GetDailyMissionDTO(int id)
    {
        return dailyMissionDTOs.Find(dto => dto.Id.Equals(id));
    }

    public void AddDailyMissionData(int id)
    {
        if (dailyMissionDTOs.Exists(dd => dd.Id.Equals(id))) return;

        dailyMissionDTOs.Add(new DailyMissionDTO(id));
    }

    public void UpdateMissionData(DailyMissionDataSO dmd)
    {
        if (!dailyMissionDTOs.Exists(dto => dto.Id.Equals(dmd.Id))) return;

        dailyMissionDTOs.Find(dto => dto.Id.Equals(dmd.Id)).GetData(dmd);
    }

    public void ClaimDailyMissionReward()
    {
        isRewardClaimed = true;
    }

    public void ResetMission()
    {
        isRewardClaimed = false;
        dailyMissionDTOs.ForEach(data => data.Reset());
    }
}

[Serializable]
public class DailyMissionDTO
{
    [SerializeField] private int id;
    [SerializeField] private int progressAmount;
    [SerializeField] private bool isClaimed;
    [SerializeField] private bool isCompleted;

    public int Id { get { return id; } }
    public int ProgressAmount { get { return progressAmount; } set { progressAmount = value; } }
    public bool IsClaimed { get { return isClaimed; } set { isClaimed = value; } }
    public bool IsCompleted { get { return isCompleted; } set { isCompleted = value; } }

    public DailyMissionDTO(int id)
    {
        this.id = id;
        this.progressAmount = 0;
        this.isClaimed = false;
        this.isCompleted = false;
    }

    public DailyMissionDTO(int id, int progressAmount, bool isClaimed, bool isCompleted)
    {
        this.id = id;
        this.progressAmount = progressAmount;
        this.isClaimed = isClaimed;
        this.isCompleted = isCompleted;
    }

    public void GetData(DailyMissionDataSO dmd)
    {
        this.progressAmount = dmd.Progress;
        this.isClaimed = dmd.IsClaimed;
        this.isCompleted = dmd.IsCompleted;
    }
    public void Reset()
    {
        progressAmount = 0;
        isClaimed = false;
        isCompleted = false;
    }
}
