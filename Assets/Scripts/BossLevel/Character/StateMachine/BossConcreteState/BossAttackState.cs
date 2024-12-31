public class BossAttackState : BossBaseState
{
    public override void EnterState(BossStateManager stateMachine)
    {
        stateMachine.DisableAllAnimations();
        stateMachine.BossAnimator.SetBool(AnimationStrings.isAttack, true);
        stateMachine.boss.DealDamage();
    }

    public override void ExitState(BossStateManager stateMachine)
    {
        BossEvent.OnBossActionEnd.Invoke();
    }

    public override void FrameUpdate(BossStateManager stateMachine)
    {

    }

    public override void PhysicsUpdate(BossStateManager stateMachine)
    {
    }
}
