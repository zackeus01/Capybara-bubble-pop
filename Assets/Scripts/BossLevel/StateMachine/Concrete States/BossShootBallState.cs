using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootBallState : BaseGameState
{
    public override void EnterState(GameStateManager stateMachine)
    {
        base.EnterState(stateMachine);
        //Call event to reset data in levelField
        BossEvent.OnCharacterTurnStart.Invoke();

        //Call event to start boss turn
        BossEvent.OnBossShootBallStart.Invoke();
    }

    public override void ExitState(GameStateManager stateMachine)
    {
        base.ExitState(stateMachine);
    }

    public override void FrameUpdate(GameStateManager stateMachine)
    {
        base.FrameUpdate(stateMachine);
    }

    public override void PhysicsUpdate(GameStateManager stateMachine)
    {
        base.PhysicsUpdate(stateMachine);
    }

    public override string GetName()
    {
        return "BossShootball";
    }
}
