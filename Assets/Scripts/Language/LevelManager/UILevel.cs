using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : UIChangeScene
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private TextMeshProUGUI _levelLabel;
    [SerializeField] private SaveStarController _saveStarController;
    [SerializeField] private GameObject bossLevel;
    [SerializeField] private GameObject normalLevel;
    public Button button;
    public GameObject unlockIcon;
    public GameObject unlockIconBoss;
    public GameObject lockIcon;
    public GameObject finishLevelIcon;
    public StarVFX[] stars;
    public GameObject starHolder;
    public GameObject currentLevel;
    public GameObject mousePos;
    public GameObject _transition;

    [Header("VFX")]
    [SerializeField] private ParticleSystem vfx;

    public LevelData LevelData
    {
        set
        {
            _levelData = value;
            UpdateDisplayData();
        }

        get
        {
            return _levelData;
        }
    }

    private void OnEnable()
    {
        vfx.gameObject.SetActive(false);
    }

    public void ShowVfx()
    {
        vfx.gameObject.SetActive(true);
    }

    protected override void EventSubscribe()
    {
        //base.EventSubscribe();
        ChangeSceneButton.onClick.AddListener(LevelStart);
    }

    protected override void EventUnsubscribe()
    {
        //base.EventUnsubscribe();
        ChangeSceneButton.onClick.RemoveListener(LevelStart);
    }

    private void LevelStart()
    {
        UIEvent.OnClickLevelStart.Invoke();
        LevelDataHolder.LevelData = _levelData;

        int levelNumber;

        if (int.TryParse(_levelData.LevelName, out levelNumber))
        {
            if (levelNumber % 10 == 0)
            {
                _changingScene = GameScenes.BossScene;
               
            }
            _transition.SetActive(true);
        }

        LoadChangingScene();
    }

    private void UpdateDisplayData()
    {
        if (_levelData == null)
            return;

        if (_levelLabel != null)
        {
            _levelLabel.text = _levelData.LevelName;
        }

        gameObject.name = _levelData.name;


        //ChangeSceneButton.interactable = _levelData.UnlockLevelOnStart;
    }

    public void LoadData()
    {
        LevelDataHolder.LevelData = _levelData;
        _saveStarController.LoadStarDataForLevel(_levelData.LevelId, stars);
    }
    public void BossLevel()
    {
        bossLevel.SetActive(true);
        normalLevel.SetActive(false);
        starHolder.SetActive(false);
    }
}
