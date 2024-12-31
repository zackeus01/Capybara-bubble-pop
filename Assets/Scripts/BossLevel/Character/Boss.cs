using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    #region Element
    [Header("Element")]
    [SerializeField] private ElementType element;
    public ElementType Element { get { return element; } set { element = value; } }
    public override ElementType AttackElementType => element;
    public override ElementType DefenseElementType => element;
    #endregion

    #region Cache Component
    [Header("Component")]
    [SerializeField]
    private BossStateManager stateManager;
    [SerializeField]
    private DragonCtrl dragonCtrl;
    #endregion

    private bool isRage = false;

    protected override void Start()
    {
        base.Start();
        this.LoadComponent();

        BossStatMutiplier();

        AttackBuff = new EffectStackable(EffectType.AttackBuff, DefaultStats.BOSS_AttackBuff);
        AttackDebuff = new EffectStackable(EffectType.AttackDebuff, DefaultStats.BOSS_AttackDebuff);
        DefenseBuff = new EffectStackable(EffectType.DefenseBuff, DefaultStats.BOSS_DefenseBuff);
        DefenseDebuff = new EffectStackable(EffectType.DefenseDebuff, DefaultStats.BOSS_DefenseDebuff);
        ShieldEffect = new EffectStackable(EffectType.Shield, DefaultStats.BOSS_ShieldAmount);
        DotEffect = new EffectStackable(EffectType.DOT, DefaultStats.EFFECT_DotDamage);
        CritEffect = new CritEffect(EffectType.Crit, 1);

        BossEvent.OnPlayerAttack.AddListener(TakeDamage);
        BossEvent.OnBossShootBallStart.AddListener(EffectPassTurn);
        BossEvent.OnBossReceiveEffect.AddListener(ApplyEffects);
        BossEvent.OnBossRage.AddListener(SelfEffectApplied);
        BossEvent.OnBossRegenMana.AddListener(ApplyManaRegen);
        BossEvent.OnBossHeal.AddListener(ApplyHeal);
    }

    private void BossStatMutiplier()
    {
        float difficulty = LevelDataHolder.LevelData.BossDifficulty;

        MaxHp += MaxHp * (0.25f * difficulty);
        Attack += Attack * (0.2f * difficulty);
        Defense += Defense * (0.25f * difficulty);
    }

    private void LoadComponent()
    {
        stateManager = this.GetComponent<BossStateManager>();
        dragonCtrl = this.GetComponent<DragonCtrl>();
    }

    #region Overrides
    public override void TakeDamage(float[] inputDamages, ElementType attackElement)
    {
        //base.TakeDamage(inputDamages, attackElement);
        isHurt = true;

        float totalDefense = defense * (1 + DefenseBuff.TotalAmount) * (1 - DefenseDebuff.TotalAmount);

        float baseDamage = (inputDamages[0] - totalDefense) * dragonCtrl.PhysicDmgReceiveDivisor;
        float elementalDamage = (inputDamages[1] * (1 + GetElementalFactor(attackElement)) - elementalDefense) * dragonCtrl.ElementalDmgReceiveDivisor;
        float realDamage = (baseDamage + elementalDamage) * dragonCtrl.TotalDmgReceiveDivisor;

        //Debug.Log($"<color=white> {GetType().Name} receives {baseDamage} Physical Damage</color>");
        //Debug.Log($"<color=purple> {GetType().Name} receives {elementalDamage} Elemental Damage</color>");
        //Debug.Log($"<color=red> {GetType().Name} lost {realDamage} HP</color>");

        LoseShieldHpAndHp(realDamage);

        BossEvent.OnBossHurt.Invoke();

        if (currentHp <= MaxHp * 0.5f && !isRage)
        {
            isRage = true;
            BossEvent.OnBossRage.Invoke();
        }

        if (currentHp <= 0)
        {
            //GameplayEvent.OnGameWin.Invoke();
            //BossEvent.OnGameWin.Invoke();

            BossEvent.OnBossDead.Invoke();
        }
    }

    public override void ApplyHeal(int stack)
    {
        base.ApplyHeal(stack);
        TakeHeal(stack * DefaultStats.BOSS_HealAmount * dragonCtrl.HealMultiplier);
        BossUIEvent.OnBossHealActivated.Invoke();
    }

    public override void ApplyEffects(Dictionary<EffectType, int> effects)
    {
        foreach (KeyValuePair<EffectType, int> effect in effects)
        {
            switch (effect.Key)
            {
                case EffectType.AttackBuff:
                    AttackBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.BossAttackBuff.UpdateUI(AttackBuff.TurnDuration, AttackBuff.CurrentStack);
                    break;
                case EffectType.AttackDebuff:
                    AttackDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.BossAttackDebuff.UpdateUI(AttackDebuff.TurnDuration, AttackDebuff.CurrentStack);
                    break;
                case EffectType.DefenseBuff:
                    DefenseBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.BossDefenseBuff.UpdateUI(DefenseBuff.TurnDuration, DefenseBuff.CurrentStack);
                    break;
                case EffectType.DefenseDebuff:
                    DefenseDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.BossDefenseDebuff.UpdateUI(DefenseDebuff.TurnDuration, DefenseBuff.CurrentStack);
                    break;
                case EffectType.Shield:
                    ShieldEffect.ApplyNew(DefaultStats.EFFECT_ShieldDuration, effect.Value);
                    SetShieldTotalHp();
                    EffectUIManager.Instance.BossShieldEffect.UpdateUI(ShieldEffect.TurnDuration, ShieldEffect.CurrentStack);
                    break;
                case EffectType.DOT:
                    DotEffect.ApplyNew(DefaultStats.EFFECT_DotDuration, effect.Value);
                    EffectUIManager.Instance.BossDotEffect.UpdateUI(DotEffect.TurnDuration, DotEffect.CurrentStack);
                    break;
                case EffectType.Crit:
                    CritEffect.ApplyNew();
                    EffectUIManager.Instance.BossCritEffect.UpdateUI(CritEffect.TurnDuration, CritEffect.CurrentStage);
                    break;
            }
        }
    }

    public override void EffectUIUpdate()
    {
        base.EffectUIUpdate();
        BossUIEvent.OnBossUpdateEffects.Invoke(GetAllEffects());
    }

    public override void DealDamage()
    {
        base.DealDamage();
        BossEvent.OnBossAttack.Invoke(CalculateOutputDamage(), AttackElementType);
    }

    public override void RegenMana(float amount)
    {
        CurrentMana = Mathf.Min(currentMana + amount * dragonCtrl.ManaMultiplier, manaCap);
    }

    protected override float[] CalculateOutputDamage()
    {
        float[] outputDamages = new float[2];
        float baseDamage;
        float elementalDamage;
        int crit;                                           //= 1 if there's crit, else = 0

        //Debug.Log($"<color=pink>Current crit increase {CritEffect.GetCurrentCritRate()}</color>");
        crit = CalculateCrit(critRate + CritEffect.GetCurrentCritRate() + dragonCtrl.CritBonus);
        //Debug.Log($"<color=pink>Crit? {crit}.");

        //Physic Damage
        baseDamage = attack //Base physic atk
            * (1 + AttackBuff.TotalAmount) * Mathf.Clamp(1 - AttackDebuff.TotalAmount, 0.01f, 1f) //Effect multiplier
            * (1 + (crit * critDamage)) //Crit multiplier
            * dragonCtrl.PhysicDmgMultiplier * dragonCtrl.TotalDmgMultiplier; //Rage multiplier

        //Elemental Damage
        elementalDamage = elementalAttack //Base elemental atk
            * (1 + AttackBuff.TotalAmount) * Mathf.Clamp(1 - AttackDebuff.TotalAmount, 0.01f, 1f) //Effect multiplier
            * (1 + (crit * critDamage)) //Crit multiplier
            * dragonCtrl.ElementalDmgMultiplier * dragonCtrl.TotalDmgMultiplier; //Rage multiplier

        //Real Damage
        outputDamages[0] = baseDamage;
        outputDamages[1] = elementalDamage;

        //CustomDebug.Log("Boss Deal base damage" + baseDamage, Color.cyan);
        //CustomDebug.Log("Boss Deal elemental damage" + elementalDamage, Color.cyan);

        return outputDamages;
    }
    #endregion

    #region Bar UI
    protected override void SetUpHealthBar()
    {
        base.SetUpHealthBar();
        BossUIEvent.OnBossHealthSetUp.Invoke(MaxHp);
    }

    protected override void SetUpShieldBar()
    {
        base.SetUpShieldBar();
        BossUIEvent.OnBossShieldSetUp.Invoke(MaxHp);
    }

    protected override void SetUpManaBar()
    {
        base.SetUpManaBar();
        BossUIEvent.OnBossManaSetUp.Invoke(ManaCap);
    }

    protected override void UpdateHealthBar()
    {
        base.UpdateHealthBar();
        BossUIEvent.OnBossHealthUpdate.Invoke(currentHp);
    }

    protected override void UpdateShieldBar()
    {
        base.UpdateShieldBar();
        BossUIEvent.OnBossShieldUpdate.Invoke(ShieldHp);
    }

    protected override void UpdateManaBar()
    {
        base.UpdateManaBar();
        BossUIEvent.OnBossManaUpdate.Invoke(currentMana);
    }
    #endregion

    private void SelfEffectApplied()
    {

    }

}
