using UnityEngine.Events;

public class AchievementEvent
{
    private readonly static UnityEvent<AchievementType, int> _onUpdateAchievement = new UnityEvent<AchievementType, int>();
    private readonly static UnityEvent<int> _onClaimAchievementReward = new UnityEvent<int>();
    private readonly static UnityEvent<AchievementType, int> _onChangeAchievementData = new UnityEvent<AchievementType, int>();

    public static UnityEvent<AchievementType, int> OnUpdateAchievement { get => _onUpdateAchievement; }
    public static UnityEvent<int> OnClaimAchievementReward { get => _onClaimAchievementReward; }
    public static UnityEvent<AchievementType, int> OnChangeAchievementData { get => _onChangeAchievementData; }
}
