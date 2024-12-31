using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PurchaseWindow : MonoBehaviour
{
    [Header("Purchase UI")]
    private List<Sprite> skinSprite;
    [SerializeField] private Transform skinDisplay;
    [SerializeField] private Image skinImage;
    [SerializeField] private Transform ballDisplay;
    [SerializeField] private Image ballImage;

    [Header("Currency")]
    [SerializeField] private GameObject gold;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject gem;
    [SerializeField] private TextMeshProUGUI gemText;

    [Header("Button")]
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private GameObject PopUpAdsItem;

    [Header("Ad")]
    //[SerializeField] private GameObject WatchAdsBtn;
    //[SerializeField] private TextMeshProUGUI textInsideWatchAds;
    [SerializeField] private Button watchAdsBtn;

    [Header("Noti")]
    [SerializeField] private TextMeshProUGUI notiText;

    public void InitData(SkinUI skin)
    {
        Debug.Log("Init Purchase Data");

        //Disable displays
        ballDisplay.gameObject.SetActive(false);

        //Display skins
        Image newSkin = Instantiate(skinImage, skinDisplay);
        newSkin.sprite = skin.Skin.Sprite;
        newSkin.preserveAspect = true;
        skinDisplay.gameObject.SetActive(true);

        gold.SetActive(skin.Skin.GoldPrice > 0);
        gem.SetActive(skin.Skin.GemPrice > 0);

        // Set price labels using string interpolation
        goldText.text = $"x{skin.Skin.GoldPrice}";
        gemText.text = $"x{skin.Skin.GemPrice}";

        // Remove previous listeners and add new one
        purchaseBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(()
            => Purchase(skin as ISkinUnlockable, skin.Skin.GoldPrice, skin.Skin.GemPrice, skin.Skin.CanUnlockByAds));

        watchAdsBtn.onClick.RemoveAllListeners();
        watchAdsBtn.onClick.AddListener(()
            => CompleteWatchAd(skin as ISkinUnlockable));
    }

    public void InitData(ThemeUI theme)
    {
        Debug.Log("Init Purchase Data");

        //Disable displays
        skinDisplay.gameObject.SetActive(false);

        //Display skins
        skinSprite = new List<Sprite>();

        foreach (var s in theme.Theme.Skins)
        {
            Image newSkin = Instantiate(ballImage, ballDisplay);
            newSkin.sprite = s.Sprite;
            newSkin.preserveAspect = true;
        }
        ballDisplay.gameObject.SetActive(true);


        gold.SetActive(theme.Theme.GoldPrice > 0);
        gem.SetActive(theme.Theme.GemPrice > 0);

        // Set price labels using string interpolation
        goldText.text = $"x{theme.Theme.GoldPrice}";
        gemText.text = $"x{theme.Theme.GemPrice}";

        // Remove previous listeners and add new one
        purchaseBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(()
            => Purchase(theme as ISkinUnlockable, theme.Theme.GoldPrice, theme.Theme.GemPrice, theme.Theme.CanUnlockByAds));

        watchAdsBtn.onClick.RemoveAllListeners();
        watchAdsBtn.onClick.AddListener(()
            => CompleteWatchAd(theme as ISkinUnlockable));
    }

    public void ClosePurchase()
    {
        this.gameObject.SetActive(false);
        Debug.Log("Close Purchase");
    }

    private void OnDisable()
    {
        ClearSkin();
        ClearBalls();
        notiText.gameObject.SetActive(false);
    }

    private void ClearSkin()
    {
        foreach (Transform child in skinDisplay)
        {
            Destroy(child.gameObject);  // Destroy each child GameObject
        }
    }

    private void ClearBalls()
    {
        foreach (Transform child in ballDisplay)
        {
            Destroy(child.gameObject);  // Destroy each child GameObject
        }
    }

    public void CompleteWatchAd(ISkinUnlockable unlockableSkin)
    {
        unlockableSkin.UnlockSkin();
        ClosePurchase();
    }


    public void Purchase(ISkinUnlockable unlockableSkin, int coin, int gem, bool canUnlockByAd)
    {
        CurrencyData currencyData = CurrencyDataController.Instance.CurrencyData;

        // Check if the player has enough currency
        bool hasEnoughCurrency = currencyData.Coin >= coin &&
                                 currencyData.Gem >= gem;

        Debug.Log(hasEnoughCurrency);

        //If not enough currency AND ad is UNAVAILABLE
        if (!hasEnoughCurrency && !canUnlockByAd)
        {
            Debug.Log("not enough currency AND ad is UNAVAILABLE");
            notiText.text = "Not enough currency. Please go to shop!";
            notiText.gameObject.SetActive(true);
            //GoToShop();

            //PopUpAdsItem.SetActive(true);
            //WatchAdsBtn.SetActive(false);
            //textInsideWatchAds.text = "Not enough item, plese go to Shop to trade or purchase";
        }
        //If not enough currency AND ad is AVAILABLE
        else if (!hasEnoughCurrency && canUnlockByAd)
        {
            Debug.Log("not enough currency AND ad is AVAILABLE");
            notiText.text = "Not enough currency. Please go to shop or watch an Ad!";
            notiText.gameObject.SetActive(true);

            //textInsideWatchAds.text = "Do you want to watch ads or purchase?";
            //WatchAdsBtn.SetActive(true);
            //PopUpAdsItem.SetActive(true);
            ////Debug.Log(skinUI.name);
            //watchAdsBtn.onClick.RemoveAllListeners();
            //watchAdsBtn.onClick.AddListener(() => WatchAdsComplete(skin));
        }

        //If has enough currency
        if (hasEnoughCurrency)
        {
            Debug.Log("has enough currency");

            // Deduct currency amounts
            currencyData.Coin -= coin;
            currencyData.Gem -= gem;
            UIEvent.OnCurrencyChanged.Invoke();

            CurrencyDataController.Instance.SaveData();


            unlockableSkin.UnlockSkin();
            ClosePurchase();
        }
    }

    public void GoToShop()
    {
        PopUpAdsItem.SetActive(false);
        ClosePurchase();
    }
}
