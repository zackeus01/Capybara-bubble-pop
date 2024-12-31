using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class Character : MonoBehaviour
{
    #region Stats
    [Header("Stats")]
    [SerializeField]
    protected float currentHp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float attack;
    [SerializeField] protected float elementalAttack;
    [SerializeField] protected float defense;
    [SerializeField] protected float elementalDefense;
    [SerializeField] protected float critRate = 0f;
    [SerializeField] protected float critDamage = DefaultStats.CritDamage;


    public float CurrentHp { get { return currentHp; } set { currentHp = value; UpdateHealthBar(); } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; SetUpHealthBar(); } }
    public float Attack { get { return attack; } set { attack = value; } }
    public float ElementalAttack { get { return elementalAttack; } set { elementalAttack = value; } }
    public float Defense { get { return defense; } set { defense = value; } }
    public float ElementalDefense { get { return elementalDefense; } set { elementalDefense = value; } }
    public float CritRate { get { return critRate; } set { critRate = Mathf.Clamp(value, 0, 1); } }
    public float CritDamage { get { return critDamage; } set { critDamage = value; } }
    #endregion

    #region Mana
    [Header("Mana for Ult")]
    [SerializeField] protected float manaCap;
    protected float currentMana;

    public float ManaCap { get { return manaCap; } set { manaCap = value; } }
    public float CurrentMana
    {
        get
        {
            return currentMana;
        }
        set
        {
            currentMana = Mathf.Clamp(value, 0, manaCap);
            UpdateManaBar();
        }
    }
    #endregion

    #region Shield
    private float shieldHp;
    public float ShieldHp { get { return shieldHp; } set { shieldHp = value; UpdateShieldBar(); } }
    #endregion

    #region Element
    public virtual ElementType AttackElementType { get; set; }
    public virtual ElementType DefenseElementType { get; set; }
    #endregion

    #region Effects
    public EffectStackable AttackBuff { get; set; }
    public EffectStackable AttackDebuff { get; set; }
    public EffectStackable DefenseBuff { get; set; }
    public EffectStackable DefenseDebuff { get; set; }
    public EffectStackable ShieldEffect { get; set; }
    public EffectStackable DotEffect { get; set; }
    public CritEffect CritEffect { get; set; }
    #endregion

    #region Bools
    protected bool isFreezing;
    protected bool isSkillActivated;
    protected bool isHurt;
    protected bool isReceivingEffects;

    public bool IsFreezing { get { return isFreezing; } set { isFreezing = value; } }

    public bool IsSkillActivated
    {
        get
        {
            return isSkillActivated;
        }
        set
        {
            isSkillActivated = value;
            CustomDebug.Log($"IsSkillActivated: {IsSkillActivated}", Color.cyan);
        }
    }

    public bool IsHurt { get { return isHurt; } set { isHurt = value; } }
    public bool IsReceivingEffects { get { return isReceivingEffects; } set { isHurt = isReceivingEffects; } }
    #endregion

    #region UIs
    protected HealthBarController healthBar;
    public HealthBarController HealthBar { get { return healthBar; } }

    protected ShieldBarController shieldBar;
    public ShieldBarController ShieldBar { get { return shieldBar; } }

    protected ManaBarController manaBar;
    public ManaBarController ManaBar { get { return manaBar; } }
    #endregion

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarController>(true);
    }

    protected virtual void Start()
    {
        currentHp = maxHp;
        currentMana = 0;

        isSkillActivated = false;
        isFreezing = false;

        SetUpHealthBar();
        SetUpShieldBar();
        SetUpManaBar();
    }

    #region UI methods
    protected virtual void SetUpHealthBar()
    {
        //healthBar = GetComponentInChildren<HealthBarController>(true);
        //healthBar.SetupHealthBar(maxHp);
    }

    protected virtual void UpdateHealthBar()
    {
        //healthBar.UpdateHealthBar(currentHp);
    }

    protected virtual void SetUpShieldBar()
    {
        //shieldBar = GetComponentInChildren<ShieldBarController>(true);
        //shieldBar.SetupShieldBar(maxHp);
    }
    protected virtual void UpdateShieldBar()
    {
        //shieldBar.UpdateShieldBar(shieldHp);
    }

    protected virtual void SetUpManaBar()
    {
        //manaBar = GetComponentInChildren<ManaBarController>(true);
        //manaBar.SetupManaBar(manaCap);
    }
    protected virtual void UpdateManaBar()
    {
        //manaBar.UpdateManaBar(currentMana);
    }
    #endregion

    #region Hurt/Heal Methods
    public virtual void TakeDamage(float[] inputDamages, ElementType attackElement)
    {
        isHurt = true;


        float totalDefense = defense * (1 + DefenseBuff.TotalAmount) * Mathf.Clamp(1 - DefenseDebuff.TotalAmount, 0.01f, 1f);


        //CustomDebug.LogError("inputDamages base" + inputDamages[0], Color.cyan);
        //CustomDebug.LogError("inputDamages elemetal" + inputDamages[1], Color.cyan);
        //CustomDebug.LogError("totalDefense" + totalDefense, Color.cyan);

        float baseDamage = inputDamages[0] - totalDefense;
        baseDamage = Mathf.Clamp(baseDamage, 1, 100000);

        float elementalDamage = inputDamages[1] * (1 + GetElementalFactor(attackElement)) - elementalDefense;
        elementalDamage = Mathf.Clamp(elementalDamage, 1, 100000);

        float realDamage = baseDamage + elementalDamage;

        //LogEffectStatus();

        CustomDebug.Log($"{GetType().Name}'s Elemental Type: {DefenseElementType}", Color.magenta);
        CustomDebug.Log($"Enemy's Elemental Type: {attackElement}", Color.magenta);
        CustomDebug.Log($"Elemental Factor: {GetElementalFactor(attackElement)}", Color.magenta);

        CustomDebug.Log($"{GetType().Name} receives {baseDamage} Physical Damage", Color.red);
        CustomDebug.Log($"{GetType().Name} receives {elementalDamage} Elemental Damage", Color.red);
        CustomDebug.Log($"{GetType().Name} lost {realDamage} HP", Color.red);

        LoseShieldHpAndHp(realDamage);
    }

    public virtual void TakeHeal(float healAmount)
    {
        //Debug.Log($"<color=lime> {GetType().Name} receives {maxHp * healAmount} HP</color>");
        CurrentHp = Mathf.Clamp(currentHp + (maxHp * healAmount), 0, maxHp);
    }

    public virtual void TakeDotDamage()
    {
        float dotDamage = MaxHp * DefaultStats.EFFECT_DotDamage * DotEffect.CurrentStack;
        CustomDebug.Log($"{GetType().Name} receives {dotDamage} Dot Damage", Color.red);
        LoseShieldHpAndHp(dotDamage);
    }

    protected virtual void LoseShieldHpAndHp(float totalDamage)
    {
        if (totalDamage > 0)
        {
            ShieldHp = Mathf.Max(shieldHp - totalDamage, 0);
            float remainingDamage = totalDamage - shieldHp;
            if (remainingDamage > 0)
            {
                CurrentHp = Mathf.Max(currentHp - remainingDamage, 0);
            }
        }
    }

    public void Die()
    {
        //TO DO
    }
    #endregion

    #region Shield Methods
    protected void SetShieldTotalHp()
    {
        ShieldHp = Mathf.Clamp(ShieldEffect.TotalAmount, 0, maxHp);
    }
    #endregion

    #region Mana Methods
    public virtual void RegenMana(float amount)
    {
        //Debug.Log($"<color=aqua> {GetType().Name} receives {amount} mama</color>");
        CurrentMana = Mathf.Min(currentMana + amount, manaCap);
    }

    public void ResetMana(float amount)
    {
        CurrentMana = 0;
    }
    #endregion

    #region Deal Damage Methods
    public virtual void DealDamage() { }

    public virtual void SkillDamage(float dmgMultiplier) { }

    protected virtual float[] CalculateOutputDamage()
    {
        float[] outputDamages = new float[2];
        float baseDamage;
        float elementalDamage;
        int crit;                                           //= 1 if there's crit, else = 0

        crit = CalculateCrit(critRate + CritEffect.GetCurrentCritRate());

        //Damage
        baseDamage = attack * (1 + AttackBuff.TotalAmount) * Mathf.Clamp(1 - AttackDebuff.TotalAmount, 0.01f, 1f) * (1 + (crit * critDamage));
        elementalDamage = elementalAttack * (1 + AttackBuff.TotalAmount) * Mathf.Clamp(1 - AttackDebuff.TotalAmount, 0.01f, 1f) * (1 + (crit * critDamage));
        //Crit

        //Real Damage
        outputDamages[0] = baseDamage;
        outputDamages[1] = elementalDamage;

        //LogEffectStatus();

        CustomDebug.Log($"Crit: {crit}", Color.cyan);
        CustomDebug.Log($"{GetType().Name} Deals {baseDamage} Physical Damage", Color.cyan);
        CustomDebug.Log($"{GetType().Name} Deals {elementalDamage} Elemental Damage", Color.cyan);

        return outputDamages;
    }

    protected float[] CalculateSkillDamage(float dmgMultiplier)
    {
        float[] outputDamages = new float[2];
        outputDamages[0] = CalculateOutputDamage()[0] * dmgMultiplier;
        outputDamages[1] = CalculateOutputDamage()[1] * dmgMultiplier;
        return outputDamages;
    }

    protected virtual int CalculateCrit(float critRate)
    {
        // Generate a random float between 0 and 1
        float randomValue = Random.value;

        // If the random value is less than the crit rate, a crit happens (crit = 1), otherwise no crit (crit = 0)
        int crit = (randomValue <= critRate) ? 1 : 0;
        return crit;
    }

    protected float GetElementalFactor(ElementType attackerElement)
    {
        float factor = 0f;

        // IF attacker's element counter this one's element => buff 0.5 damage
        if (ElementHelper.IsCounter(attackerElement, DefenseElementType))
        {
            factor = 0.5f;
        }
        //If this one's element resist attacker's element
        else if (ElementHelper.IsCounter(DefenseElementType, attackerElement))
        {
            factor = -0.5f;
        }
        //DEFAULT
        else factor = 0f;

        return factor;
    }
    #endregion

    #region Effect Methods
    protected List<Effect> GetAllEffects()
    {
        List<Effect> currentEffects = new List<Effect>
        {
            AttackBuff,
            AttackDebuff,
            DefenseBuff,
            DefenseDebuff,
            ShieldEffect,
            DotEffect,
            CritEffect
        };
        return currentEffects;
    }

    public virtual void ApplyHeal(int stack)
    {
    }

    public virtual void ApplyManaRegen(int stack)
    {
        RegenMana(stack * DefaultStats.ManaPerBall);
    }

    public void ApplyFreeze()
    {
        isFreezing = true;
    }

    public virtual void ApplyEffects(Dictionary<EffectType, int> effects)
    {
        foreach (KeyValuePair<EffectType, int> effect in effects)
        {
            switch (effect.Key)
            {
                case EffectType.AttackBuff:
                    AttackBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    break;
                case EffectType.AttackDebuff:
                    AttackDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    break;
                case EffectType.DefenseBuff:
                    DefenseBuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    break;
                case EffectType.DefenseDebuff:
                    DefenseDebuff.ApplyNew(DefaultStats.EFFECT_StatEffectDuration, effect.Value);
                    break;
                case EffectType.Shield:
                    ShieldEffect.ApplyNew(DefaultStats.EFFECT_ShieldDuration, effect.Value);
                    SetShieldTotalHp();
                    break;
                case EffectType.DOT:
                    DotEffect.ApplyNew(DefaultStats.EFFECT_DotDuration, effect.Value);
                    break;
                case EffectType.Crit:
                    CritEffect.ApplyNew();
                    break;
            }
        }
    }

    public void EffectPassTurn()
    {
        AttackBuff.PassATurn();
        AttackDebuff.PassATurn();
        DefenseBuff.PassATurn();
        DefenseDebuff.PassATurn();
        ShieldEffect.PassATurn();
        DotEffect.PassATurn();
        CritEffect.PassATurn();

        IsFreezing = false;

        EffectUIUpdate();
    }

    public virtual void EffectUIUpdate() { }
    #endregion

    protected void LogEffectStatus()
    {
        CustomDebug.Log($"{GetType().Name.ToUpper()}'s EFFECTS:", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {AttackBuff.EffectType}: {AttackBuff.CurrentStack} stacks, {AttackBuff.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {AttackDebuff.EffectType}: {AttackDebuff.CurrentStack} stacks, {AttackDebuff.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {DefenseBuff.EffectType}: {DefenseBuff.CurrentStack} stacks, {DefenseBuff.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {DefenseDebuff.EffectType}: {DefenseDebuff.CurrentStack} stacks, {DefenseDebuff.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {ShieldEffect.EffectType}: {ShieldEffect.CurrentStack} stacks, {ShieldEffect.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {DotEffect.EffectType}: {DotEffect.CurrentStack} stacks, {DotEffect.TurnDuration} turns.", Color.green);
        CustomDebug.Log($"{GetType().Name}'s {CritEffect.EffectType}: Stage {CritEffect.CurrentStage}, {CritEffect.TurnDuration} turns.", Color.green);
    }

}

public enum CharacterType
{
    Boss,
    Player
}