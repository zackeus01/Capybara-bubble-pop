using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public List<Transition> Transitions = new List<Transition>();

    public virtual void EnterState(PlayerStateMachine stateMachine)
    {
        Debug.Log ($"<color=yellow>PLAYER Enter State: {this.GetType().Name} </color>");
    }

    public virtual void ExitState(PlayerStateMachine stateMachine)
    {
        Debug.Log($"<color=yellow>PLAYER Exit State: {this.GetType().Name} </color>");
    }

    // Checks for valid transitions based on conditions
    public PlayerState GetNextState()
    {
        foreach (var transition in Transitions)
        {
            if (transition.Condition())
            {
                return transition.ToState;
            }
        }
        return null;
    }
}
