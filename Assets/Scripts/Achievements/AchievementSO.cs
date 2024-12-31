using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement ", menuName = "Bubble Shooter/AchievementSO")]
public class AchievementSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private string VIDescription;
    [SerializeField] private AchievementType type;
    [SerializeField] private List<AchievementMilestone> milestones;
    [SerializeField] private int progress;
    [SerializeField] private int currentMilestone;
    [SerializeField] private int currentRewardIndex;
    [SerializeField] private bool isCompleted;

    [Header("UI")]
    [SerializeField] private Sprite achivementSprite;

    public int Id { get { return id; } private set { id = value; } }
    public string Description { get { return description; } private set { description = value; } }
    public string _VIDescription { get { return VIDescription; } private set { VIDescription = value; } }
    public int Progress { get { return progress; } private set { progress = value; } }
    public bool IsCompleted { get { return isCompleted; } private set { isCompleted = value; } }
    public int CurrentMilestone { get { return currentMilestone; } private set { currentMilestone = value; } }
    public int CurrentRewardIndex { get { return currentRewardIndex; } private set { currentRewardIndex = value; } }
    public Sprite AchievementSprite { get { return achivementSprite; } }
    public Sprite RewardSprite { get { return milestones[currentMilestone].RewardSprite; } }
    public int RewardAmount { get { return milestones[currentMilestone].RewardAmount; } }
    public int AchievementCap { get { return milestones[currentMilestone].Goal; } }
    public List<AchievementMilestone> Milestones { get { return milestones; } private set { milestones = value; } }
    public AchievementType Type => type;
}