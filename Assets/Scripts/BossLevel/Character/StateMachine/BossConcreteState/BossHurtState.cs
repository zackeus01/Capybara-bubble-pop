public class BossHurtState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isHurt, true);
    }

    public override void ExitState(BossStateManager stateMachine)
    {
        stateMachine.IsHurt = false;
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {

    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
