using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderDragon : DragonCtrl
{
    private void Awake()
    {
        this.AddEventListener();
    }

    private void AddEventListener()
    {
        BossEvent.OnBossRage.AddListener(EnterRageState);
    }

    public override void ShootBall(BossStateManager stateMachine)
    {
        HexCell targetCell;
        int count = 0;
        do
        {
            targetCell = GetRandomVisibleCell(stateMachine);

            if (!targetCell) continue;

            count++;
            if (count > 10)
            {
                Debug.LogError("Count > 10!!!!!!!!");
                break;
            }

        } while (!CheckBallChain(targetCell));

        DestroyBall(targetCell);
    }

    private HexCell GetRandomVisibleCell(BossStateManager stateMachine)
    {
        if (stateMachine.VisibleBalls.Count <= 0)
        {
            return null;
        }

        int index = Random.Range(0, stateMachine.VisibleBalls.Count - 1);
        Ball ball = stateMachine.VisibleBalls[index];
        return ball.parentCell;
    }

    private void DestroyBall(HexCell targetCell)
    {
        BossEvent.OnBossDestroyBall.Invoke(targetCell);
    }

    private bool CheckBallChain(HexCell targetCell)
    {
        Ball ball = targetCell.GetBall();

        if (ball.ballType != BallType.Normal)
        {
            return true;
        }

        List<HexCell> listCells = new List<HexCell>
        {
            targetCell
        };

        return GetCellWithSameColor(targetCell, listCells);
    }

    private bool GetCellWithSameColor(HexCell targetCell, List<HexCell> listCells)
    {
        foreach (var cell in targetCell.Neighbors)
        {
            if (!cell) continue;

            Ball ball = cell.GetBall();
            if (!ball) continue;

            if (ball.ballType != BallType.Normal) continue;
            if (ball.color != targetCell.GetBall().color) continue;

            if (!listCells.Contains(cell))
            {
                listCells.Add(cell);

                if (listCells.Count >= 3)
                {
                    return true;
                }

                return GetCellWithSameColor(cell, listCells);
            }
        }

        if (listCells.Count >= 3)
        {
            return true;
        }

        return false;
    }

    public override void EnterRageState()
    {
        List<Effect> effList = new List<Effect>
        {
            new Effect(EffectType.AttackBuff),
            new Effect(EffectType.DefenseBuff),
            new Effect(EffectType.Crit)
        };

        BossEvent.OnBossSkillBuff.Invoke(effList);
        ApplyRageMultiplier();
    }

    private void ApplyRageMultiplier()
    {
        totalDmgMultiplier = 1.1f;
        totalDmgReceiveDivisor = 0.9f;
        critBonus = 0.1f;
        manaMultiplier = 3f;
    }
}
