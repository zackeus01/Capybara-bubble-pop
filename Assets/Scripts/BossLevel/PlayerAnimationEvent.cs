using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvent : MonoBehaviour
{
    private static readonly UnityEvent _onAnimationEnd = new UnityEvent();
    public static UnityEvent OnAnimationEnd { get { return _onAnimationEnd; } }

    public void EndAnimation()
    {
        OnAnimationEnd.Invoke();
    }

    private static readonly UnityEvent _onAttackAnimationEnd = new UnityEvent();
    private static readonly UnityEvent _onSkillAnimationEnd = new UnityEvent();
    private static readonly UnityEvent _onHurtAnimationEnd = new UnityEvent();
    private static readonly UnityEvent _onBuffAnimationEnd = new UnityEvent();

    private static readonly UnityEvent _onAttackHitBoss = new UnityEvent();
    private static readonly UnityEvent _onHealSkillTrigger = new UnityEvent();
    private static readonly UnityEvent _onComboSkillTrigger = new UnityEvent();
    private static readonly UnityEvent _onFreezeSkillTrigger = new UnityEvent();

    public static UnityEvent OnAttackAnimationEnd {  get { return _onAttackAnimationEnd; } }
    public static UnityEvent OnSkillAnimationEnd {  get { return _onSkillAnimationEnd; } }
    public static UnityEvent OnHurtAnimationEnd {  get { return _onHurtAnimationEnd; } }
    public static UnityEvent OnBuffAnimationEnd {  get { return _onBuffAnimationEnd; } }
    public static UnityEvent OnAttackHitBoss {  get { return _onAttackHitBoss; } }
    public static UnityEvent OnHealSkillTrigger {  get { return _onHealSkillTrigger; } }
    public static UnityEvent OnComboSkillTrigger {  get { return _onComboSkillTrigger; } }
    public static UnityEvent OnFreezeSkillTrigger {  get { return _onFreezeSkillTrigger; } }

    private static readonly UnityEvent _onAttackHit = new UnityEvent();
    public static UnityEvent OnAttackHit { get => _onAttackHit; }

    public void OnAtkHit()
    {
        OnAttackHit.Invoke();
    }

    //Animation event : Deal skill damage to boss
    public void AttackHitBoss()
    {
        OnAttackHitBoss.Invoke();
    }

    //Animation event : Deal skill damage to boss
    public void SkillComboHitBoss()
    {
        OnComboSkillTrigger.Invoke();
    }

    //Animation event : Heal player
    public void HealPlayer()
    {
        OnHealSkillTrigger.Invoke();
    }

    //Animation event : Freeze boss
    public void FreezeBoss()
    {
        OnFreezeSkillTrigger.Invoke();
    }
}
