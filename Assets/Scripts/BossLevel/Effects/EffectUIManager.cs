using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectUIManager : Singleton<EffectUIManager>
{
    [Header("Player Effect UIs")]
    [SerializeField] private EffectUI playerAttackBuff;
    [SerializeField] private EffectUI playerAttackDebuff;
    [SerializeField] private EffectUI playerDefenseBuff;
    [SerializeField] private EffectUI playerDefenseDebuff;
    [SerializeField] private EffectUI playerShieldEffect;
    [SerializeField] private EffectUI playerDotEffect;
    [SerializeField] private EffectUI playerCritEffect;

    [Header("Boss Effect UIs")]
    [SerializeField] private EffectUI bossAttackBuff;
    [SerializeField] private EffectUI bossAttackDebuff;
    [SerializeField] private EffectUI bossDefenseBuff;
    [SerializeField] private EffectUI bossDefenseDebuff;
    [SerializeField] private EffectUI bossShieldEffect;
    [SerializeField] private EffectUI bossDotEffect;
    [SerializeField] private EffectUI bossCritEffect;

    [Header("Boss Skill UIs")]
    [SerializeField] private EffectUI bossAttackSkill;
    [SerializeField] private EffectUI bossDefenseSkill;
    [SerializeField] private EffectUI bossCritSkill;
    [SerializeField] private EffectUI bossHealSkill;
    public EffectUI PlayerAttackBuff { get { return playerAttackBuff; } }
    public EffectUI PlayerAttackDebuff { get { return playerAttackDebuff; } }
    public EffectUI PlayerDefenseBuff { get { return playerDefenseBuff; } }
    public EffectUI PlayerDefenseDebuff { get { return playerDefenseDebuff; } }
    public EffectUI PlayerShieldEffect { get { return playerShieldEffect; } }
    public EffectUI PlayerDotEffect { get { return playerDotEffect; } }
    public EffectUI PlayerCritEffect { get { return playerCritEffect; } }

    public EffectUI BossAttackBuff { get { return bossAttackBuff; } }
    public EffectUI BossAttackDebuff { get { return bossAttackDebuff; } }
    public EffectUI BossDefenseBuff { get { return bossDefenseBuff; } }
    public EffectUI BossDefenseDebuff { get { return bossDefenseDebuff; } }
    public EffectUI BossShieldEffect { get { return bossShieldEffect; } }
    public EffectUI BossDotEffect { get { return bossDotEffect; } }
    public EffectUI BossCritEffect { get { return bossCritEffect; } }

    private void Start()
    {
        ResetAllUI();

        BossUIEvent.OnPlayerUpdateEffects.AddListener(UpdatePlayerEffectUIs);
        BossUIEvent.OnBossUpdateEffects.AddListener(UpdateBossEffectUIs);
        BossEvent.OnBossSkillBuff.AddListener(EnableBossSkillUIs);
    }

    private void EnableBossSkillUIs(List<Effect> list)
    {
        foreach (Effect effect in list)
        {
            switch (effect.EffectType)
            {
                case EffectType.AttackBuff:
                    bossAttackSkill.PlayEntryAnimation();
                    //Debug.LogError("bossAttackSkill");
                    break;
                case EffectType.DefenseBuff:
                    bossDefenseSkill.PlayEntryAnimation();
                    //Debug.LogError("bossDefenseSkill");
                    break;
                case EffectType.Crit:
                    bossCritSkill.PlayEntryAnimation();
                    //Debug.LogError("bossCritSkill");
                    break;
            }
        }
    }

    private void ResetAllUI()
    {
        playerAttackBuff.HideUI();
        playerAttackDebuff.HideUI();
        playerDefenseBuff.HideUI();
        playerDefenseDebuff.HideUI();
        playerShieldEffect.HideUI();
        playerDotEffect.HideUI();
        playerCritEffect.HideUI();

        bossAttackBuff.HideUI();
        bossAttackDebuff.HideUI();
        bossDefenseBuff.HideUI();
        bossDefenseDebuff.HideUI();
        bossShieldEffect.HideUI();
        bossDotEffect.HideUI();
        bossCritEffect.HideUI();

        bossAttackSkill.HideUI();
        bossDefenseSkill.HideUI();
        bossCritSkill.HideUI();
        bossHealSkill.HideUI();
    }

    public void UpdatePlayerEffectUIs(List<Effect> effects)
    {
        foreach (Effect effect in effects)
        {
            switch (effect.EffectType)
            {
                case EffectType.AttackBuff:
                    playerAttackBuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.AttackDebuff:
                    playerAttackDebuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DefenseBuff:
                    playerDefenseBuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DefenseDebuff:
                    playerDefenseDebuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.Shield:
                    playerShieldEffect.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DOT:
                    playerDotEffect.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.Crit:
                    playerCritEffect.UpdateUI(effect.TurnDuration, (effect as CritEffect).CurrentStage);
                    break;
            }
        }
    }

    public void UpdateBossEffectUIs(List<Effect> effects)
    {
        foreach (Effect effect in effects)
        {
            switch (effect.EffectType)
            {
                case EffectType.AttackBuff:
                    bossAttackBuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.AttackDebuff:
                    bossAttackDebuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DefenseBuff:
                    bossDefenseBuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DefenseDebuff:
                    bossDefenseDebuff.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.Shield:
                    bossShieldEffect.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.DOT:
                    bossDotEffect.UpdateUI(effect.TurnDuration, (effect as EffectStackable).CurrentStack);
                    break;
                case EffectType.Crit:
                    bossCritEffect.UpdateUI(effect.TurnDuration, (effect as CritEffect).CurrentStage);
                    break;
            }
        }
    }
}
