using System;
using System.Collections.Generic;
using UnityEngine;

public enum WallPosition
{
    Left,
    Right,
    Bottom,
    Top
}

public enum WallFunction
{
    Reflect,
    Destroy,
    AttachToField
}
public class WallSettings : MonoBehaviour
{
    public WallPosition position;
    public WallFunction wallFunction;
    public bool destroyOnExit;

    public Vector2 wallNormal;
    public readonly List<Ball> dropedBalls = new List<Ball>();
    public Vector2 WallNormal { get => wallNormal; }

    public WallFunction WallFunctionValue { get => wallFunction; }

    private LevelField levelField;

    private void Start()
    {
        LoadComponent();
    }

    private void Awake()
    {
        switch (position)
        {
            case WallPosition.Left:
                wallNormal = Vector2.right;
                break;

            case WallPosition.Right:
                wallNormal = Vector2.left;
                break;
            case WallPosition.Bottom:
                wallNormal = Vector2.up;
                break;
            case WallPosition.Top:
                wallNormal = Vector2.down;
                break;
        }
        GameplayEvent.OnBallsDropStarted.AddListener(SetDroppedBallsCells);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball collisionBall;
        if (!collision.TryGetComponent<Ball>(out collisionBall))
            return;

        if (WallFunctionValue == WallFunction.Destroy)
        {
            collisionBall.DestroyBall(ScoreType.Drop, BallType.Normal);

            if (collisionBall.isActiveBall)
            {
                Debug.Log("OnAllFieldActionsEnd/WallSetting/OnTriggerEnter2D/collisionBall.isActiveBall");
                if (LevelDataHolder.LevelData.IsBossLevel)
                {
                    //Change to boss shooting state
                }
                else
                {
                    GameplayEvent.OnAllFieldActionsEnd.Invoke();
                }
            }
            else
            {
                if (dropedBalls.Count >= 0 && dropedBalls.Contains(collisionBall))
                    dropedBalls.Remove(collisionBall);

                if (dropedBalls.Count <= 0)
                {
                    //Debug.Log("Wall setting event");
                    GameplayEvent.OnBallsDropFinished.Invoke();
                    Debug.Log("OnAllFieldActionsEnd/WallSetting/OnTriggerEnter2D/dropedBalls.Count <= 0");
                    if (LevelDataHolder.LevelData.IsBossLevel)
                    {
                        BossEvent.OnShootBallTurnEnd.Invoke(levelField.BallsDestroyedInTurn);
                    }
                    else
                    {
                        GameplayEvent.OnAllFieldActionsEnd.Invoke();
                    }
                    HelperEvent.OnHelperDeactivated.Invoke();
                }
            }
        }

        if (WallFunctionValue == WallFunction.Reflect)
        {
            if (position == WallPosition.Top)
            {
                return;
            }

            if (collisionBall.ballType == BallType.Firework)
            {
                return;
            }

            collisionBall.targetVelocity = Vector2.Reflect(collisionBall.targetVelocity, wallNormal);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball collisionBall;
        if (!collision.gameObject.TryGetComponent<Ball>(out collisionBall))
            return;

        if (WallFunctionValue == WallFunction.Reflect)
        {
            if (collisionBall.isActiveBall)
                collisionBall.WallSettingsCollision = this;
        }
        else if (WallFunctionValue == WallFunction.AttachToField)
        {
            if (collisionBall.isActiveBall)
            {
                collisionBall.isBallCollided = true;
                GameplayEvent.OnActiveBallCollided.Invoke(collisionBall);
            }
        }
    }
    private void OnDestroy()
    {
        GameplayEvent.OnBallsDropStarted.RemoveListener(SetDroppedBallsCells);
    }

    private void SetDroppedBallsCells(List<HexCell> dropedBallsCells)
    {
        foreach (HexCell dropedBallCell in dropedBallsCells)
        {
            dropedBalls.Add(dropedBallCell.GetBall());
        }
    }

    private void LoadComponent()
    {
        levelField = GameObject.Find("LevelField").GetComponent<LevelField>();
    }
}
