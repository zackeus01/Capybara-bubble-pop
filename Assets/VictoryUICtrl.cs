using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUICtrl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;

    [SerializeField]
    private TMP_Text label;
    [SerializeField]
    private Image img;
    [SerializeField]
    private UIStarVFX[] starVFXs;
    [SerializeField]
    private GameObject imgGO;

    [SerializeField]
    private SaveStarController saveStarController; // Tham chiếu đến SaveStarController

    private const string LEVEL = "Level";
    private string crLevel;
    private void Start()
    {
        img.DOFade(0f, 0.4f).OnComplete(() =>
        {
            imgGO.SetActive(false);
        });
    }

    private void OnEnable()
    {
        score.text = "0";
        label.text = LevelDataHolder.LevelData.name;

        // Bắt đầu tính điểm
        StartCoroutine(CountTo(0, ScoreController.Instance.TotalScore + ScoreController.Instance.TargetScore));
    }

    private IEnumerator CountTo(int current, int moveTo)
    {
        float rate = Mathf.Abs(moveTo) / 0.4f;
        int target = current + moveTo;

        while (current != target)
        {
            current = (int)Mathf.MoveTowards(current, target, rate * Time.deltaTime);
            TryGetStar(current);

            score.text = current.ToString();
            yield return null;
        }

        // Sau khi hoàn thành việc đếm điểm, lưu dữ liệu
        SaveData(current);
    }

    private void TryGetStar(int current)
    {
        long maxScore = ScoreController.Instance.MaxScore;

        if (current >= (float)maxScore * 0.22f)
        {
            starVFXs[0].PlayVFX();
        }

        if (current >= (float)maxScore * 0.62f)
        {
            starVFXs[1].PlayVFX();
        }

        if (current >= (float)maxScore * 0.99f)
        {
            starVFXs[2].PlayVFX();
        }
    }

    private void SaveData(int currentScore)
    {
        if (saveStarController != null)
        {
            string levelId = LevelDataHolder.LevelData.LevelId;
            float starSliderValue = Mathf.Clamp01((float)currentScore / ScoreController.Instance.MaxScore);

            // Lưu dữ liệu sao
            saveStarController.SaveStarData(levelId, starSliderValue);

            // Gửi sự kiện cập nhật thành tích
            AchievementEvent.OnChangeAchievementData.Invoke(AchievementType.FullStarLevel, saveStarController.FullStarLevelCount);

            Debug.Log($"Saved star data for LevelId: {levelId}, Score: {currentScore}, StarSlider: {starSliderValue}");
        }
        else
        {
            Debug.LogWarning("SaveStarController is not assigned!");
        }
    }

    public void ReturnHome()
    {
        crLevel = LevelDataHolder.LevelData.LevelName;
        int level = int.Parse(crLevel);
        int currentLevel = PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 1);
        if (currentLevel == level)
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.CURRENT_LEVEL, PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 1) + 1);
        }
        imgGO.SetActive(true);
        //PlayerPrefs.SetInt(PlayerPrefsConst.CURRENT_LEVEL, PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 1) + 1);
        BossEvent.OnExitLevel.Invoke();
        StartCoroutine(SceneController.Instance.ChangeSceneTransition("MenuScene", img));
    }
}
