using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementDTO
{
    [SerializeField] private int id;
    [SerializeField] private List<AchievementMilestoneDTO> milestoneDTOs;
    [SerializeField] private int progress;
    [SerializeField] private int currentMilestone;
    [SerializeField] private int currentRewardIndex;
    [SerializeField] private bool isCompleted;

    public AchievementDTO(int id)
    {
        this.id = id;
        milestoneDTOs = new List<AchievementMilestoneDTO>();
        for (int i = 0; i < 3; ++i) milestoneDTOs.Add(new AchievementMilestoneDTO());
        progress = 0;
        currentMilestone = 0;
        currentRewardIndex = 0;
        isCompleted = false;
    }

    public int Id { get { return id; } }
    public List<AchievementMilestoneDTO> MilestoneDTOs { get { return milestoneDTOs; } }
    public int Progress { get { return progress; } }
    public bool IsCompleted { get { return isCompleted; } }

    public int CurrentMilestone { get { return currentMilestone; } }

    public int CurrentRewardIndex { get { return currentRewardIndex; } }

    public void GetData(Achievement aso)
    {
        progress = aso.Progress;
        isCompleted = aso.IsCompleted;
        currentMilestone = aso.CurrentMilestone;
        currentRewardIndex = aso.CurrentRewardIndex;
        for (int i = 0; i < aso.Milestones.Count; ++i) milestoneDTOs[i].GetData(aso.Milestones[i]);
    }
}

[Serializable]
public class AchievementMilestoneDTO
{
    [SerializeField] private bool isClaimed;
    [SerializeField] private bool isUnlocked;

    public bool IsClaimed { get { return isClaimed; } }
    public bool IsUnlocked { get { return isUnlocked; } }

    public AchievementMilestoneDTO()
    {
        isClaimed = false;
        isUnlocked = false;
    }

    public void GetData(AchievementMilestone am)
    {
        isClaimed = am.IsClaimed;
        isUnlocked = am.IsUnlocked;
    }
}
