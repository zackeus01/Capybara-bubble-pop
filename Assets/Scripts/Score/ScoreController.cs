using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : Singleton<ScoreController>
{
    [SerializeField]
    private int _totalScore = 0;

    [SerializeField]
    private long _maxScore;

    private int _winScore = 200;

    private int oldTargetScore = 0;

    public int WinScore => _winScore;

    public long MaxScore
    {
        get { return _maxScore; }
    }

    public int TotalScore
    {
        get
        {
            return _totalScore;
        }
        set
        {
            _totalScore = value;
            GameplayEvent.OnTotalScoreChanged.Invoke(_totalScore);
        }
    }

    public int TargetScore = 0;

    [SerializeField]
    private int _stack = 0;

    public int Stack
    {
        get { return _stack; }

        set { _stack = value; }
    }

    public int Score
    {
        get { return _stack * 10; }
    }

    public int DropScore
    {
        get { return _stack * 10 * 2; }
    }

    private void OnEnable()
    {
        GameplayEvent.OnGameFieldGenerated.AddListener(ResetAll);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(AddTotalScore);
        GameplayEvent.OnLevelDataLoaded.AddListener(GetMaxScore);
    }

    private void GetMaxScore(LevelData data)
    {
        _maxScore = data.LevelMaxScore;
    }
    private void AddTotalScore()
    {
        StartCoroutine(CountTo(0.1f));
    }

    private IEnumerator CountTo(float time)
    {
        if (oldTargetScore > TotalScore)
        {
            TotalScore = oldTargetScore;
        }

        float rate = (float) TargetScore / time;
        int target = TotalScore + TargetScore;
        oldTargetScore = target;
        while (TotalScore != target)
        {
            TotalScore = (int)Mathf.MoveTowards(TotalScore, target, rate * Time.deltaTime);
            yield return null;
        }
        TotalScore = target;
        TargetScore = 0;
    }

    public void IncreaseStack()
    {
        _stack++;

        if (_stack >= 5)
        {
            DailyMissionEvent.OnUpdateMissionData.Invoke(DailyMissionType.ConsecutiveHit, 1);
            if (_stack >= 10)
            {
                AchievementEvent.OnUpdateAchievement.Invoke(AchievementType.ConsecutiveHit, 1);
            }
        }
    }

    public void ResetStack()
    {
        _stack = 0;
    }

    public void ResetAll(List<Ball> balls)
    {
        _stack = 0;
        _totalScore = 0;
        oldTargetScore = 0;
    }

    public void AddScore(int ballCount, ScoreType type)
    {
        int score = GetScore(type);

        int totalScore = score * ballCount;

        ScoreController.Instance.TargetScore += totalScore;
    }

    public int GetScore(ScoreType type)
    {
        switch (type)
        {
            case ScoreType.Drop:
                return DropScore;
            case ScoreType.Destroy:
                return Score;
            case ScoreType.Win:
                return WinScore;
            default:
                return 0;
        }
    }

    public void AddScore(ScoreType type, float delay)
    {
        int score = GetScore(type);

        ScoreController.Instance.TargetScore += score;

        StartCoroutine(CountTo(delay));
    }
}
