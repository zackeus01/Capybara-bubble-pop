using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameState 
{
    public virtual void EnterState(GameStateManager stateMachine) { }
    public virtual void ExitState(GameStateManager stateMachine) { }
    public virtual void FrameUpdate(GameStateManager stateMachine) { }
    public virtual void PhysicsUpdate(GameStateManager stateMachine) { }
    public virtual string GetName() {
        return "BaseState";
    }
}
