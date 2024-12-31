using UnityEngine.Events;

public class UIEvent
{
    private readonly static UnityEvent _onClickLevelStart = new UnityEvent();
    private readonly static UnityEvent _onClickPause = new UnityEvent();
    private readonly static UnityEvent _onClickResume = new UnityEvent();
    private readonly static UnityEvent _onClickRestart = new UnityEvent();
    private readonly static UnityEvent _onClickReturnToMainMenu = new UnityEvent();
    private readonly static UnityEvent _onCurrencyChanged = new UnityEvent();
    private readonly static UnityEvent<int> _onClaimDailyReward = new UnityEvent<int>();
    private readonly static UnityEvent<int> _onChangeMenuTab = new UnityEvent<int>();
    private readonly static UnityEvent _onNewDay = new UnityEvent();
    private readonly static UnityEvent<string> _onUpdateEquipment = new UnityEvent<string>();
    private readonly static UnityEvent<string> _onOpenEquipmentPopup = new UnityEvent<string>();
    private readonly static UnityEvent<string, EquipmentType> _onEquipedEquipment = new UnityEvent<string, EquipmentType>();
    private readonly static UnityEvent<string, int> _onUpgradeEquipment = new UnityEvent<string,int>();
    private readonly static UnityEvent _onUpdatePlayerMirroring = new UnityEvent();

    public static UnityEvent OnClickLevelStart { get => _onClickLevelStart; }
    public static UnityEvent OnClickPause { get => _onClickPause; }
    public static UnityEvent OnClickResume { get => _onClickResume; }
    public static UnityEvent OnClickRestart { get => _onClickRestart; }
    public static UnityEvent OnClickReturnToMainMenu { get => _onClickReturnToMainMenu; }
    public static UnityEvent OnCurrencyChanged { get => _onCurrencyChanged; }
    public static UnityEvent<int> OnClaimDailyReward { get => _onClaimDailyReward; }
    public static UnityEvent<int> OnChangeMenuTab { get => _onChangeMenuTab; }
    public static UnityEvent OnNewDay { get => _onNewDay; }
    public static UnityEvent<string> OnUpdateEquipment { get => _onUpdateEquipment; }
    public static UnityEvent<string> OnOpenEquipmentPopup { get => _onOpenEquipmentPopup; }
    public static UnityEvent<string, EquipmentType> OnEquipedEquipment { get => _onEquipedEquipment;}
    public static UnityEvent<string, int> OnUpgradeEquipment { get => _onUpgradeEquipment; }
    public static UnityEvent OnUpdatePlayerMirroring { get => _onUpdatePlayerMirroring; }
}
