using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "Bubble Shooter/Skill")]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillType skillType;

    [Header("Damage Type")]
    [SerializeField] private List<float> damageMultiplers;

    [Header("Heal Type")]
    [SerializeField] private float healAmount;

    [Header("Others")]
    [SerializeField] private float manaCost;
    [SerializeField] private Sprite icon;
    [SerializeField] private AnimationClip animationClip;

    public SkillType SkillType {  get { return skillType; } set { skillType = value; } }
    public List<float> DamageMultipliers {  get { return damageMultiplers; } set {  damageMultiplers = value; } }
    public float HealAmount {  get { return healAmount; } set { healAmount = value; } }
    public float ManaCost {  get { return manaCost; } set { manaCost = value; } }
    public Sprite Icon {  get { return icon; } set { icon = value; } }
    public AnimationClip AnimationClip {  get { return animationClip; } set { animationClip = value; } }
}

public enum SkillType
{
    ComboDamage,
    Heal,
    Freeze
}
