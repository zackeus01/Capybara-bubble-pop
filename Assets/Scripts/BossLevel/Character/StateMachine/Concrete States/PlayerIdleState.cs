using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        PlayAnimation(stateMachine);
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.SetBoolAnimator(AnimationStrings.isIdle, true);
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
    }
}
