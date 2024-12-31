using UnityEngine.Events;

public class DailyMissionEvent
{
    private readonly static UnityEvent<DailyMissionType, int> _onUpdateMissionData = new UnityEvent<DailyMissionType, int>();
    private readonly static UnityEvent<int> _onClaimMissionReward = new UnityEvent<int>();
    private readonly static UnityEvent _onClaimDailyMissionReward = new UnityEvent();

    public static UnityEvent<DailyMissionType, int> OnUpdateMissionData { get => _onUpdateMissionData; }
    public static UnityEvent<int> OnClaimMissionReward { get => _onClaimMissionReward; }
    public static UnityEvent OnClaimDailyMissionReward { get => _onClaimDailyMissionReward; }
}
