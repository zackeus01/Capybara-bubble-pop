using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUltimateState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
    }

    public override void ExitState(BossStateManager stateMachine)
    {
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {
    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
