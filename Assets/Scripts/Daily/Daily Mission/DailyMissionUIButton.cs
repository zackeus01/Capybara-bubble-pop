using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionUIButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI progressDescription;
    [SerializeField] private Image tick;
    [SerializeField] private Slider slider;
    [SerializeField] private Image rewardImg;
    [SerializeField] private TextMeshProUGUI rewardAmount;

    private void OnEnable()
    {
        claimButton.onClick.RemoveListener(ClaimMissionReward);
        claimButton.onClick.AddListener(ClaimMissionReward);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(ClaimMissionReward);
    }

    public void SetupButton(DailyMissionDataSO dmd)
    {
        this.id = dmd.Id;
        if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            description.text = dmd.Description;
        } else
        {
            description.text = dmd._vietnameseDes;
        }
        this.slider.maxValue = dmd.Goal;
        this.slider.value = dmd.Progress;
        progressDescription.text = $"{slider.value} / {slider.maxValue}";
        rewardImg.sprite = dmd.Reward[0].RewardSprite;
        rewardAmount.text = dmd.Reward[0].RewardAmount.ToString();
        claimButton.interactable = (!dmd.IsClaimed && dmd.IsCompleted);
        tick.gameObject.SetActive(dmd.IsClaimed);
    }
   

    private void ClaimMissionReward()
    {
        claimButton.interactable = false;
        tick.gameObject.SetActive(true);
        DailyMissionEvent.OnClaimMissionReward.Invoke(id);
    }
}
