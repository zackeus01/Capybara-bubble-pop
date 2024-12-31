using UnityEngine;
using UnityEngine.UI;

public class StarController : MonoBehaviour
{
    [SerializeField]
    private StarVFX[] stars;
    [SerializeField]
    private Slider starSlider;
    [SerializeField]
    private SaveStarController saveStarController;

    private void Awake()
    {
        GameplayEvent.OnTotalScoreChanged.AddListener(MoveSlider);
        GameplayEvent.OnGameWin.AddListener(SaveData);
    }

    private void OnDisable()
    {
        GameplayEvent.OnTotalScoreChanged.RemoveListener(MoveSlider);

    }
    private void OnDestroy()
    {
        GameplayEvent.OnGameWin.RemoveListener(SaveData);
    }

    private void MoveSlider(int score)
    {
        float rate = (float)score / ScoreController.Instance.MaxScore;

        if (rate > 1)
        {
            rate = 1;
        }

        starSlider.value = rate;

        TryGetStar();
    }

    private void TryGetStar()
    {
        if (starSlider.value >= 0.22f)
        {
            stars[0].GetStar();
        }

        if (starSlider.value >= 0.62f)
        {
            stars[1].GetStar();
        }

        if (starSlider.value >= 0.99f)
        {
            stars[2].GetStar();
        }
    }

    private void SaveData()
    {
        if (saveStarController != null)

        {
            string levelId = LevelDataHolder.LevelData.LevelId;
            saveStarController.SaveStarData(levelId, starSlider.value);
            AchievementEvent.OnChangeAchievementData.Invoke(AchievementType.FullStarLevel, saveStarController.FullStarLevelCount);
        }

    }

}
