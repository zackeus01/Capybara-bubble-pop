using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isDead, true);
    }

    public override void ExitState(BossStateManager stateMachine)
    {
        stateMachine.bossGFX.SetActive(false);
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {
    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
