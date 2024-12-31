using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealVFX : MonoBehaviour
{
    [SerializeField] private GameObject groundSymbol;
    [SerializeField] private List<ParticleSystem> vfxs;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        DisableVFXs();

        BossUIEvent.OnPlayerHealActivated.AddListener(EnableVFXs);
    }

    public void DisableVFXs()
    {
        groundSymbol.SetActive(false);
        foreach (var vfx in vfxs)
        {
            vfx.gameObject.SetActive(false);
        }
    }

    public void EnableVFXs()
    {
        groundSymbol.SetActive(true);
        StartCoroutine(HideGroundSymbolAfterTime());

        foreach (var vfx in vfxs)
        {
            vfx.gameObject.SetActive(true);
            vfx.Play();
            StartCoroutine(StopVFXAfterTime(vfx));
        }
    }

    private IEnumerator StopVFXAfterTime(ParticleSystem vfx)
    {
        yield return new WaitForSeconds(duration);
        if (vfx != null)
        {
            vfx.Stop();
            vfx.gameObject.SetActive(false);
        }
    }

    private IEnumerator HideGroundSymbolAfterTime()
    {
        yield return new WaitForSeconds(duration);
        groundSymbol.SetActive(false);
    }

}
