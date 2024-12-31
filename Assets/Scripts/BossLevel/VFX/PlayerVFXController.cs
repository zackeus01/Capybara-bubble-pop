
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    #region Cache VFX
    [Header("VFX")]
    [SerializeField]
    private ParticleSystem slashVfx;
    [SerializeField]
    private ParticleSystem healSkill;
    [SerializeField]
    private ParticleSystem healBall;
    #endregion


    private void Awake()
    {
        PlayerAnimationEvent.OnAttackHit.AddListener(PlayAttackVFX);
        BossUIEvent.OnPlayerHealActivated.AddListener(EnableHealVFXs);
        BossUIEvent.OnPlayerHealSkillActivated.AddListener(PlayHealSkillVFX);
    }

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        slashVfx = this.transform.Find("Slash").GetComponent<ParticleSystem>();
        healSkill = this.transform.Find("HealSkill").GetComponent<ParticleSystem>();
        healBall = this.transform.Find("HealBall").GetComponent<ParticleSystem>();
    }

    private void PlayAttackVFX()
    {
        slashVfx.Play();

        int i = Random.Range(0, 101);
        if (i >= 50)
        {
            SoundManager.Instance.PlayOneShotSFX(SoundKey.SwordHit1);
        }
        else
        {
            SoundManager.Instance.PlayOneShotSFX(SoundKey.SwordHit2);
        }
    }

    private void PlayHealSkillVFX()
    {
        healSkill.Play();
        SoundManager.Instance.PlayOneShotSFX(SoundKey.Healing);
    }

    private void EnableHealVFXs()
    {
        healBall.Play();
    }
}
