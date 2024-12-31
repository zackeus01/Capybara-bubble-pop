using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject VictoryUI;
    [SerializeField] GameObject RetryUI;
    [SerializeField] private Image img;
    [SerializeField] private GameObject go;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject TipUI;
    [SerializeField] Shooter shooter;


    private int CountBall;
    private void Reset()
    {
        GameOverUI = GameObject.Find("GameOverUI");
        VictoryUI = GameObject.Find("VictoryUI");
        RetryUI = GameObject.Find("RetryUI");

    }
    private void Awake()
    {
        GameplayEvent.OnAvailableBallsCountChanged.AddListener(CountBallLeft);
        //GameplayEvent.onTotalBallLeftChange.AddListener(CountTotalBallOnFieldLeft);
        GameplayEvent.OnGameWin.AddListener(PopUpWinUI);
        GameplayEvent.OnGameOver.AddListener(PopUpGameOverUI);
    }

    private void Start()
    {
        GameOverUI.SetActive(false);
        VictoryUI.SetActive(false);
        RetryUI.SetActive(false);
        if (!PlayerPrefs.HasKey("DoneTutorial"))
        {
            Tutorial.SetActive(true);
        }
        shooter = FindAnyObjectByType<Shooter>();
    }

    private void OnDestroy()
    {
        GameplayEvent.OnGameWin.RemoveListener(PopUpWinUI);
        GameplayEvent.OnGameOver.RemoveListener(PopUpGameOverUI);
        //GameplayEvent.onTotalBallLeftChange.RemoveListener(CountTotalBallOnFieldLeft);
    }

    private void CountBallLeft(int count)
    {
        CountBall = count;

        if (CountBall == 0)
        {
            PopUpGameOverUI();
        }
    }

    public void OnClickToPlayOn(int value)
    {
        shooter.IncreaseCountBall(value);
        shooter.UnlockCatapult();
        Debug.LogWarning("Ads - Cash out here");
    }

    private void CountTotalBallOnFieldLeft(int count)
    {
        if (count <= 0 && CountBall > 0)
        {
            PopUpWinUI();
            SoundManager.Instance.PlayOneShotSFX(SoundKey.Victory);
        }
        //Debug.Log("count" + count);
    }

    private void PopUpWinUI()
    {
        //PopupOn();
        shooter.LockCatapult();
        VictoryUI.SetActive(true);

        if (LevelDataHolder.LevelData.IsBossLevel)
        {
            return;
        }

        SaveStarController ssc = new SaveStarController();

        if (LevelDataHolder.LevelData.LevelReward.Count > 0 && !ssc.LoadStarDataForLevel(LevelDataHolder.LevelData.LevelId).IsRewardCollected)
        {
            LevelDataHolder.LevelData.LevelReward.ForEach(reward => reward.ClaimReward());
            ssc.ClaimReward(LevelDataHolder.LevelData.LevelId);
        }
    }

    private void PopUpGameOverUI()
    {
        StartCoroutine(ShowGameOverUIAfterDelay(1f));
    }

    private IEnumerator ShowGameOverUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOverUI.SetActive(true);
        PopupOn();
    }

    public void Home()
    {
        PopupOff();
        BossEvent.OnExitLevel.Invoke();
        go.SetActive(true);
        StartCoroutine(SceneController.Instance.ChangeSceneTransition("MenuScene", img));
    }

    public void TryAgain()
    {
        PopupOff();
        go.SetActive(true);

        if (LevelDataHolder.LevelData.IsBossLevel)
        {
            BossEvent.OnExitLevel.Invoke();
            StartCoroutine(SceneController.Instance.ChangeSceneTransition("Boss", img));
            return;
        }

        StartCoroutine(SceneController.Instance.ChangeSceneTransition("GamePlay", img));
    }

    public void PlayOn()
    {
        shooter.IncreaseCountBall(5);
        PopupOff();
    }

    public void PopupOn()
    {
        shooter.PauseHandle();
    }

    public void PopupOff()
    {
        shooter.UnpauseHandle();
    }

    public void PauseGame()
    {
        if (PlayerPrefs.HasKey("DoneTutorial"))
        {
            PauseUI.SetActive(true);
            shooter.LockCatapult();
        }
        else
        {
            Debug.Log("Alert Done Tutorial");
        }
    }
    public void Resume()
    {
        PauseUI.SetActive(false);
        shooter.UnlockCatapult();
    }

    public void TipOpen()
    {
        TipUI.SetActive(true);
        shooter.LockCatapult();
    }
    public void TipClose()
    {
        TipUI.SetActive(false);
        shooter.UnlockCatapult();
    }
}
