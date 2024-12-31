using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public int currentPage;
    [Header("Swipe")]
    [SerializeField] List<GameObject> listUI;
    public float fadeTime;
    public Vector3 targetPos;
    public Vector3 pageStep;
    public RectTransform levelPagesRect;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI homeGemTxt;
    [SerializeField] private TextMeshProUGUI homeCoinTxt;
    [SerializeField] private TextMeshProUGUI shopGemTxt;
    [SerializeField] private TextMeshProUGUI shopCoinTxt;
    [SerializeField] private TextMeshProUGUI backpackGemTxt;
    [SerializeField] private TextMeshProUGUI backpackCoinTxt;
    [SerializeField] private TextMeshProUGUI achiveCoinTxt;
    [SerializeField] private TextMeshProUGUI achiveGemTxt;
    [SerializeField] private TextMeshProUGUI dailyCoinTxt;
    [SerializeField] private TextMeshProUGUI dailyGemTxt;
    [Header("Bottom Navigation")]
    [SerializeField] RectTransform NavItem;
    [SerializeField] Image NavIcon;
    [SerializeField] TextMeshProUGUI NavText;
    [SerializeField] RectTransform[] defaultItemPos;
    [SerializeField] Image[] ItemImages;

    [SerializeField] GameObject FadeIn;
    public DateTime? LastClaimedTime
    {
        get
        {
            string data = PlayerPrefs.GetString(PlayerPrefsConst.LAST_LOGIN_REWARD_CLAIMED_TIME, DateTime.MinValue.ToString());

            if (!string.IsNullOrEmpty(data))
            {
                return DateTime.Parse(data);
            }

            return null;
        }

        private set
        {
            if (value != null)
            {
                Debug.Log($"PlayerPref sets Value = {value.ToString()}");
                PlayerPrefs.SetString(PlayerPrefsConst.LAST_LOGIN_REWARD_CLAIMED_TIME, value.ToString());
            }
            else
                PlayerPrefs.DeleteKey(PlayerPrefsConst.LAST_LOGIN_REWARD_CLAIMED_TIME);
        }
    }

    private void Awake()
    {
        InitEvent();
    }

    private void Start()
    {
        UpdateCurrency();
        CheckNewDay();
        InitEvent();
        Application.targetFrameRate = 90;
        FadeIn.SetActive(true);
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void InitEvent()
    {
        UIEvent.OnCurrencyChanged.AddListener(UpdateCurrency);
    }

    private void RemoveEvent()
    {
        UIEvent.OnCurrencyChanged.RemoveListener(UpdateCurrency);
    }

    private void UpdateCurrency()
    {
        homeGemTxt.text = BankSystem.Instance.Gem.ToString();
        homeCoinTxt.text = BankSystem.Instance.Coin.ToString();
        shopGemTxt.text = BankSystem.Instance.Gem.ToString();
        shopCoinTxt.text = BankSystem.Instance.Coin.ToString();
        backpackGemTxt.text = BankSystem.Instance.Gem.ToString();
        backpackCoinTxt.text = BankSystem.Instance.Coin.ToString();
        achiveCoinTxt.text = BankSystem.Instance.Coin.ToString();
        achiveGemTxt.text = BankSystem.Instance.Gem.ToString();
        dailyCoinTxt.text = BankSystem.Instance.Coin.ToString();
        dailyGemTxt.text = BankSystem.Instance.Gem.ToString();
    }

    public void MovePage(int index)
    {
        targetPos = pageStep * index;
        levelPagesRect.DOAnchorPos(targetPos, fadeTime, false).SetEase(Ease.InOutQuad);
        HighLightButton(index);
        UIEvent.OnChangeMenuTab.Invoke(index);
    }

    private void HighLightButton(int index)
    {
        HorizontalLayoutGroup layoutGroup = defaultItemPos[0].transform.parent.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            switch (index)
            {
                case -2:
                    SetActiveNavItem(0, defaultItemPos[0], "Shop");
                    break;
                case -1:
                    SetActiveNavItem(1, defaultItemPos[1], "Achievement");
                    break;
                case 0:
                    SetActiveNavItem(2, defaultItemPos[2], "Home");
                    break;
                case 1:
                    SetActiveNavItem(3, defaultItemPos[3], "Backpack");
                    break;
                case 2:
                    SetActiveNavItem(4, defaultItemPos[4], "Daily");
                    break;

            }
        } else
        {
            switch (index)
            {
                case -2:
                    SetActiveNavItem(0, defaultItemPos[0], "Cửa hàng");
                    break;
                case -1:
                    SetActiveNavItem(1, defaultItemPos[1], "Thành tựu");
                    break;
                case 0:
                    SetActiveNavItem(2, defaultItemPos[2], "Trang chủ");
                    break;
                case 1:
                    SetActiveNavItem(3, defaultItemPos[3], "Túi đồ");
                    break;
                case 2:
                    SetActiveNavItem(4, defaultItemPos[4], "Nhiệm vụ");
                    break;

            }
        }

    }
    private void SetActiveNavItem(int index, RectTransform target, string text)
    {
        Vector3 worldPos = target.transform.position;
        Vector2 localPos = NavItem.parent.InverseTransformPoint(worldPos);
        NavItem.DOKill(); 
        NavItem.DOAnchorPos(new Vector2(localPos.x, NavItem.anchoredPosition.y), 0.4f).SetEase(Ease.Linear);
        NavText.text = text;
        NavIcon.sprite = ItemImages[index].sprite;
    }


    private void CheckNewDay()
    {
        if (LastClaimedTime.Value.Day < DateTime.Now.Day)
        {
            Debug.Log("On New Day");
            LastClaimedTime = DateTime.Now;
            UIEvent.OnNewDay?.Invoke();
        }
    }
}
