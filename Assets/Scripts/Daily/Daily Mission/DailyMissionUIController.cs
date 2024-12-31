using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DailyMissionUIController : MonoBehaviour
{
    [SerializeField] private DailyMissionUIButton dailyMissionUIPrefab;
    [SerializeField] private Transform buttonsTransform;

    [Header("Daily Mission Reward")]
    [SerializeField] private Slider rewardSlider;
    [SerializeField] private List<RewardSO> dailyMissionRewards;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Image rewardImage;
    [SerializeField] private Button claimRewardButton;


    private ObjectPool<DailyMissionUIButton> pool;

    private void Awake()
    {
        SetupPool();
        SetupController();
    }

    private void OnEnable()
    {
        UIEvent.OnChangeMenuTab.AddListener(OnChangeMenuTab);
    }

    private void OnDisable()
    {
        ClearPool();
        UIEvent.OnChangeMenuTab.RemoveListener(OnChangeMenuTab);
    }

    private void SetupController()
    {
        rewardSlider.maxValue = DailyMissionDataController.Instance.Data.Count;
        dailyMissionRewards = DailyMissionDataController.Instance.DailyMissionReward;
    }

    private void SetupPool()
    {
        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<DailyMissionUIButton>(
            createFunc: () => Instantiate(dailyMissionUIPrefab, buttonsTransform),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 6
        );

    }
    public void ClearPool()
    {
        // Loop through all active objects in the pool and release them
        for (int i = buttonsTransform.childCount - 1; i >= 0; i--)
        {
            var btn = buttonsTransform.GetChild(i).GetComponent<DailyMissionUIButton>();
            if (btn != null && btn.isActiveAndEnabled)
            {
                pool.Release(btn);
            }
        }

    }

    public void SetupUI()
    {
        ClearPool();

        foreach (DailyMissionDataSO dmd in DailyMissionDataController.Instance.Data)
        {
            pool.Get().SetupButton(dmd);
        }

        rewardImage.sprite = dailyMissionRewards[0].RewardSprite;
        rewardSlider.value = DailyMissionDataController.Instance.Data.Count(dmd => dmd.IsCompleted);
        rewardText.text = $"{rewardSlider.value} / {rewardSlider.maxValue}";

        claimRewardButton.interactable = (DailyMissionDataController.Instance.Data.All(dmd => dmd.IsCompleted)
                                            && !DailyMissionDataController.Instance.IsDailyMissionRewardClaimed);
       

    }

    public void ClaimDailyMissionReward()
    {
        claimRewardButton.interactable = false;

        DailyMissionEvent.OnClaimDailyMissionReward.Invoke();
    }

    public void OnChangeMenuTab(int id)
    {
        //Debug.Log(id, this);
        if (id == 2) SetupUI();
    }
}
