using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VFXRewardController : MonoBehaviour
{
    public static VFXRewardController Instance { get; private set; }

    public GameObject[] rewardItems;
    public RectTransform MiddlePos;
    public TextMeshProUGUI[] rewardTxt;
    public GameObject PopUp;
    public RectTransform[] itemPos;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        UpdateText();
    }
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        rewardTxt[0].text = BankSystem.Instance.Coin.ToString();
        rewardTxt[1].text = BankSystem.Instance.Gem.ToString();
        rewardTxt[2].text = HelperController.Instance.GetHelperCount(BallType.Bomb).ToString();
        rewardTxt[3].text = HelperController.Instance.GetHelperCount(BallType.Ziczac).ToString();
        rewardTxt[4].text = HelperController.Instance.GetHelperCount(BallType.Rainbow).ToString();
        rewardTxt[5].text = HelperController.Instance.GetHelperCount(BallType.Firework).ToString();
    }
    public IEnumerator CreateVFXSpawnItem(string Item, int quantity, int amount, int split)
    {
        
        RectTransform directionPos = null;
        PopUp.SetActive(true);
        UpdateText();
        switch (Item)
        {
            case "Coin":
                directionPos = itemPos[0];
                break;
            case "Gem":
                directionPos = itemPos[1];
                break;
            case "Bomb":
                directionPos = itemPos[2];
                break;
            case "ZicZac":
                directionPos = itemPos[3];
                break;
            case "Rainbow":
                directionPos = itemPos[4];
                break;
            case "Firework":
                directionPos = itemPos[5];
                break;
            case "LargeGem":
                directionPos = itemPos[1];
                break;
        }
        yield return new WaitForSeconds(0.2f);
        if (directionPos != null)
        {

            List<RectTransform> list = new List<RectTransform>();
            for (int i = 0; i < quantity; i++)
            {
                RectTransform coinFly = ObjectPool.Instance.GetObject(Item).GetComponent<RectTransform>();
                var offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
                var startPos = MiddlePos.transform.position;
                coinFly.transform.position = startPos;
                list.Add(coinFly);
            }
            int temp = 0;
            if (split != 0)
            {
                temp = (amount / split);
            }
            foreach (var coinFly in list)
            {
                coinFly.gameObject.SetActive(true);

                Vector3 endPos = directionPos.transform.position;
                coinFly.DOMoveX(coinFly.transform.position.x - 2f, 0.5f).SetEase(Ease.InQuad);
                coinFly.DOMoveY(directionPos.transform.position.y, 0.7f).SetEase(Ease.InQuad);
                coinFly.DOMoveX(coinFly.transform.position.x - 1.5f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    coinFly.DOMove(endPos, 1.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        coinFly.gameObject.SetActive(false);
                        coinFly.transform.position = MiddlePos.transform.position;
                        switch (Item)
                        {
                            case "Coin":
                                BankSystem.Instance.DepositCoin(temp);
                                break;
                            case "Gem":
                                BankSystem.Instance.DepositGem(temp);
                                break;
                            case "Bomb":
                                HelperController.Instance.AddHelper(BallType.Bomb, temp);
                                break;
                            case "ZicZac":
                                HelperController.Instance.AddHelper(BallType.Ziczac, temp);
                                break;
                            case "Rainbow":
                                HelperController.Instance.AddHelper(BallType.Rainbow, temp);
                                break;
                            case "Firework":
                                HelperController.Instance.AddHelper(BallType.Firework, temp);
                                break;
                            case "LargeGem":
                                break;
                        }
                        //SoundManager.Instance.PlayOneShotSFX(SoundKey.RewardItem);
                        UpdateText();
                    });
                });
                yield return new WaitForSeconds(0.2f);
            }
        }

        //yield return new WaitForSeconds(3f);
    }
    public void StartVFXSpawnItem(string Item, int quantity, int amount, int split)
    {
        StartCoroutine(CreateVFXSpawnItem(Item, quantity, amount, split));
    }

    public void StartVFXSpawnItem(string Item, int amount)
    {
        StartCoroutine(CreateVFXSpawnItem(Item, 1, amount, 1));
    }

    public void DeactivePopUpByTime(float timeer)
    {
        StartCoroutine(DisablePopUpTab(timeer));
    }
    public IEnumerator DisablePopUpTab(float time)
    {
        yield return new WaitForSeconds(time);
        PopUp.SetActive(false);
    }
}
