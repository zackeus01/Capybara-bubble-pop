using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntryState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isEntry, true);
        stateMachine.bossGFX.SetActive(true);
    }

    public override void ExitState(BossStateManager stateMachine)
    {
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {
        AnimatorStateInfo stateInfo = stateMachine.BossAnimator.GetCurrentAnimatorStateInfo(0); 
        if (stateInfo.normalizedTime >= 1f)
        {
            stateMachine.SwitchState(stateMachine.idleState);
        }
    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
