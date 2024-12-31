using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    #region Weapon/Armor
    public WeaponDataSO Weapon => EquipmentDataController.Instance.GetEquippedWeapon();
    public ArmorDataSO Armor => EquipmentDataController.Instance.GetEquippedArmor();

    [Header("Weapon")]
    [SerializeField] private SpriteRenderer swordSlot;
    [SerializeField] private SpriteRenderer bowSlot;
    [SerializeField] private SpriteRenderer magicSealSlot;

    [Header("Armor")]
    [SerializeField] private SpriteRenderer headArmorSlot;
    [SerializeField] private SpriteRenderer bodyArmorSlot;
    [SerializeField] private SpriteRenderer LArmArmorSlot;
    [SerializeField] private SpriteRenderer RArmArmorSlot;
    #endregion

    #region Element
    public override ElementType AttackElementType => Weapon? Weapon.ElementType : ElementType.None;
    public override ElementType DefenseElementType => Armor? Armor.ElementType : ElementType.None;
    #endregion

    #region Skills
    [Header("Skills")]
    [SerializeField] private List<SkillSO> skills;
    public List<SkillSO> Skills { get { return skills; } }
    #endregion

    protected override void Start()
    {
        SetSpriteEquipment();
        LoadstatsFromData();
        AddEquipmentStats();
        InitializeEffects();

        currentHp = maxHp;
        currentMana = 0;

        isSkillActivated = false;
        isFreezing = false;

        BossEvent.OnBossAttack.AddListener(TakeDamage);
        BossEvent.OnPlayerReceiveEffect.AddListener(ApplyEffects);
        BossEvent.OnPlayerShootBallStart.AddListener(EffectPassTurn);
        BossEvent.OnPlayerShootBallStart.AddListener(CheckDotDamage);
        BossEvent.OnPlayerActivateSkill.AddListener(ActivateSkill);
        BossEvent.OnPlayerSkillLand.AddListener(SkillDamage);

        BossEvent.OnPlayerRegenMana.AddListener(ApplyManaRegen);
        BossEvent.OnPlayerHeal.AddListener(ApplyHeal);

        BossEvent.OnPlayerUseHealSkill.AddListener(TakeHeal);

        PlayerAnimationEvent.OnAttackHitBoss.AddListener(DealDamage);

        SetUpSkillUIs();

        SetUpHealthBar();
        SetUpShieldBar();
        SetUpManaBar();
    }

    #region Initials
    private void SetSpriteEquipment()
    {
        swordSlot.gameObject.SetActive(false);
        bowSlot.gameObject.SetActive(false);
        magicSealSlot.gameObject.SetActive(false);

        headArmorSlot.gameObject.SetActive(false);
        bodyArmorSlot.gameObject.SetActive(false);
        LArmArmorSlot.gameObject.SetActive(false);
        RArmArmorSlot.gameObject.SetActive(false);

        if (Weapon != null)
        {
            switch (Weapon.WeaponType)
            {
                case WeaponTypeInGame.Sword:
                    swordSlot.sprite = Weapon.Avatar;
                    swordSlot.gameObject.SetActive(true);
                    break;
                case WeaponTypeInGame.Bow:
                    bowSlot.sprite = Weapon.Avatar;
                    bowSlot.gameObject.SetActive(true);
                    break;
                case WeaponTypeInGame.MagicSeal:
                    magicSealSlot.sprite = Weapon.Avatar;
                    magicSealSlot.gameObject.SetActive(true);
                    break;
            }
        }

        if (Armor != null)
        {
            headArmorSlot.sprite = Armor.Head;
            headArmorSlot.gameObject.SetActive(true);

            bodyArmorSlot.sprite = Armor.Body;
            bodyArmorSlot.gameObject.SetActive(true);

            LArmArmorSlot.sprite = Armor.LArm;
            LArmArmorSlot.gameObject.SetActive(true);

            RArmArmorSlot.sprite = Armor.RArm;
            RArmArmorSlot.gameObject.SetActive(true);
        }
    }

    private void LoadstatsFromData()
    {
        if (PlayerSavecontroller.Instance == null)
        {
            Debug.LogError("Player Data Collection is NULL");
        }
        else
        {
            MaxHp = PlayerSavecontroller.Instance.PlayerDataCollection.CurrenHp;
            Attack = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentAtk;
            Defense = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentDef;
            ElementalAttack = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentElDame;
            ElementalDefense = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentRes;
            CritRate = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentCrit;
            ManaCap = PlayerSavecontroller.Instance.PlayerDataCollection.CurrentMana;
        }
    }

    private void AddEquipmentStats()
    {
        if (Weapon != null)
        {
            Attack += Weapon.BaseWeaponATK;
            ElementalAttack += Weapon.BaseWeaponElementATK;
            CritRate += Weapon.BaseWeaponCrit;
        }
        if (Armor != null)
        {
            MaxHp += Armor.BaseArmorHP;
            CurrentHp = MaxHp;
            Defense += Armor.BaseArmorDEF;
            ElementalDefense += Armor.BaseArmorElementRes;
        }
    }

    private void InitializeEffects()
    {
        AttackBuff = new EffectStackable(EffectType.AttackBuff, DefaultStats.PLAYER_AttackBuff);
        AttackDebuff = new EffectStackable(EffectType.AttackDebuff, DefaultStats.PLAYER_AttackDebuff);
        DefenseBuff = new EffectStackable(EffectType.DefenseBuff, DefaultStats.PLAYER_DefenseBuff);
        DefenseDebuff = new EffectStackable(EffectType.DefenseDebuff, DefaultStats.PLAYER_DefenseDebuff);
        ShieldEffect = new EffectStackable(EffectType.Shield, DefaultStats.PLAYER_ShieldAmount);
        DotEffect = new EffectStackable(EffectType.DOT, DefaultStats.EFFECT_DotDamage);
        CritEffect = new CritEffect(EffectType.Crit, 1);
    }
    #endregion

    #region Overrides
    public override void TakeHeal(float healAmount)
    {
        base.TakeHeal(healAmount);
        BossUIEvent.OnPlayerHealActivated.Invoke();
    }

    public override void TakeDamage(float[] inputDamages, ElementType attackElement)
    {
        base.TakeDamage(inputDamages, attackElement);

        if (CurrentHp <= 0)
        {
            GameplayEvent.OnGameOver.Invoke();
        }

        BossEvent.OnPlayerHurt.Invoke();
    }

    public override void TakeDotDamage()
    {
        base.TakeDotDamage();
        BossEvent.OnPlayerHurt.Invoke();
    }

    public override void DealDamage()
    {
        base.DealDamage();
        BossEvent.OnPlayerAttack.Invoke(CalculateOutputDamage(), AttackElementType);
    }

    public override void SkillDamage(float dmgMultipler)
    {
        base.SkillDamage(dmgMultipler);
        BossEvent.OnPlayerAttack.Invoke(CalculateSkillDamage(dmgMultipler), AttackElementType);
    }

    public override void ApplyHeal(int stack)
    {
        base.ApplyHeal(stack);
        TakeHeal(stack * DefaultStats.PLAYER_HealAmount);
    }

    public override void ApplyEffects(Dictionary<EffectType, int> effects)
    {
        foreach (KeyValuePair<EffectType, int> effect in effects)
        {
            switch (effect.Key)
            {
                case EffectType.AttackBuff:
                    AttackBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.PlayerAttackBuff.UpdateUI(AttackBuff.TurnDuration, AttackBuff.CurrentStack);
                    break;
                case EffectType.AttackDebuff:
                    AttackDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.PlayerAttackDebuff.UpdateUI(AttackDebuff.TurnDuration, AttackDebuff.CurrentStack);
                    break;
                case EffectType.DefenseBuff:
                    DefenseBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.PlayerDefenseBuff.UpdateUI(DefenseBuff.TurnDuration, DefenseBuff.CurrentStack);
                    break;
                case EffectType.DefenseDebuff:
                    DefenseDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    EffectUIManager.Instance.PlayerDefenseDebuff.UpdateUI(DefenseDebuff.TurnDuration, DefenseBuff.CurrentStack);
                    break;
                case EffectType.Shield:
                    ShieldEffect.ApplyNew(DefaultStats.EFFECT_ShieldDuration, effect.Value);
                    SetShieldTotalHp();
                    EffectUIManager.Instance.PlayerShieldEffect.UpdateUI(ShieldEffect.TurnDuration, ShieldEffect.CurrentStack);
                    break;
                case EffectType.DOT:
                    DotEffect.ApplyNew(DefaultStats.EFFECT_DotDuration, effect.Value);
                    EffectUIManager.Instance.PlayerDotEffect.UpdateUI(DotEffect.TurnDuration, DotEffect.CurrentStack);
                    break;
                case EffectType.Crit:
                    CritEffect.ApplyNew();
                    EffectUIManager.Instance.PlayerCritEffect.UpdateUI(CritEffect.TurnDuration, CritEffect.CurrentStage);
                    break;
            }
        }
    }

    public override void EffectUIUpdate()
    {
        base.EffectUIUpdate();
        BossUIEvent.OnPlayerUpdateEffects.Invoke(GetAllEffects());
    }

    #endregion

    #region Bar UI
    protected override void SetUpHealthBar()
    {
        base.SetUpHealthBar();
        BossUIEvent.OnPlayerHealthSetUp.Invoke(MaxHp);
    }

    protected override void SetUpShieldBar()
    {
        base.SetUpShieldBar();
        BossUIEvent.OnPlayerShieldSetUp.Invoke(MaxHp);
    }

    protected override void SetUpManaBar()
    {
        base.SetUpManaBar();
        BossUIEvent.OnPlayerManaSetUp.Invoke(ManaCap);
    }

    protected override void UpdateHealthBar()
    {
        base.UpdateHealthBar();
        BossUIEvent.OnPlayerHealthUpdate.Invoke(currentHp);
    }

    protected override void UpdateShieldBar()
    {
        base.UpdateShieldBar();
        BossUIEvent.OnPlayerShieldUpdate.Invoke(ShieldHp);
    }

    protected override void UpdateManaBar()
    {
        base.UpdateManaBar();
        BossUIEvent.OnPlayerManaUpdate.Invoke(currentMana);
    }
    #endregion

    #region Skill
    protected void SetUpSkillUIs()
    {
        BossUIEvent.OnPlayerSkillUISetup.Invoke(skills);
    }

    public void ActivateSkill(SkillSO skillSO)
    {
        CustomDebug.Log("Activate Skill!", Color.cyan);
        CurrentMana = Mathf.Max(currentMana - skillSO.ManaCost, 0);
    }

    #endregion

    #region DOT
    public void CheckDotDamage()
    {
        if (DotEffect.TurnDuration > 0)
        {
            TakeDotDamage();
        }
    }

    #endregion
}

