using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReceiveEffectState : PlayerState
{
    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);
        PlayAnimation(stateMachine);

        ApplyEffects();
        PlayerAnimationEvent.OnAnimationEnd.AddListener(FinishReceivingEffects);
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
        PlayerAnimationEvent.OnAnimationEnd.RemoveAllListeners();
    }

    public void FinishReceivingEffects()
    {
        PlayerAnimationEvent.OnBuffAnimationEnd.Invoke();
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.SetBoolAnimator(AnimationStrings.isBuffed, true);
    }

    private void ApplyEffects()
    {
        EffectApplier.ApplyEffect(CharacterType.Player, GameStateManager.Instance.BallsDestroyed);
    }
}
