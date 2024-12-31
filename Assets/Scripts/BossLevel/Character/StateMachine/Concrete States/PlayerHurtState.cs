using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerState
{
    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        stateMachine.IsHurt = true;
        PlayerAnimationEvent.OnAnimationEnd.AddListener(FinishHurt);
        PlayAnimation(stateMachine);
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.SetBoolAnimator(AnimationStrings.isHurt, true);
    }

    public void FinishHurt()
    {
        PlayerAnimationEvent.OnHurtAnimationEnd.Invoke();
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
        PlayerAnimationEvent.OnAnimationEnd.RemoveAllListeners();
    }

}
