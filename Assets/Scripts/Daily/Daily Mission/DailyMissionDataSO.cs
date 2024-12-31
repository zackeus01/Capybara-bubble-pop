using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyMission_", menuName = "Bubble Shooter/DailyMissionSO")]
public class DailyMissionDataSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private string vietnameseDescription;
    [SerializeField] private int goal;
    [SerializeField] private int progress;
    [SerializeField] private DailyMissionType type;
    [SerializeField] private List<RewardSO> rewards;
    [SerializeField] private bool isCompleted;
    [SerializeField] private bool isClaimed;

    public int Id { get { return id; } private set { id = value; } }
    public string _vietnameseDes { get { return vietnameseDescription; } private set { vietnameseDescription = value; } }
    public string Description { get { return description; } private set { description = value; } }
    public int Goal { get { return goal; } private set { goal = value; } }
    public int Progress { get { return progress; } private set { progress = value; } }
    public List<RewardSO> Reward { get { return rewards; } private set { rewards = value; } }
    public bool IsCompleted { get { return isCompleted; } private set { isCompleted = value; } }
    public bool IsClaimed { get { return isClaimed; } private set { isClaimed = value; } }
    public DailyMissionType Type => type;

    public void GetData(DailyMissionDTO dto)
    {
        progress = dto.ProgressAmount;
        isClaimed = dto.IsClaimed;
        isCompleted = dto.IsCompleted;
    }

    public void AddProgress(int amount)
    {
        if (isCompleted) return;

        progress += amount;
        if (progress >= goal)
        {
            isCompleted = true;
        }
    }

    public void ClaimMissionReward()
    {
        if (isCompleted)
        {
            isClaimed = true;
            rewards.ForEach(reward => reward?.ClaimReward());
        }
        else Debug.LogError("<color = red>Reward is not Completed</color>");
    }

}
