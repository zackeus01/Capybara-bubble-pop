using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButtonUI : MonoBehaviour
{
    [Header("Achievement Data")]
    [SerializeField] private int achievementId;
    [SerializeField] private Image achievementImage;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI achievementDescription;
    [Header("Reward")]
    [SerializeField] private Button rewardButton;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI rewardAmount;

    public void SetupButton(int id)
    {
        achievementId = id;
        Achievement data = AchievementDataController.Instance.GetAchievement(id);

        //UI
        achievementImage.sprite = data.AchievementSprite;
        if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            achievementDescription.text = data.Description;
        } else
        {
            achievementDescription.text = data._vietnameseDes;
        }
        
        progressSlider.maxValue = data.AchievementCap;
        progressSlider.value = data.Progress;
        progressText.text = $"{data.Progress} / {data.AchievementCap}";

        //Reward
        rewardButton.interactable = data.CanClaimReward();

        rewardButton.onClick.RemoveListener(ClaimReward);
        rewardButton.onClick.AddListener(ClaimReward);

        rewardImage.sprite = data.RewardSprite;
        rewardAmount.text = data.RewardAmount.ToString();
    }

    private void ClaimReward()
    {
        AchievementEvent.OnClaimAchievementReward.Invoke(achievementId);
        SetupButton(achievementId);
    }

}
