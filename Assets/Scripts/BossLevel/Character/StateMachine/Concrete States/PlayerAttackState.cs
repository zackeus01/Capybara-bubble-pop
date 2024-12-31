using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        PlayerAnimationEvent.OnAnimationEnd.AddListener(FinishAttack);
        PlayAnimation(stateMachine);
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.SetBoolAnimator(AnimationStrings.isAttack, true);
    }

    public void FinishAttack()
    {
        PlayerAnimationEvent.OnAttackAnimationEnd.Invoke();
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
        PlayerAnimationEvent.OnAnimationEnd.RemoveAllListeners();
    }
}
