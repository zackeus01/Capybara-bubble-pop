using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Bubble Shooter/RewardSO")]
public class RewardSO : ScriptableObject
{
    [SerializeField] private string rewardName;
    [SerializeField] private RewardType rewardType;
    [SerializeField] private Sprite rewardSprite;
    [SerializeField] private ThemeSO rewardTheme;
    [SerializeField] private SkinSO rewardBackground;
    [SerializeField] private BallType helperReward;
    public int rewardAmount;

    public string RewardName { get { return rewardName; } }
    public RewardType RewardType { get { return rewardType; } }
    public Sprite RewardSprite { get { return rewardSprite; } }
    public ThemeSO RewardTheme { get { return rewardTheme; } }
    public SkinSO RewardBackground { get { return rewardBackground; } }
    public int RewardAmount { get { return rewardAmount; } }
    public BallType HelperReward { get { return helperReward; } }

    public void ClaimReward()
    {
        switch (rewardType)
        {
            case RewardType.Coin:
                ClaimCoin(); break;
            case RewardType.Gem:
                ClaimGem(); break;
            case RewardType.Theme:
                ClaimTheme(); break;
            case RewardType.Background:
                ClaimBackground(); break;
            case RewardType.Helper:
                ClaimHelper(); break;
        }
    }

    private void ClaimTheme()
    {
        if (rewardTheme) SkinDataController.Instance.SkinCollection.UnlockSkin(rewardTheme.Id);
    }

    private void ClaimBackground()
    {
        if (RewardBackground) SkinDataController.Instance.SkinCollection.UnlockSkin(rewardBackground.Id);
    }

    private void ClaimGem()
    {
        if (rewardAmount <= 0) return;

        VFXRewardController.Instance.StartVFXSpawnItem("Gem", 5, rewardAmount, 5);
        VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
    }

    private void ClaimCoin()
    {
        if (rewardAmount <= 0) return;

        VFXRewardController.Instance.StartVFXSpawnItem("Coin", 10, rewardAmount, 10);
        VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
    }

    private void ClaimHelper()
    {
        if (rewardAmount <= 0) return;

        switch (helperReward)
        {
            case BallType.Ziczac:
                VFXRewardController.Instance.StartVFXSpawnItem("ZicZac", rewardAmount);
                break;

            case BallType.Firework:
                VFXRewardController.Instance.StartVFXSpawnItem("Firework", rewardAmount);
                break;

            case BallType.Rainbow:
                VFXRewardController.Instance.StartVFXSpawnItem("Rainbow", rewardAmount);
                break;

            case BallType.Bomb:
                VFXRewardController.Instance.StartVFXSpawnItem("Bomb", rewardAmount);
                break;
        }

        VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
    }
}

[Serializable]
public enum RewardType
{
    Coin,
    Gem,
    Theme,
    Background,
    Helper
}
