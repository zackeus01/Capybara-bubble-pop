using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossEvent
{
    private readonly static UnityEvent<Dictionary<BallColor, int>> _onShootBallEnd = new UnityEvent<Dictionary<BallColor, int>>();
    private readonly static UnityEvent _onCharacterTurnStart = new UnityEvent();
    private readonly static UnityEvent _onTurnEnd = new UnityEvent();
    private readonly static UnityEvent _onBossShootBallStart = new UnityEvent();
    private readonly static UnityEvent<HexCell> _onBossDestroyBall = new UnityEvent<HexCell>();
    private readonly static UnityEvent _onBossActionStart = new UnityEvent();
    private readonly static UnityEvent _onBossActionEnd = new UnityEvent();
    private readonly static UnityEvent _onBossRage = new UnityEvent();
    private readonly static UnityEvent<List<Effect>> _onBossSkillBuff = new UnityEvent<List<Effect>>();

    private readonly static UnityEvent _onPlayerShootBallStart = new UnityEvent();
    private readonly static UnityEvent _onPlayerShootBallEnd = new UnityEvent();
    private readonly static UnityEvent _onPlayerActionStart = new UnityEvent();
    private readonly static UnityEvent _onPlayerActionEnd = new UnityEvent();

    private readonly static UnityEvent<Dictionary<EffectType, int>> _onPlayerReceiveEffect = new UnityEvent<Dictionary<EffectType, int>>();
    private readonly static UnityEvent<Dictionary<EffectType, int>> _onBossReceiveEffect = new UnityEvent<Dictionary<EffectType, int>>();

    private readonly static UnityEvent<float[], ElementType> _onPlayerAttack = new UnityEvent<float[], ElementType>();
    private readonly static UnityEvent<float[], ElementType> _onBossAttack = new UnityEvent<float[], ElementType>();

    private readonly static UnityEvent _onPlayerHurt = new UnityEvent();
    private readonly static UnityEvent _onBossHurt = new UnityEvent();

    private readonly static UnityEvent _onGameWin = new UnityEvent();
    private readonly static UnityEvent<SkillSO> _onPlayerActivateSkill = new UnityEvent<SkillSO>();
    private readonly static UnityEvent<float> _onPlayerSkillLand = new UnityEvent<float>();

    private readonly static UnityEvent _onPlayerTakeDotDamage = new UnityEvent();

    private readonly static UnityEvent<int> _onPlayerRegenMana = new UnityEvent<int>();
    private readonly static UnityEvent<int> _onBossRegenMana = new UnityEvent<int>();

    private readonly static UnityEvent<int> _onPlayerHeal = new UnityEvent<int>();
    private readonly static UnityEvent<int> _onBossHeal = new UnityEvent<int>();

    private readonly static UnityEvent<float> _onPlayerUseHealSkill = new UnityEvent<float>();
    private readonly static UnityEvent _onPlayerUseFreezeSkill = new UnityEvent();

    private readonly static UnityEvent _onTryAgain = new UnityEvent();

    private readonly static UnityEvent _onBossDead = new UnityEvent();

    public static UnityEvent OnBossDead { get => _onBossDead; }
    public static UnityEvent<Dictionary<BallColor, int>> OnShootBallTurnEnd { get => _onShootBallEnd; }
    public static UnityEvent OnCharacterTurnStart { get => _onCharacterTurnStart; }
    public static UnityEvent OnTurnEnd { get => _onTurnEnd; }
    public static UnityEvent OnBossShootBallStart { get => _onBossShootBallStart; }
    public static UnityEvent OnBossActionStart { get => _onBossActionStart; }
    public static UnityEvent OnBossActionEnd { get => _onBossActionEnd; }

    public static UnityEvent OnBossRage { get => _onBossRage; }

    public static UnityEvent<List<Effect>> OnBossSkillBuff { get => _onBossSkillBuff; }
    public static UnityEvent OnPlayerShootBallStart { get => _onPlayerShootBallStart; }
    public static UnityEvent OnPlayerShootBallEnd { get => _onPlayerShootBallEnd; }
    public static UnityEvent OnPlayerActionStart { get => _onPlayerActionStart; }
    public static UnityEvent OnPlayerActionEnd { get => _onPlayerActionEnd; }
    public static UnityEvent<HexCell> OnBossDestroyBall { get => _onBossDestroyBall; }
    public static UnityEvent<Dictionary<EffectType, int>> OnPlayerReceiveEffect { get => _onPlayerReceiveEffect; }
    public static UnityEvent<Dictionary<EffectType, int>> OnBossReceiveEffect { get => _onBossReceiveEffect; }
    public static UnityEvent<float[], ElementType> OnPlayerAttack { get => _onPlayerAttack; }
    public static UnityEvent<float[], ElementType> OnBossAttack { get => _onBossAttack; }
    public static UnityEvent OnPlayerHurt { get => _onPlayerHurt; }
    public static UnityEvent OnBossHurt { get => _onBossHurt; }

    public static UnityEvent OnGameWin { get => _onGameWin; }
    public static UnityEvent<SkillSO> OnPlayerActivateSkill { get => _onPlayerActivateSkill; }
    public static UnityEvent<float> OnPlayerSkillLand { get => _onPlayerSkillLand; }
    public static UnityEvent OnPlayerTakeDotDamage { get => _onPlayerTakeDotDamage; }
    public static UnityEvent<int> OnPlayerRegenMana { get => _onPlayerRegenMana; }
    public static UnityEvent<int> OnBossRegenMana { get => _onBossRegenMana; }
    public static UnityEvent<int> OnPlayerHeal { get => _onPlayerHeal; }
    public static UnityEvent<int> OnBossHeal { get => _onBossHeal; }
    public static UnityEvent<float> OnPlayerUseHealSkill { get => _onPlayerUseHealSkill; }
    public static UnityEvent OnPlayerUseFreezeSkill { get => _onPlayerUseFreezeSkill; }
    public static UnityEvent OnExitLevel { get => _onTryAgain; }
}
