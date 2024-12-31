using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootBallState : BaseGameState
{
    public override void EnterState(GameStateManager stateMachine)
    {
        base.EnterState(stateMachine);
        //GameplayEvent.OnAllFieldActionsEnd.Invoke();
        BossEvent.OnCharacterTurnStart.Invoke();
        BossEvent.OnPlayerShootBallStart.Invoke();
    }

    public override void ExitState(GameStateManager stateMachine)
    {
        base.ExitState(stateMachine);
        BossEvent.OnPlayerShootBallEnd.Invoke();
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
        return "PlayerShootBall";
    }
}
