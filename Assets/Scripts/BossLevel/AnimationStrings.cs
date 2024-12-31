using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStrings : MonoBehaviour
{
    public static readonly string isIdle = "isIdle";
    public static readonly string isDead = "isDead";
    public static readonly string isAttack = "isAttack";
    public static readonly string isSkill = "isSkill";
    public static readonly string isRage = "isRage";
    public static readonly string isUlti = "isUltimate";
    public static readonly string isEntry = "isEntry";
    public static readonly string isHurt = "isHurt";
    public static readonly string isBuffed = "isBuffed";
    public static readonly string isComboSkill = "isComboSkill";
    public static readonly string isHealSkill = "isHealSkill";
    public static readonly string isFreezeSkill = "isFreezeSkill";
    public static readonly string isSword = "isSword";
    public static readonly string isBow = "isBow";
    public static readonly string isMagic = "isMagic";

    public static List<string> listBossAnimations = new List<string>
    {
        isIdle,
        isDead,
        isAttack,
        isSkill,
        isRage,
        isUlti,
        isHurt,
        isEntry,
        isBuffed
    };

    public static List<string> listPlayerAnimations = new List<string>
    {
        isIdle,
        isDead,
        isAttack,
        isHurt,
        isBuffed,
        isComboSkill,
        isHealSkill,
        isFreezeSkill
    };

    public static List<string> listPlayerSkillAnimations = new List<string>
    {
        isComboSkill,
        isHealSkill,
        isFreezeSkill
    };

    public static List<string> listPlayerWeaponAnimations = new List<string>
    {
        isSword,
        isBow,
        isMagic
    };
}
