using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Func<bool> Condition;        // Condition that triggers the transition
    public PlayerState ToState;      // Target state to transition to

    public Transition(Func<bool> condition, PlayerState toState)
    {
        Condition = condition;
        ToState = toState;
    }
}
