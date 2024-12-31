using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReceiveEffectState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        EffectApplier.ApplyEffect(CharacterType.Boss, GameStateManager.Instance.BallsDestroyed);
        stateMachine.BossAnimator.SetBool(AnimationStrings.isBuffed, true);
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
