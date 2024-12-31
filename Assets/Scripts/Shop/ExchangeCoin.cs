using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeCoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cointxt;
    [SerializeField] private TextMeshProUGUI gemtxt;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Button increase;
    [SerializeField] private Button decrease;
    [SerializeField] private Button confirm;


    private int coin = 100;
    private int gem = 10;
    private int amt = 1;
    public GameObject popUpTab;
    public GameObject popthis;

    private void Start()
    {
        display();
    }

    private void display()
    {
        cointxt.text = coin.ToString();
        gemtxt.text = gem.ToString();
        amount.text = amt.ToString();
    }
    public void increaseAmount()
    {
        amt += 1;
        coin += 100;
        gem += 10;
        display();
    }
    public void decreaseAmount()
    {
        if ( gem >= 10 && coin >= 100 && amt >= 1)
        {
            coin -= 100;
            gem -= 10;
            amt -= 1;
            display();
        }
    }
    public void confirmAmount()
    {
        bool isEnoughtCoin = BankSystem.Instance.WithdrawGem(gem);
        if (isEnoughtCoin)
        {
            BankSystem.Instance.WithdrawGem(gem);
         //   BankSystem.Instance.DepositGem(gem);
            StartCoroutine(VFXRewardController.Instance.CreateVFXSpawnItem("Coin", 5, coin, 5));
            VFXRewardController.Instance.DeactivePopUpByTime(3.5f);
        }
        else
        {
            popUpTab.SetActive(true);
            popthis.SetActive(false);
        }

    }
}
