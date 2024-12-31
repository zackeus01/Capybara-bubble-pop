using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreezeState : PlayerState
{

    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        //BossEvent.OnPlayerActionEnd.AddListener(UnFreeze);
    }

    public void UnFreeze()
    {
        //player.IsFreezing = false;
        //playerStateMachine.ChangeState(player.StandByState);
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
    }
}
