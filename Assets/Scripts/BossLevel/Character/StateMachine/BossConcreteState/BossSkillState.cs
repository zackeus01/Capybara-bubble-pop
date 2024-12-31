
using System.Collections.Generic;
using UnityEngine;

public class BossSkillState : BossBaseState
{
    //State for boss destroy ball logic
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isSkill, true);

        stateMachine.dragonCtrl.ShootBall(stateMachine);
        
    }

    public override void ExitState(BossStateManager stateMachine)
    {
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {
        //AnimatorStateInfo stateInfo = stateMachine.BossAnimator.GetCurrentAnimatorStateInfo(0);
        //if (stateInfo.normalizedTime >= 1f)
        //{
        //    stateMachine.SwitchState(stateMachine.idleState);
        //}
    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
