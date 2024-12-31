using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimation : MonoBehaviour
{
    private static readonly UnityEvent _onSkillAnimationEnd = new UnityEvent();
    public static UnityEvent OnSkillAnimEnd { get { return _onSkillAnimationEnd; } }

    private static readonly UnityEvent _onBuffAnimationEnd = new UnityEvent();
    public static UnityEvent OnBuffAnimEnd { get { return _onBuffAnimationEnd; } }

    private static readonly UnityEvent _onAttackAnimationEnd = new UnityEvent();
    public static UnityEvent OnAttackAnimEnd { get { return _onAttackAnimationEnd; } }

    private static readonly UnityEvent _onHurtAnimationEnd = new UnityEvent();
    public static UnityEvent OnHurtAnimEnd { get { return _onHurtAnimationEnd; } }

    private static readonly UnityEvent _onAttackHit = new UnityEvent();
    public static UnityEvent OnAttackHit { get => _onAttackHit; }


    public void OnDeadAnimationEnd()
    {
        BossEvent.OnGameWin.Invoke();
        GameplayEvent.OnGameWin.Invoke();
    }
    public void OnAtkHit()
    {
        OnAttackHit.Invoke();
    }

    public void OnHurtAnimationEnd()
    {
        OnHurtAnimEnd.Invoke();
    }
    public void OnSkillAnimationEnd()
    {
        OnSkillAnimEnd.Invoke();
    }

    public void OnBuffAnimationEnd()
    {
        OnBuffAnimEnd.Invoke();
    }

    public void OnAttackAnimationEnd()
    {
        OnAttackAnimEnd.Invoke();
    }
}
