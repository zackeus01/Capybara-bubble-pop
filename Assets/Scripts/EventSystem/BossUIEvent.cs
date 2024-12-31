using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossUIEvent : MonoBehaviour
{
    #region Effects
    private readonly static UnityEvent<List<Effect>> _onPlayerUpdateEffects = new UnityEvent<List<Effect>>();
    private readonly static UnityEvent<List<Effect>> _onBossUpdateEffects = new UnityEvent<List<Effect>>();
    public static UnityEvent<List<Effect>> OnPlayerUpdateEffects { get => _onPlayerUpdateEffects; }
    public static UnityEvent<List<Effect>> OnBossUpdateEffects { get => _onBossUpdateEffects; }
    #endregion

    #region Player bar UI
    private readonly static UnityEvent<float> _onPlayerHealthSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onPlayerHealthUpdate = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onPlayerShieldSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onPlayerShieldUpdate = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onPlayerManaSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onPlayerManaUpdate = new UnityEvent<float>();

    public static UnityEvent<float> OnPlayerHealthSetUp { get => _onPlayerHealthSetUp; }
    public static UnityEvent<float> OnPlayerHealthUpdate { get => _onPlayerHealthUpdate; }
    public static UnityEvent<float> OnPlayerShieldSetUp { get => _onPlayerShieldSetUp; }
    public static UnityEvent<float> OnPlayerShieldUpdate { get => _onPlayerShieldUpdate; }
    public static UnityEvent<float> OnPlayerManaSetUp { get => _onPlayerManaSetUp; }
    public static UnityEvent<float> OnPlayerManaUpdate { get => _onPlayerManaUpdate; }
    #endregion

    #region Boss bar UI
    private readonly static UnityEvent<float> _onBossHealthSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onBossHealthUpdate = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onBossShieldSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onBossShieldUpdate = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onBossManaSetUp = new UnityEvent<float>();
    private readonly static UnityEvent<float> _onBossManaUpdate = new UnityEvent<float>();

    public static UnityEvent<float> OnBossHealthSetUp { get => _onBossHealthSetUp; }
    public static UnityEvent<float> OnBossHealthUpdate { get => _onBossHealthUpdate; }
    public static UnityEvent<float> OnBossShieldSetUp { get => _onBossShieldSetUp; }
    public static UnityEvent<float> OnBossShieldUpdate { get => _onBossShieldUpdate; }
    public static UnityEvent<float> OnBossManaSetUp { get => _onBossManaSetUp; }
    public static UnityEvent<float> OnBossManaUpdate { get => _onBossManaUpdate; }
    #endregion

    #region Player Skill
    private readonly static UnityEvent<List<SkillSO>> _onPlayerSkillUISetup = new UnityEvent<List<SkillSO>>();
    public static UnityEvent<List<SkillSO>> OnPlayerSkillUISetup { get => _onPlayerSkillUISetup; }

    private readonly static UnityEvent<SkillSO> _onPlayerFinishSkill = new UnityEvent<SkillSO>();
    public static UnityEvent<SkillSO> OnPlayerFinishSkill { get => _onPlayerFinishSkill; }
    #endregion

    #region VFX
    private readonly static UnityEvent _onPlayerHealActivated = new UnityEvent();
    public static UnityEvent OnPlayerHealActivated { get => _onPlayerHealActivated; }

    private readonly static UnityEvent _onPlayerHealSkillActivated = new UnityEvent();
    public static UnityEvent OnPlayerHealSkillActivated { get => _onPlayerHealSkillActivated; }

    private readonly static UnityEvent _onBossHealActivated = new UnityEvent();
    public static UnityEvent OnBossHealActivated { get => _onBossHealActivated; }

    private readonly static UnityEvent _onBossHealSkillActivated = new UnityEvent();
    public static UnityEvent OnBossHealSkillActivated { get => _onBossHealSkillActivated; }
    #endregion
}
