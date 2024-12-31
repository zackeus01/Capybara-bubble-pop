using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : Singleton<GameStateManager>
{
    private BaseGameState currentState { get; set; }

    public BaseGameState CurrentState { get => currentState; }

    #region State machine
    public PlayerShootBallState playerShootState = new PlayerShootBallState();
    public PlayerActionState playerActionState = new PlayerActionState();
    public BossShootBallState bossShootState = new BossShootBallState();
    public BossActionState bossActionState = new BossActionState();

    private bool isStateChangable = true;
    #endregion

    private Dictionary<BallColor, int> ballsDestroyed = new Dictionary<BallColor, int>();
    public Dictionary<BallColor, int> BallsDestroyed { get { return ballsDestroyed; } }

    private void Start()
    {
        Initialize(playerShootState);
        isStateChangable = true;

        BossEvent.OnShootBallTurnEnd.AddListener(PlayNextState);
        BossEvent.OnBossActionEnd.AddListener(ToNextState);
        BossEvent.OnPlayerActionEnd.AddListener(ToNextState);

        BossEvent.OnExitLevel.AddListener(ExitLevel);
        GameplayEvent.OnGameFieldSetupDone.AddListener(AllowChangeState);
    }

    private void Update()
    {
        currentState.FrameUpdate(this);
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void Initialize(BaseGameState startingState)
    {
        currentState = startingState;
        currentState.EnterState(this);
    }

    public void ChangeState(BaseGameState newState)
    {
        if (isStateChangable)
        {
            currentState.ExitState(this);
            Debug.Log("<color=blue>GAME STATE / EXIT state:</color> " + currentState);
            currentState = newState;
            Debug.Log("<color=blue>GAME STATE / ENTER state:</color> " + currentState);
            currentState.EnterState(this);
        }
    }


    private void PlayNextState(Dictionary<BallColor, int> ballsDestroyedInPreviousTurn)
    {
        ballsDestroyed = ballsDestroyedInPreviousTurn;
        ToNextState();
    }

    private void ToNextState()
    {
        int crrStateIndex = GetCurrentStateIndex();
        crrStateIndex = IncreaseStateIndex(crrStateIndex);

        GameStateEnum stateType = (GameStateEnum)crrStateIndex;

        switch (stateType)
        {
            case GameStateEnum.PlayerShoot:
                ChangeState(playerShootState);
                break;
            case GameStateEnum.PlayerAttack:
                ChangeState(playerActionState);
                break;
            case GameStateEnum.BossShoot:
                ChangeState(bossShootState);
                break;
            case GameStateEnum.BossAttack:
                ChangeState(bossActionState);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(crrStateIndex), "Invalid state index");
        }
    }

    private int GetCurrentStateIndex()
    {
        if (currentState == playerShootState) return (int)GameStateEnum.PlayerShoot;
        if (currentState == playerActionState) return (int)GameStateEnum.PlayerAttack;
        if (currentState == bossShootState) return (int)GameStateEnum.BossShoot;
        if (currentState == bossActionState) return (int)GameStateEnum.BossAttack;

        throw new InvalidOperationException("Current state is not recognized");
    }

    private int IncreaseStateIndex(int currentState)
    {
        currentState++;

        if (currentState > 3)
        {
            currentState -= 4;
        }

        return currentState;
    }

    public void ExitLevel()
    {
        isStateChangable = false;
        ToPlayerShootBallState();
    }

    private void ToPlayerShootBallState()
    {
        if (currentState == null)
        {
            Initialize(playerShootState);
        }
        else if (currentState != playerShootState)
        {
            currentState.ExitState(this);
            Debug.Log("<color=blue>GAME STATE / EXIT state:</color> " + currentState);
            currentState = playerShootState;
            Debug.Log("<color=blue>GAME STATE / ENTER state:</color> " + currentState);
            currentState.EnterState(this);
        }
    }

    private void AllowChangeState()
    {
        isStateChangable = true;
    }
}
