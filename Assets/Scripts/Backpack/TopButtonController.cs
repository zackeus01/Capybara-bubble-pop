
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopButtonController : MonoBehaviour
{
    [Header("Hard Button")]
    [SerializeField] private Button Ball;
    [SerializeField] private GameObject ballTab;
    [SerializeField] private Image ballImg;

    [SerializeField] private Button Player;
    [SerializeField] private GameObject playerTab;
    [SerializeField] private Image playerImg;


    [SerializeField] private Button Inventory;
    [SerializeField] private GameObject inventoryTab;
    [SerializeField] private Image invenImg;

    [Header("Infor")]
    [SerializeField] private Sprite imgInactive;
    [SerializeField] private Sprite imgActive;
    [SerializeField] private Button player;
    [SerializeField] private Button inventory;
    [SerializeField] private Button btnDisplayNoti;
    [SerializeField] private GameObject gojBtnNoti;
    [SerializeField] private GameObject popUpNoti;
    [SerializeField] private TextMeshProUGUI textNoti;



    //private void Awake()
    //{
    //    UnLockButtonPassLevel9();
    //}

    //public void UnLockButtonPassLevel9()
    //{

    //    if (PlayerPrefs.GetInt("CurrentLevel") < 10)
    //    {
    //        Debug.Log(PlayerPrefs.GetInt("CurrentLevel"));

    //        player.interactable = false;
    //        inventory.interactable = false;
    //        gojBtnNoti.SetActive(true);
    //        //playerLock.interactable = true ;
    //        //inventoryLock.interactable = true ;

    //    }
    //    else

    //    {
    //        //playerLock.interactable = false;
    //        //inventoryLock.interactable = false;
    //        gojBtnNoti.SetActive(false);
    //        player.interactable = true;
    //        inventory.interactable = true;

    //    }
    //}

    public void OnActicePopUpLock()
    {
        Debug.Log("Hien popup");
        popUpNoti.SetActive(true);
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            textNoti.text = $"You must pass level 9.";
        }
        else
        {
            textNoti.text = $"Bạn phải vượt qua level 9.";
        }

        SetActiveForBall();
    }


    public void SetActiveForBall()
    {
        ballImg.sprite = imgActive;
        playerImg.sprite = imgInactive;
        invenImg.sprite = imgInactive;

        ballTab.SetActive(true);
        playerTab.SetActive(false);
        inventoryTab.SetActive(false);
    }
    public void SetActiveForPlayer()
    {
        Debug.Log(PlayerPrefs.GetInt("CurrentLevel"));
        if (PlayerPrefs.GetInt("CurrentLevel") < 10)
        {
            OnActicePopUpLock();
        }
        else
        {
            ballImg.sprite = imgInactive;
            playerImg.sprite = imgActive;
            invenImg.sprite = imgInactive;

            ballTab.SetActive(false);
            playerTab.SetActive(true);
            inventoryTab.SetActive(false);
        }

    }
    public void SetActiveForInventory()
    {

        if (PlayerPrefs.GetInt("CurrentLevel") < 10)
        {
            OnActicePopUpLock();
        }
        else
        {
            ballImg.sprite = imgInactive;
            playerImg.sprite = imgInactive;
            invenImg.sprite = imgActive;

            ballTab.SetActive(false);
            playerTab.SetActive(false);
            inventoryTab.SetActive(true);
        }

    }



}
