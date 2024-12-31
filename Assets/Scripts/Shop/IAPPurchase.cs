using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IAPPurchase : MonoBehaviour
{
    private IAPPack currentPack;
    private int currentIndex;
    private int boom;
    private int ziczac;
    private int rainbow;
    private int firework;
    private int coin;
    private int gem;
    public GameObject starterPack;
    [Header("Purschase Helper")]
    public GameObject popUpTab;
    public Button goToGemOrCoinPos;
    public RectTransform contentViewPos; 
    public void UpdateIAPPopup(IAPPack pack, int index)
    {
        currentIndex = index;

        currentPack = pack;
        if (pack.boomtxt != null)
        {
            string boomStr = pack.boomtxt.text.Replace("x", "");
            int.TryParse(boomStr, out boom);
        }

        if (pack.ziczactxt != null)
        {
            string ziczacStr = pack.ziczactxt.text.Replace("x", "");
            int.TryParse(ziczacStr, out ziczac);
        }

        if (pack.rainbowtxt != null)
        {
            string rainbowStr = pack.rainbowtxt.text.Replace("x", "");
            int.TryParse(rainbowStr, out rainbow);
        }

        if (pack.fireworktxt != null)
        {
            string fireworkStr = pack.fireworktxt.text.Replace("x", "");
            int.TryParse(fireworkStr, out firework);
        }
        if (pack.Coin != null) int.TryParse(pack.Coin.text, out coin);
        if (pack.Gem != null) int.TryParse(pack.Gem.text, out gem);



    }
    public void BuyItem()
    {
        if (CheckInternet.Instance.CheckInternetInformation())
        {
            CustomDebug.LogError("Purchase API activated", Color.red);

            // Add sound effect for rewarding
            SoundManager.Instance.PlayOneShotSFX(SoundKey.BuyItem);

            switch (currentPack.rewardType)
            {
                case IAPPack.RewardType.COIN:
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, currentPack.rewardCoin, 10));
                    VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
                    break;
                case IAPPack.RewardType.GEM:
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Gem", 5, currentPack.rewardGem, 5));
                    VFXRewardController.Instance.DeactivePopUpByTime(3.5f);
                    break;

                case IAPPack.RewardType.IAPCOIN:
                    if (currentPack.Coin != null)
                    {
                        bool isEnoughtCoin = BankSystem.Instance.WithdrawCoin(coin);
                        if (isEnoughtCoin)
                        {
                            if (currentPack.boomtxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                            }
                            if (currentPack.ziczactxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                            }
                            if (currentPack.rainbowtxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                            }
                            if (currentPack.fireworktxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));
                            }
                            VFXRewardController.Instance.DeactivePopUpByTime(3f);
                        }
                        else
                        {
                            popUpTab.SetActive(true);
                            goToGemOrCoinPos.onClick.AddListener(() =>
                            {
                                returnToShopGemOrCoin(IAPPack.RewardType.IAPCOIN);
                            });
                        }
                    }

                    break;
                case IAPPack.RewardType.IAPGEM:
                    if (currentPack.Gem != null)
                    {
                        bool checking = BankSystem.Instance.WithdrawGem(gem);
                        if (checking)
                        {
                            if (currentPack.boomtxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                            }
                            if (currentPack.ziczactxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                            }
                            if (currentPack.rainbowtxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                            }
                            if (currentPack.fireworktxt != null)
                            {
                                StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));
                            }
                            VFXRewardController.Instance.DeactivePopUpByTime(3f);
                        }
                        else
                        {
                            popUpTab.SetActive(true);
                            goToGemOrCoinPos.onClick.AddListener(() =>
                            {
                                returnToShopGemOrCoin(IAPPack.RewardType.IAPGEM);
                            });
                        }
                    }

                    break;
                case IAPPack.RewardType.RemoveADS:
                    Debug.Log("Remove ADS");
                    break;
                case IAPPack.RewardType.START:

                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, currentPack.rewardCoin, 10));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));

                    PlayerPrefs.SetInt("StartBundle", 1);
                    starterPack.SetActive(false);
                    VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
                    break;
                case IAPPack.RewardType.HELPER:
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));
                    VFXRewardController.Instance.DeactivePopUpByTime(3f);
                    break;
                case IAPPack.RewardType.HELPER | IAPPack.RewardType.COIN:

                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));

                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, currentPack.rewardCoin, 10));
                    VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
                    break;
                case IAPPack.RewardType.HELPER | IAPPack.RewardType.COIN | IAPPack.RewardType.GEM:
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Gem", 5, currentPack.rewardGem, 5));

                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", boom, boom, boom));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", ziczac, ziczac, ziczac));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", rainbow, rainbow, rainbow));
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", firework, firework, firework));

                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, currentPack.rewardCoin, 10));
                    VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
                    break;
            }
        }
    }
    public void returnToShopGemOrCoin(IAPPack.RewardType type)
    {
        switch (type)
        {
            case IAPPack.RewardType.IAPCOIN:
                if (PlayerPrefs.HasKey("StartBundle"))
                {
                    contentViewPos.DOAnchorPosY(516f, 0.5f).SetEase(Ease.OutSine);
                } else
                {
                    contentViewPos.DOAnchorPosY(776f, 0.5f).SetEase(Ease.OutSine);
                }
                break;
            case IAPPack.RewardType.IAPGEM:
                if (PlayerPrefs.HasKey("StartBundle"))
                {
                    contentViewPos.DOAnchorPosY(1f, 0.5f).SetEase(Ease.OutSine);
                }
                else
                {
                    contentViewPos.DOAnchorPosY(213f, 0.5f).SetEase(Ease.OutSine);
                }
                break;
        }
        goToGemOrCoinPos.onClick.RemoveAllListeners();
    }
}
