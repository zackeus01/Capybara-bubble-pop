using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActionState : BaseGameState
{
    public override void EnterState(GameStateManager stateMachine)
    {
        base.EnterState(stateMachine);
        BossEvent.OnBossActionStart.Invoke();
    }

    public override void ExitState(GameStateManager stateMachine)
    {
        base.ExitState(stateMachine);
        BossEvent.OnTurnEnd.Invoke();
        GameplayEvent.OnAllFieldActionsEnd.Invoke();
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
        return "BossActionState";
    }
}
