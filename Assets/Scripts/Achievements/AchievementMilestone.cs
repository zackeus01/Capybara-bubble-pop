using System;
using UnityEngine;

[Serializable]
public class AchievementMilestone
{
    [SerializeField] private int goal;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isClaimed;
    [SerializeField] private RewardSO reward;

    public int Goal { get { return goal; } private set { goal = value; } }
    public bool IsUnlocked { get { return isUnlocked; } }
    public bool IsClaimed { get { return isClaimed; } }

    public Sprite RewardSprite { get { return reward.RewardSprite; } }
    public int RewardAmount { get { return reward.RewardAmount; } }

    public void Reset()
    {
        isUnlocked = false;
        isClaimed = false;
    }

    public AchievementMilestone(int goal)
    {
        this.goal = goal;
        this.isUnlocked = false;
        this.isClaimed = false;
    }

    public void GetData(AchievementMilestoneDTO dto)
    {
        this.isClaimed = dto.IsClaimed;
        this.isUnlocked = dto.IsUnlocked;
    }

    public void UnlockMilestone()
    {
        isUnlocked = true;
    }

    public void ClaimReward()
    {
        isClaimed = true;
        reward.ClaimReward();
    }
}
