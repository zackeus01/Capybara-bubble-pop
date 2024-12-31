
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Effect : MonoBehaviour
{
    protected EffectType effectType;

    protected int turnDuration;

    public EffectType EffectType { get { return effectType; } }
    public int TurnDuration { get { return turnDuration; } set { turnDuration = value; } }
    public int IsInEffect { get { return (turnDuration > 0 ? 1 : 0); } }

    public Effect(EffectType effectType)
    {
        this.effectType = effectType;
        this.turnDuration = 0;
    }

    public virtual void PassATurn()
    {
        turnDuration = Mathf.Max(turnDuration - 1, 0);
    }
}


