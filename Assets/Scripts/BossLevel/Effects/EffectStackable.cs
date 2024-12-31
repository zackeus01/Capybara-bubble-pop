using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStackable : Effect
{
    protected float amountPerStack;
    protected int currentStack;

    public virtual int CurrentStack { get { return currentStack; } set { currentStack = value; } }

    public float TotalAmount { get { return IsInEffect * amountPerStack * currentStack; } } //Total Buff/Debuff amount

    public EffectStackable(EffectType effectType, float amountPerStack) : base(effectType)
    {
        this.amountPerStack = amountPerStack;
        this.currentStack = 0;
    }

    public void ApplyNew(int turnDuration, int stack)
    {
        this.turnDuration = turnDuration;
        if (stack > currentStack)
        {
            this.currentStack = stack;
        }
    }
}
