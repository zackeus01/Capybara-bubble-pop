using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragonCtrl : MonoBehaviour
{
    [SerializeField]
    private BossStateManager stateManager;

    #region Rage multiplier
    protected float atkMultiplier = 1f;
    public float AtkMultiplier { get { return atkMultiplier; } }

    protected float defMultiplier = 1f;
    public float DefMultiplier { get { return defMultiplier; } }

    protected float totalDmgMultiplier = 1f;
    public float TotalDmgMultiplier { get { return totalDmgMultiplier; } }

    protected float physicDmgMultiplier = 1f;
    public float PhysicDmgMultiplier { get { return physicDmgMultiplier; } }

    protected float elementalDmgMultiplier = 1f;
    public float ElementalDmgMultiplier { get { return elementalDmgMultiplier; } }

    protected float totalDmgReceiveDivisor = 1f;
    public float TotalDmgReceiveDivisor { get { return totalDmgReceiveDivisor; } }

    protected float physicDmgReceiveDivisor = 1f;
    public float PhysicDmgReceiveDivisor { get { return physicDmgReceiveDivisor; } }

    protected float elemetalDmgReceiveDivisor = 1f;
    public float ElementalDmgReceiveDivisor { get { return elemetalDmgReceiveDivisor; } }

    protected float manaMultiplier = 1f;
    public float ManaMultiplier { get { return manaMultiplier; } }

    protected float healMultiplier = 1f;
    public float HealMultiplier { get { return healMultiplier; } }

    protected float critBonus = 0f;
    public float CritBonus { get { return critBonus; } }
    #endregion

    private void Reset()
    {
        this.LoadComponent();
    }

    protected virtual void LoadComponent()
    {
        stateManager = this.GetComponent<BossStateManager>();
    }

    public abstract void ShootBall(BossStateManager bossStateManager);

    public abstract void EnterRageState();


}
