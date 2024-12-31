using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVFXController : MonoBehaviour
{
    #region Cache VFX
    [Header("VFX")]
    [SerializeField]
    private ParticleSystem firstClawAtk;
    [SerializeField]
    private ParticleSystem secondClawAtk;
    [SerializeField]
    private ParticleSystem healSkill;
    [SerializeField]
    private ParticleSystem healBall;
    [SerializeField]
    private ParticleSystem rageVfx;
    [SerializeField]
    private ParticleSystem rageStill;
    #endregion

    private void Reset()
    {
        this.LoadComponent();
    }

    private void Awake()
    {
        BossAnimation.OnAttackHit.AddListener(PlayAttackVFX);
        BossUIEvent.OnBossHealActivated.AddListener(EnableHealVFXs);
        BossUIEvent.OnBossHealSkillActivated.AddListener(PlayHealSkillVFX);
        BossEvent.OnBossRage.AddListener(PlayRageVfx);
        BossEvent.OnBossDead.AddListener(StopRageVfx);
    }

    private void LoadComponent()
    {
        firstClawAtk = this.transform.Find("Claw1").GetComponent<ParticleSystem>();
        secondClawAtk = this.transform.Find("Claw2").GetComponent<ParticleSystem>();
        healSkill = this.transform.Find("HealSkill").GetComponent<ParticleSystem>();
        healBall = this.transform.Find("HealBall").GetComponent<ParticleSystem>();
        rageVfx = this.transform.Find("Rage").GetComponent<ParticleSystem>();
        rageStill = this.transform.Find("RageStill").GetComponent<ParticleSystem>();
    }

    private void PlayAttackVFX()
    {
        StartCoroutine(PlaySequenceAttack());
    }

    private IEnumerator PlaySequenceAttack()
    {
        //CustomDebug.LogError("Play atk vfx", Color.cyan);
        firstClawAtk.Play();
        SoundManager.Instance.PlayOneShotSFX(SoundKey.Claw1);
        yield return new WaitForSeconds(0.1f);
        secondClawAtk.Play();
        SoundManager.Instance.PlayOneShotSFX(SoundKey.Claw2);
    }

    private void PlayRageVfx()
    {
        rageVfx.Play();
        rageStill.Play();
    }

    private void StopRageVfx()
    {
        rageStill.Stop();
    }

    private void EnableHealVFXs()
    {
        healBall.Play();
    }

    private void PlayHealSkillVFX()
    {
        healSkill.Play();

    }
}
