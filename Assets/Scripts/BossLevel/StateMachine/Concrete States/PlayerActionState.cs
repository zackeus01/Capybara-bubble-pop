using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionState : BaseGameState
{
    private bool skipTurn = true;
    public override void EnterState(GameStateManager stateMachine)
    {
        base.EnterState(stateMachine);
        BossEvent.OnPlayerActionStart.Invoke();
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
        return "PlayerActionBall";
    }

}
