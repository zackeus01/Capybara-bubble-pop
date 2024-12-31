using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    private int id;
    private string description;
    private string vietnameseDes;
    private AchievementType type;
    private List<AchievementMilestone> milestones;
    private int progress;
    private int currentMilestone;
    private int currentRewardIndex;
    private bool isCompleted;

    [Header("UI")]
    private Sprite achievementSprite;

    public int Id { get { return id; } private set { id = value; } }
    public string Description { get { return description; } private set { description = value; } }
    public string _vietnameseDes { get { return vietnameseDes; } private set { vietnameseDes = value; } }
    public int Progress { get { return progress; } private set { progress = value; } }
    public bool IsCompleted { get { return isCompleted; } private set { isCompleted = value; } }
    public int CurrentMilestone { get { return currentMilestone; } private set { currentMilestone = value; } }
    public int CurrentRewardIndex { get { return currentRewardIndex; } private set { currentRewardIndex = value; } }
    public Sprite AchievementSprite { get { return achievementSprite; } }
    public Sprite RewardSprite { get { return milestones[currentRewardIndex].RewardSprite; } }
    public int RewardAmount { get { return milestones[currentRewardIndex].RewardAmount; } }
    public int AchievementCap { get { return milestones[currentMilestone].Goal; } }
    public List<AchievementMilestone> Milestones { get { return milestones; } private set { milestones = value; } }
    public AchievementType Type => type;

    public Achievement(AchievementSO so)
    {
        id = so.Id;
        description = so.Description;
        vietnameseDes = so._VIDescription;
        type = so.Type;
        milestones = so.Milestones;
        progress = so.Progress;
        currentMilestone = so.CurrentMilestone;
        currentRewardIndex = so.CurrentRewardIndex;
        IsCompleted = so.IsCompleted;
        achievementSprite = so.AchievementSprite;
    }

    public void GetData(AchievementDTO dto)
    {
        this.progress = dto.Progress;
        currentMilestone = dto.CurrentMilestone;
        currentRewardIndex = dto.CurrentRewardIndex;
        for (int i = 0; i < milestones.Count; ++i)
        {
            milestones[i].GetData(dto.MilestoneDTOs[i]);
        }
        isCompleted = dto.IsCompleted;

        //Debug.Log($"{Id} {progress} {currentMilestone} {currentRewardIndex} {isCompleted}");
    }
    public void AddProgress(int amount)
    {


        if (isCompleted) return;

        Debug.Log($"Add progress {amount} for {description}");

        //Debug.Log($"{Description} {progress} {currentMilestone}");

        progress += amount;

        if (progress < milestones[currentMilestone].Goal) return;

        Debug.Log($"Unlock Achievement {id} {description}");
        milestones[currentMilestone].UnlockMilestone();
        ++currentMilestone;

        if (currentMilestone >= milestones.Count)
        {
            isCompleted = true;
            --currentMilestone;
            if (progress > milestones[currentMilestone].Goal)
                progress = milestones[currentMilestone].Goal;
        }
    }

    public void ChangeData(int amount)
    {
        if (isCompleted) return;

        //Debug.Log($"{Description} {progress} {currentMilestone}");

        progress = amount;

        while (progress >= milestones[currentMilestone].Goal)
        {
            if (progress == milestones[currentMilestone].Goal) break;

            milestones[currentMilestone].UnlockMilestone();
            ++currentMilestone;

            if (currentMilestone >= milestones.Count)
            {
                isCompleted = true;
                --currentMilestone;

                if (progress > milestones[currentMilestone].Goal)
                    progress = milestones[currentMilestone].Goal;
            }
        }
    }

    public void ClaimAchievementReward()
    {
        if (!milestones[currentRewardIndex].IsUnlocked)
        {
            Debug.Log("Current Milestone Reward is not unlocked");
            return;
        }

        Debug.Log($"{id} claim rewards");

        milestones[currentRewardIndex].ClaimReward();
        ++currentRewardIndex;

        if (currentRewardIndex >= milestones.Count)
            --currentRewardIndex;
    }
    public bool CanClaimReward()
    {
        //Debug.Log(CurrentRewardIndex);

        return (progress >= milestones[currentRewardIndex].Goal && !milestones[currentRewardIndex].IsClaimed);
    }
}
