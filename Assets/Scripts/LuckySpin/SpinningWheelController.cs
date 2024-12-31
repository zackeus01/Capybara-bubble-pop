using EasyUI.PickerWheelUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinningWheelController : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    [SerializeField] private TextMeshProUGUI spinButtonText;
    [SerializeField] private PickerWheel pickerWheel;

    [Space]
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject luckySpinPanel;
    private const string LastSpinTimeKey = "LastSpinTime";
    private const string SpinCountKey = "SpinCount";
    private bool isHaveChance;
    private bool isOutOfSpins = false;
    [SerializeField] GameObject CoinGO;
    [SerializeField] TextMeshProUGUI coinTxt;
    [SerializeField] int[] coinSpend;
    [SerializeField] GameObject NotEnoughMoney;

    private void Start()
    {
        CanSpin();
        SpinWheel();
        CheckSpinLeft();
    }
    public bool CheckHasKey()
    {
        if (PlayerPrefs.HasKey(SpinCountKey))
        {
            return true;
        }
        return false;
    }
    public void CheckSpinLeft()
    {
        if (PlayerPrefs.HasKey(SpinCountKey))
        {
            if (PlayerPrefs.GetInt(SpinCountKey) == 0)
            {
                CoinGO.SetActive(false);
                if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                {
                    spinButtonText.text = "Out of spins";
                } else
                {
                    spinButtonText.text = "Hết lượt quay";
                }
                
                spinButtonText.alignment = TextAlignmentOptions.Right;
                isOutOfSpins = true;
            }
        }
    }
    public bool CheckEnoughMoney()
    {
        int value = Int32.Parse(coinTxt.text);
        if(BankSystem.Instance.Coin - value >= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
    public void SpinWheel()
    {
        spinButton.onClick.AddListener(() =>
        {
            //spinButton.interactable = false;
            CheckSpinLeft();
            if (CheckEnoughMoney() && !isOutOfSpins)
            {
                SpinCoin();
            }
            else if (!CheckEnoughMoney() && !isOutOfSpins)
            {
                if(spinButtonText.text.ToString().EndsWith("t") || spinButtonText.text.ToString().EndsWith("u"))
                {
                    SpinCoin();
                } else
                {
                    Debug.Log("PopUp Here" + spinButtonText.text.ToString());
                    NotEnoughMoney.SetActive(true);
                }  
            }

        });
    }
    public void SpinCoin()
    {
        pickerWheel.OnSpinStart(() =>
        {
            // Add sound effect for spinning wheel
            SoundManager.Instance.PlaySFXDuration(SoundKey.SpinWheel, 3.199f);

            PlayerPrefs.SetString(LastSpinTimeKey, DateTime.Now.ToString());

            if (isHaveChance)
            {
                PlayerPrefs.SetInt(SpinCountKey, PlayerPrefs.GetInt(SpinCountKey) - 1);
            }
            int value = Int32.Parse(coinTxt.text);
            if (value != 600)
            {
                BankSystem.Instance.WithdrawCoin(value);
            }
            CanSpin();
            CheckSpinLeft();
            closeButton.interactable = false;
        });

        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            // Add sound effect for get reward
            //SoundManager.Instance.PlayOneShotSFX(SoundKey.RewardItem);

            spinButton.interactable = true;
            closeButton.interactable = true;
            switch (wheelPiece.Label)
            {
                case "Coin":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 10, wheelPiece.Amount, 10));
                    VFXRewardController.Instance.DeactivePopUpByTime(4.5f);
                    break;
                case "Gem":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Gem", 5, wheelPiece.Amount, 5));
                    VFXRewardController.Instance.DeactivePopUpByTime(4f);
                    break;
                case "Boom":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Bomb", wheelPiece.Amount, wheelPiece.Amount, wheelPiece.Amount));
                    VFXRewardController.Instance.DeactivePopUpByTime(2.5f);
                    break;
                case "Firework":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Firework", wheelPiece.Amount, wheelPiece.Amount, wheelPiece.Amount));
                    VFXRewardController.Instance.DeactivePopUpByTime(2.5f);
                    break;
                case "Ziczac":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("ZicZac", wheelPiece.Amount, wheelPiece.Amount, wheelPiece.Amount));
                    VFXRewardController.Instance.DeactivePopUpByTime(2.5f);
                    break;
                case "Rainbow":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Rainbow", wheelPiece.Amount, wheelPiece.Amount, wheelPiece.Amount));
                    VFXRewardController.Instance.DeactivePopUpByTime(2.5f);
                    break;
                case "LargeGem":
                    StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Gem", 5, wheelPiece.Amount, 5));
                    VFXRewardController.Instance.DeactivePopUpByTime(4f);
                    break;
            }

        });
        pickerWheel.Spin();
    }
    public bool CanSpin()
    {
        if (PlayerPrefs.HasKey(LastSpinTimeKey))
        {
            //Debug.Log("Đã có key");
            string lastSpinTimeString = PlayerPrefs.GetString(LastSpinTimeKey);
            DateTime lastSpinTime = DateTime.Parse(lastSpinTimeString);
            if (DateTime.Now >= lastSpinTime.AddHours(24))
            {
                isHaveChance = false;
                PlayerPrefs.SetInt(SpinCountKey, 5);
                if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                {
                    spinButtonText.text = "Get Start";
                } else
                {
                    spinButtonText.text = "Bắt đầu";
                }
                spinButtonText.fontSize = 30;
                CoinGO.SetActive(false);
                spinButtonText.alignment = TextAlignmentOptions.Right;
                return true;
            }
            isHaveChance = true;
            //Debug.Log("Đã có key nhưng chưa đủ tgian");
            if(PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
            {
                spinButtonText.text = $"Spin!({PlayerPrefs.GetInt(SpinCountKey)}/5)";
            } else
            {
                spinButtonText.text = $"Quay!({PlayerPrefs.GetInt(SpinCountKey)}/5)";
            }
            
            spinButtonText.fontSize = 24;
            CoinGO.SetActive(true);
            coinTxt.text = coinSpend[PlayerPrefs.GetInt(SpinCountKey)].ToString();
            spinButtonText.alignment = TextAlignmentOptions.Left;
            return false;
        }
        isHaveChance = false;
        //Debug.Log("chưa có key");
        CoinGO.SetActive(false);
        PlayerPrefs.SetInt(SpinCountKey, 5);
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            spinButtonText.text = "Get Start";
        }
        else
        {
            spinButtonText.text = "Bắt đầu";
        }
        spinButtonText.fontSize = 30;
        spinButtonText.alignment = TextAlignmentOptions.Right;
        return true;
    }
}