using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayEvent
{
    private readonly static UnityEvent<Ball> _onActiveBallShooted = new UnityEvent<Ball>();
    private readonly static UnityEvent<List<Ball>> _onGameFieldGenerated = new UnityEvent<List<Ball>>();
    private readonly static UnityEvent<Ball> _onBallDestoyed = new UnityEvent<Ball>();
    private readonly static UnityEvent<Ball> _onActiveBallCollided = new UnityEvent<Ball>();
    private readonly static UnityEvent<Ball> _onActiveBallSetOnField = new UnityEvent<Ball>();
    private readonly static UnityEvent<Ball> _onActiveBallDestroyed = new UnityEvent<Ball>();
    private readonly static UnityEvent<List<Ball>> _onBallGroupDestroyed = new UnityEvent<List<Ball>>();
    private readonly static UnityEvent<List<HexCell>> _onBallsDropStarted = new UnityEvent<List<HexCell>>();
    private readonly static UnityEvent _onBallsDropFinished = new UnityEvent();
    private readonly static UnityEvent _onAllFieldActionsEnd = new UnityEvent();
    private readonly static UnityEvent<Ball> _onNextBallChanged = new UnityEvent<Ball>();
    private readonly static UnityEvent<int> _onAvailableBallsCountChanged = new UnityEvent<int>();
    private readonly static UnityEvent _onAvailableBallsEnd = new UnityEvent();
    private readonly static UnityEvent _onCountBallsOnFieldChecked = new UnityEvent();
    private readonly static UnityEvent<int> _onTotalBallLeftChange = new UnityEvent<int>();
    private readonly static UnityEvent<List<Ball>> _onGameFieldChanged = new UnityEvent<List<Ball>>();
    private readonly static UnityEvent _onGameOver = new UnityEvent();
    private readonly static UnityEvent _onGameWin = new UnityEvent();
    private readonly static UnityEvent<int> _onTotalScoreChanged = new UnityEvent<int>();
    private readonly static UnityEvent<LevelData> _onLevelDataLoaded = new UnityEvent<LevelData>();
    private readonly static UnityEvent _onMoveGameObjectDone = new UnityEvent();
    private readonly static UnityEvent _onNoBallOnField = new UnityEvent();
    private readonly static UnityEvent<Ball> _onActiveBallChanged = new UnityEvent<Ball>();
    private readonly static UnityEvent _onGameFieldSetupDone = new UnityEvent();


    public static UnityEvent OnGameFieldSetupDone { get => _onGameFieldSetupDone; }
    public static UnityEvent OnNoBallOnField { get => _onNoBallOnField; }
    public static UnityEvent<Ball> OnActiveBallShooted { get => _onActiveBallShooted; }
    public static UnityEvent OnMoveGameObjectDone { get => _onMoveGameObjectDone; }
    public static UnityEvent<LevelData> OnLevelDataLoaded { get => _onLevelDataLoaded; }
    public static UnityEvent<int> onTotalBallLeftChange { get => _onTotalBallLeftChange; }
    public static UnityEvent<List<Ball>> OnGameFieldGenerated { get => _onGameFieldGenerated; }
    public static UnityEvent<Ball> OnBallDestroyed { get => _onBallDestoyed; }
    public static UnityEvent<Ball> OnActiveBallCollided { get => _onActiveBallCollided; }
    public static UnityEvent<Ball> OnActiveBallSetOnField { get => _onActiveBallSetOnField; }
    public static UnityEvent<Ball> OnActiveBallDestroyed { get => _onActiveBallDestroyed; }
    public static UnityEvent<List<Ball>> OnBallGroupDestroyed { get => _onBallGroupDestroyed; }
    public static UnityEvent<List<HexCell>> OnBallsDropStarted { get => _onBallsDropStarted; }
    public static UnityEvent OnBallsDropFinished { get => _onBallsDropFinished; }
    public static UnityEvent OnAllFieldActionsEnd { get => _onAllFieldActionsEnd; }
    public static UnityEvent<Ball> OnNextBallChanged { get => _onNextBallChanged; }
    public static UnityEvent<int> OnAvailableBallsCountChanged { get => _onAvailableBallsCountChanged; }
    public static UnityEvent OnAvailableBallsEnd { get => _onAvailableBallsEnd; }
    public static UnityEvent OnCountBallsOnFieldChecked { get => _onCountBallsOnFieldChecked; }
    public static UnityEvent OnGameOver { get => _onGameOver; }
    public static UnityEvent OnGameWin { get => _onGameWin; }
    public static UnityEvent<List<Ball>> OnGameFieldChanged { get => _onGameFieldChanged; }
    public static UnityEvent<int> OnTotalScoreChanged { get => _onTotalScoreChanged; }
    public static UnityEvent<Ball> OnActiveBallChanged { get => _onActiveBallChanged; }
}
