public class BossIdleState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isIdle, true);
    }

    public override void ExitState(BossStateManager stateMachine)
    {
        
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {
        if (stateMachine.IsHurt)
        {
            stateMachine.SwitchState(stateMachine.hurtState);
        }
    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
        
    }
}
