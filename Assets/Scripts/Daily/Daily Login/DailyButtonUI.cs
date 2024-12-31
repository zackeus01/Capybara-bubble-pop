using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyButtonUI : MonoBehaviour
{
    [SerializeField] private int id;

    [SerializeField] private Button btn;
    [SerializeField] private Image tick;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject highlightToday;
    [SerializeField] private RewardSO reward;
    private void OnEnable()
    {
        btn.onClick.AddListener(ClaimReward);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(ClaimReward);
    }

    public void SetHighlight(bool isRewardClaimed)
    {
        btn.interactable = !isRewardClaimed;
        highlightToday.SetActive(!isRewardClaimed);
        highlightImage.gameObject.SetActive(true);
    }
    public string GetOnlyNumbers(string input)
    {
        return Regex.Replace(input, "[^0-9]", "");
    }
    public void SetupButton(int id, bool isRewardClaimed)
    {
        this.id = id;
        if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            label.text = reward.RewardName;
        } else
        {
            label.text = $"Ngày {GetOnlyNumbers(reward.RewardName)}";
        }
        

        //Debug.Log(reward.RewardSprite);

        rewardImage.sprite = reward.RewardSprite;
        amountText.text = "x" + reward.RewardAmount.ToString();

        btn.interactable = false;
        tick.gameObject.SetActive(isRewardClaimed);
        highlightToday.SetActive(false);
        highlightImage.gameObject.SetActive(false);
    }

    private void ClaimReward()
    {
        btn.interactable = false;
        highlightToday.SetActive(false);
        //highlightImage.gameObject.SetActive(false);
        tick.gameObject.SetActive(true);
        //reward.ClaimReward();
        UIEvent.OnClaimDailyReward.Invoke(id);
        checkClaim();
    }
    private void checkClaim()
    {
        switch (reward.RewardType)
        {
            case RewardType.Coin:
                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, reward.rewardAmount, 10));
                StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(4.5f));
                break;
            case RewardType.Gem:
                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Gem", 5, reward.rewardAmount, 5));
                StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(4f));
                break;
            case RewardType.Helper:
                switch (reward.HelperReward)
                {
                    case BallType.Bomb:
                        StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", reward.rewardAmount, reward.rewardAmount, reward.rewardAmount));
                        StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(2.5f));
                        break;
                    case BallType.Firework:
                        StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", reward.rewardAmount, reward.rewardAmount, reward.rewardAmount));
                        StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(2.5f));
                        break;
                    case BallType.Rainbow:
                        StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", reward.rewardAmount, reward.rewardAmount, reward.rewardAmount));
                        StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(2.5f));
                        break;
                    case BallType.Ziczac:
                        StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", reward.rewardAmount, reward.rewardAmount, reward.rewardAmount));
                        StartCoroutine(VFXRewardController.Instance.DisablePopUpTab(2.5f));
                        break;
                }
                break;
        }

    }
}
