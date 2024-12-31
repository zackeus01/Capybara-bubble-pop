using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStats
{
    #region Player exclusive
    public static readonly float PLAYER_ManaCap = 300;          //MANA required for Player's ult

    public static readonly float PLAYER_AttackBuff = 0.04f;      //Attack BUFF player can receive
    public static readonly float PLAYER_AttackDebuff = 0.1f;    //Attack DEBUFF player can receive

    public static readonly float PLAYER_DefenseBuff = 0.04f;     //Defense BUFF player can receive
    public static readonly float PLAYER_DefenseDebuff = 0.1f;   //Defense DEBUFF player can receive

    public static readonly float PLAYER_ShieldAmount = 0.2f;    //SHIELD: Percent compared to Player's HP
    public static readonly int PLAYER_MaxShieldStack = (int)(1f / PLAYER_ShieldAmount);

    public static readonly float PLAYER_HealAmount = 0.07f;      //HP Amount receive when heal
    #endregion

    #region Boss exclusive
    public static readonly float BOSS_ManaCap = 800;            //MANA required for Boss's ult

    public static readonly float BOSS_AttackBuff = 0.04f;        //Attack BUFF boss can receive
    public static readonly float BOSS_AttackDebuff = 0.12f;      //Attack DEBUFF boss can receive

    public static readonly float BOSS_DefenseBuff = 0.04f;       //Defense BUFF boss can receive
    public static readonly float BOSS_DefenseDebuff = 0.12f;     //Defense DEBUFF boss can receive

    public static readonly float BOSS_ShieldAmount = 0.1f;      //SHIELD: Percent compared to Boss's HP
    public static readonly int BOSS_MaxShieldStack = (int)(1f / PLAYER_ShieldAmount);

    public static readonly float BOSS_HealAmount = 0.05f;      //HP Amount receive when heal

    public static readonly float BOSS_StatsMultiplier = 0.2f; //Boss stat multiply by level 
    #endregion

    public static readonly float CritRateStage1 = 1f / 24f; //Crit rate STAGE 1 (4.167%)
    public static readonly float CritRateStage2 = 1f / 8f;  //Crit rate STAGE 2 (12.5%)
    public static readonly float CritRateStage3 = 1f / 2f;  //Crit rate STAGE 3 (50%)
    public static readonly float CritRateStage4 = 1;        //Crit rate STAGE 4 (100%)
    public static readonly int MaxCritStage = 4;
    public static readonly int CritBuffDuration = 3;

    public static readonly float CritDamage = 0.5f;         //Dmg Multiplier when a Crit happens

    public static readonly float ElementalBuff = 0.5f;      //Elemental Counter
    public static readonly float ElementalDebuff = -0.5f;   //Elemental Resistance

    public static readonly float ManaPerBall = 6;          //Mana Amount receive per Ball drop

    public static readonly float UltMultiplier = 3f;        //Dmg Multiplier when using Ult

    #region Effects
    public static readonly int EFFECT_BallNumForAStack = 3;        //Number of Balls required to form 1 Effect Stack
    public static readonly int EFFECT_MaxStatStack = 5;            //Max stack of 1 effect
    public static readonly int EFFECT_StatEffectDuration = 3;

    public static readonly int EFFECT_ShieldDuration = 3;

    public static readonly float EFFECT_DotDamage = 0.05f;      //DOT DMG: Percent of HP damaged
    public static readonly int EFFECT_DotDuration = 3;
    public static readonly int EFFECT_MaxDotStack = 1;
    #endregion
}
