using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    public Animator BossAnimator { get => _animator; }

    [SerializeField]
    public GameObject bossGFX;

    private List<Ball> visibleBalls = new List<Ball>();
    public List<Ball> VisibleBalls { get => visibleBalls; }

    #region Cache Component
    [Header("Components")]
    [SerializeField]
    private Transform mainCam;
    [SerializeField]
    public Character boss;
    [SerializeField]
    public DragonCtrl dragonCtrl;
    #endregion

    #region StateMachine
    public BossBaseState currentBossState;
    public BossIdleState idleState = new BossIdleState();
    public BossAttackState attackState = new BossAttackState();
    public BossRageState rageState = new BossRageState();
    public BossReceiveEffectState buffState = new BossReceiveEffectState();
    public BossUltimateState ultimateState = new BossUltimateState();
    public BossSkillState skillState = new BossSkillState(); //Shoot ball state
    public BossEntryState entryState = new BossEntryState();
    public BossHurtState hurtState = new BossHurtState();
    public BossDeathState deathState = new BossDeathState();
    #endregion

    #region StateBool
    private bool isHurt;
    private bool isDead;
    public bool IsHurt
    {
        get { return isHurt; }
        set { isHurt = value; }
    }


    private bool isRage;
    public bool IsRage
    {
        get { return isRage; }
        set { isRage = value; }
    }
    #endregion

    private void Reset()
    {
        this.LoadComponent();
    }

    private void Awake()
    {
        this.AddEventListener();
    }

    private void OnDestroy()
    {
        this.RemoveEventListner();

    }

    private void Update()
    {
        if (currentBossState == null) return;
        currentBossState.FrameUpdate(this);
    }

    private void FixedUpdate()
    {
        if (currentBossState == null) return;
        currentBossState.PhysicsUpdate(this);
    }

    private void SpawnBoss()
    {
        InitializeState(entryState);
    }

    private void AddEventListener()
    {
        GameplayEvent.OnGameFieldSetupDone.AddListener(SpawnBoss);
        BossEvent.OnBossShootBallStart.AddListener(ToSkillState);
        //Need Fixed
        GameplayEvent.OnGameFieldChanged.AddListener(GetVisibleBalls);

        BossEvent.OnBossActionStart.AddListener(ToBossBuffState);
        //To idle and wait for game state manager
        BossAnimation.OnSkillAnimEnd.AddListener(FromSkillToIdle);
        //Enter action boss state
        BossAnimation.OnBuffAnimEnd.AddListener(ToAttackState);
        BossAnimation.OnAttackAnimEnd.AddListener(ToIdleState);
        BossAnimation.OnHurtAnimEnd.AddListener(ToIdleState);
        BossEvent.OnBossDead.AddListener(ToDeathState);
        PlayerAnimationEvent.OnAttackHit.AddListener(ToHurtState);
    }

    private void RemoveEventListner()
    {
        GameplayEvent.OnGameFieldSetupDone.RemoveListener(SpawnBoss);
    }

    #region Initialize and switch state
    public void InitializeState(BossEntryState state)
    {
        currentBossState = state;
        Debug.Log("Enter state:" + currentBossState);
        currentBossState.EnterState(this);
    }

    public void SwitchState(BossBaseState newState)
    {
        currentBossState.ExitState(this);
        CustomDebug.Log("Boss/Exit state: " + currentBossState, Color.magenta);
        //Debug.Log("<color=white>Boss/Exit state:</color> " + currentBossState);
        currentBossState = newState;
        CustomDebug.Log("Boss/Enter state: " + currentBossState, Color.magenta);
        //Debug.Log("<color=white>Boss/Enter state:</color> " + currentBossState);
        currentBossState.EnterState(this);
    }
    #endregion

    #region Event listener switch state

    private void ToDeathState()
    {
        isDead = true;
        SwitchState(deathState);
    }
    private void ToHurtState()
    {
        if (isDead) return;
        CustomDebug.Log("Boss enter hurt stage", Color.red);
        //isHurt = true;

        //Switch in the idle frame update
        SwitchState(hurtState);
    }
    private void ToIdleState()
    {
        if (isDead) return;
        
        SwitchState(idleState);
    }

    private void FromSkillToIdle()
    {
        if (isDead) return;

        if (currentBossState == skillState)
        {
            SwitchState(idleState);
        }
    }

    private void ToBossBuffState()
    {
        if (isDead) return;

        SwitchState(buffState);
    }

    private void ToAttackState()
    {
        if (isDead) return;

        SwitchState(attackState);
    }

    private void ToSkillState()
    {
        if (isDead) return;

        SwitchState(skillState);
    }

    #endregion

    #region Animator
    public void DisableAllAnimations()
    {
        foreach (string animationName in AnimationStrings.listBossAnimations)
        {
            _animator.SetBool(animationName, false);
        }
    }
    #endregion

    private void GetVisibleBalls(List<Ball> ballsOnField)
    {
        visibleBalls.Clear();

        foreach (Ball ball in ballsOnField)
        {
            float ballPosY = ball.transform.position.y;
            if (Mathf.Abs(ball.transform.position.y - mainCam.position.y) <= 15)
            {
                visibleBalls.Add(ball);
            }
        }
    }

    private void LoadComponent()
    {
        bossGFX = this.transform.GetChild(0).gameObject;
        _animator = bossGFX.GetComponentInChildren<Animator>();
        mainCam = Camera.main.transform;
        boss = this.GetComponent<Character>();
        dragonCtrl = this.GetComponent<DragonCtrl>();
    }
}
