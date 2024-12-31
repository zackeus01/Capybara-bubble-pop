using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelData", menuName = "Bubble Shooter/Level")]

public class LevelData : ScriptableObject
{
    [Header("Level File")]
    [SerializeField] private string _pathToLevelFile;
    [SerializeField] private TextAsset _levelFile;

    [Header("Level Parameter")]
    [SerializeField] private string _levelId;
    [SerializeField] private string _levelName;
    [SerializeField] private long _levelMaxScore;
    [SerializeField] private bool _unlockLevelOnStart;
    [SerializeField] private int _countAvailableBalls;
    [SerializeField] private List<BallColor> _ballColorsInLevel;
    [SerializeField] private List<RewardSO> levelRewards;
    
    [Header("Score Parameter")]
    [Min(1)]
    [SerializeField] private int _ballDestroyScore;
    [Min(1)]
    [SerializeField] private int _ballDestroyScaleGroup;
    [Min(1)]
    [SerializeField] private int _ballDropScore;
    [Min(1)]
    [SerializeField] private int _ballDropScaleGroup;

    #region Boss Parameter
    [Header("Boss Parameter")]
    [SerializeField] private bool isBossLevel;
    [SerializeField] private int bossDifficulty;
    #endregion

    public int BossDifficulty { get => bossDifficulty; }
    public bool IsBossLevel { get => isBossLevel; }
    public string PathToLevelFile { get => _pathToLevelFile; }
    public TextAsset LevelFile { get => _levelFile; }
    public string LevelId { get => _levelId; }
    //public string LevelName { get => _levelName; }
    public string LevelName { get => _levelName; set { _levelName = value; } }
    public long LevelMaxScore { get => _levelMaxScore; }
    public bool UnlockLevelOnStart { get => _unlockLevelOnStart; }
    public int CountAvailableBalls { get => _countAvailableBalls; }
    public List<BallColor> BallColorsInLevel { get => _ballColorsInLevel; }
    public int BallDestroyScore { get => _ballDestroyScore; }
    public int BallDestroyScaleGroup { get => _ballDestroyScaleGroup; }
    public int BallDropScore { get => _ballDropScore; }
    public int BallDropScaleGroup { get => _ballDropScaleGroup; }
    public List<RewardSO> LevelReward => levelRewards;
}
