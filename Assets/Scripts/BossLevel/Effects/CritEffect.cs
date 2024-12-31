using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritEffect :Effect
{
    private int currentStage;

    public int CurrentStage { get { return currentStage; }  set { currentStage = Mathf.Clamp(value, 1, DefaultStats.MaxCritStage); } }

    public CritEffect(EffectType effectType, int currentStage) : base(effectType)
    {
        this.currentStage = currentStage;
    }

    public float GetCurrentCritRate()
    {
        switch (currentStage)
        {
            case 1:
                return DefaultStats.CritRateStage1;
            case 2:
                return DefaultStats.CritRateStage2;
            case 3:
                return DefaultStats.CritRateStage3;
            case 4:
                return DefaultStats.CritRateStage4;
            default: return 0f;
        }
    }

    public void IncreaseStage()
    {
        currentStage = Mathf.Clamp(currentStage + 1, 1, DefaultStats.MaxCritStage);
    }

    public void ApplyNew()
    {
        this.turnDuration = DefaultStats.CritBuffDuration;
        IncreaseStage();
    }

    public override void PassATurn()
    {
        base.PassATurn();
        if (turnDuration <= 0)
        {
            currentStage = 1;
        }
    }
}
