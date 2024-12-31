using Spine;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerStateMachine: MonoBehaviour
{
    public PlayerState CurrentState { get; set; }

    #region Bools
    private bool isFreezing;
    private bool isSkillActivated;
    private bool isHurt;
    private bool isReceivingEffects;

    public bool IsFreezing { get { return isFreezing; } set { isFreezing = value; } }

    public bool IsSkillActivated { get { return isSkillActivated; } set { isSkillActivated = value; } }

    public bool IsHurt { get { return isHurt; } set { isHurt = value; } }
    public bool IsReceivingEffects { get { return isReceivingEffects; } set { isHurt = isReceivingEffects; } }
    #endregion

    #region Skills
    private Queue<SkillSO> skillsQueue;
    private SkillSO currentSkill;
    private int skillComboIndex = 0;
    public Queue<SkillSO> SkillsQueue { get { return skillsQueue; } }
    public SkillSO CurrentSkill { get { return currentSkill; } }

    #endregion

    #region Animator
    [SerializeField] private Animator animator;

    public Animator PlayerAnimator { get => animator; }

    private const string _swordLayer = "SwordLayer";
    private const string _bowLayer = "BowLayer";
    private const string _magicLayer = "MagicLayer";
    private const string _defaultLayer = "DefaultLayer";

    private const string _skillStateName = "Skill";
    #endregion

    #region StateMachine
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerReceiveEffectState ReceiveEffectState { get; set; }
    public PlayerHurtState HurtState { get; set; }
    public PlayerAttackState AttackState { get; set; }
    public PlayerFreezeState FreezeState { get; set; }
    public PlayerSkillState SkillState { get; set; }
    #endregion

    private void Awake()
    {
        IdleState = new PlayerIdleState();
        ReceiveEffectState = new PlayerReceiveEffectState();
        HurtState = new PlayerHurtState();
        AttackState = new PlayerAttackState();
        FreezeState = new PlayerFreezeState();
        SkillState = new PlayerSkillState();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        BossEvent.OnPlayerActionStart.AddListener(FinishStandByState);
        BossEvent.OnPlayerHurt.AddListener(EnterHurtState);
        BossEvent.OnPlayerActivateSkill.AddListener(PutSkillInQueue);

        PlayerAnimationEvent.OnAttackAnimationEnd.AddListener(StateCompleted);
        PlayerAnimationEvent.OnSkillAnimationEnd.AddListener(FinishSkill);
        PlayerAnimationEvent.OnBuffAnimationEnd.AddListener(StateCompleted);
        PlayerAnimationEvent.OnHurtAnimationEnd.AddListener(PlayerHurtStop);

        PlayerAnimationEvent.OnHealSkillTrigger.AddListener(SkillHealTrigger);
        PlayerAnimationEvent.OnComboSkillTrigger.AddListener(SkillComboTrigger);
        PlayerAnimationEvent.OnFreezeSkillTrigger.AddListener(SkillFreezeTrigger);

        skillsQueue = new Queue<SkillSO>();

        this.AssignStateTransitions();
        this.SetAnimatorWeapon();
        this.Initialize(IdleState);
    }

    #region Initials
    public void Initialize(PlayerState startingState)
    {
        Debug.Log("Player State Machine Initaialize");
        CurrentState = startingState;
        CurrentState.EnterState(this);
    }

    private void AssignStateTransitions()
    {
        IdleState.Transitions.Add(new Transition(() => isHurt, HurtState));              //Stand By -> Hurt                  : If Player is hurt
        IdleState.Transitions.Add(new Transition(() => isFreezing, FreezeState));        //Stand By -> Freeze                : If Player is freezing

        HurtState.Transitions.Add(new Transition(() => true, IdleState));                //Hurt -> Stand By

        ReceiveEffectState.Transitions.Add(new Transition(() => true, AttackState));        //Receive Effect -> Normal Attack

        AttackState.Transitions.Add(new Transition(() => true, SkillState));    //Normal Attack -> Skill

        SkillState.Transitions.Add(new Transition(() => !isSkillActivated, IdleState));               //Skill -> Stand By
        SkillState.Transitions.Add(new Transition(() => isSkillActivated, SkillState));               //Skill -> Stand By
    }

    private void SetAnimatorWeapon()
    {
        DisableAllWeaponBools();

        WeaponDataSO weapon = EquipmentDataController.Instance.GetEquippedWeapon();
        if(weapon != null)
        {
            switch (weapon.WeaponType)
            {
                case WeaponTypeInGame.Sword:
                    SetBoolAnimator(AnimationStrings.isSword, true);
                    break;
                case WeaponTypeInGame.Bow:
                    SetBoolAnimator(AnimationStrings.isBow, true);
                    break;
                case WeaponTypeInGame.MagicSeal:
                    SetBoolAnimator(AnimationStrings.isMagic, true);
                    break;
                default: break;
            }
        }
       
    }
    #endregion

    #region State methods
    // Called by a state when it completes its actions
    public void StateCompleted()
    {
        // Check for transitions and change state if a valid one is found
        PlayerState nextState = CurrentState.GetNextState();
        if (nextState != null)
        {
            ChangeState(nextState);
        }
    }

    public void EndTurn()
    {
        StateCompleted();
        BossEvent.OnPlayerActionEnd.Invoke();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.ExitState(this);
        CurrentState = newState;
        CurrentState.EnterState(this);
    }

    public void FinishStandByState()
    {
        this.ChangeState(ReceiveEffectState);
    }

    public void EnterHurtState()
    {
        this.ChangeState(HurtState);
    }

    public void FinishSkill()
    {
        DisableAllAnimations();
        skillComboIndex = 0;
        BossUIEvent.OnPlayerFinishSkill.Invoke(currentSkill);

        //UseSkillInQueue(); //Skill at head of queue
        //CustomDebug.Log($"Skill queue: {skillsQueue.Count}", Color.cyan);
        if (skillsQueue.Count <= 0) //No Skills left in Queue
        {
            isSkillActivated = false;
            EndTurn();
        }
        else
        {
            StateCompleted();
        }
    }
    #endregion


    #region Bools methods
    public void PlayerHurtStop()
    {
        isHurt = false;
        StateCompleted();
    }

    #endregion

    #region SKills
    public void PutSkillInQueue(SkillSO skillSO)
    {
        isSkillActivated = true;
        skillsQueue.Enqueue(skillSO);
    }

    //Current Skill is set when enter the Skill state
    public void SetCurrentSkill()
    {
        if (skillsQueue.Count > 0)
        {
            currentSkill = skillsQueue.Dequeue();
        }
    }

    public void SkillHealTrigger()
    {
        BossEvent.OnPlayerUseHealSkill.Invoke(currentSkill.HealAmount);
        BossUIEvent.OnPlayerHealSkillActivated.Invoke();
    }

    public void SkillComboTrigger()
    {
        BossEvent.OnPlayerSkillLand.Invoke(currentSkill.DamageMultipliers[skillComboIndex]);
        skillComboIndex = Mathf.Clamp(skillComboIndex + 1, 0, currentSkill.DamageMultipliers.Count - 1);
    }

    public void SkillFreezeTrigger()
    {
        BossEvent.OnPlayerSkillLand.Invoke(currentSkill.DamageMultipliers[0]);
        BossEvent.OnPlayerUseFreezeSkill.Invoke();
    }
    #endregion

    #region Animations
    public void SetBoolAnimator(string animationBool, bool toBool)
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        animator.SetBool(animationBool, toBool);
    }

    public void DisableAllAnimations()
    {
        foreach (string animationName in AnimationStrings.listPlayerAnimations)
        {
            SetBoolAnimator(animationName, false);
        }
    }

    public void DisableAllWeaponBools()
    {
        foreach (string animationName in AnimationStrings.listPlayerWeaponAnimations)
        {
            SetBoolAnimator(animationName, false);
        }
    }
    #endregion
}
