public abstract class BossBaseState
{
    public abstract void EnterState(BossStateManager stateMachine);
    public abstract void ExitState(BossStateManager stateMachine);
    public abstract void FrameUpdate(BossStateManager stateMachine);
    public abstract void PhysicsUpdate(BossStateManager stateMachine);
}
