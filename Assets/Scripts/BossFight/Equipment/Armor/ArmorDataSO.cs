using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Armor_", menuName = "Boss/Equipment/Armor")]
public class ArmorDataSO : EquipmentDataSO
{
   
    //[SerializeField] private Sprite[] img;//0Head,1Body,2Hand,3Legs
    [SerializeField] private float baseArmorHP;
    [SerializeField] private float baseArmorDEF;
    [SerializeField] private float baseArmorElementRes;
    [SerializeField] private ArmorSkills armorSkills;
    [SerializeField] private List<string> armorSkillsName;

    [Header("Parts")]
    [SerializeField] private Sprite head;
    [SerializeField] private Sprite body;
    [SerializeField] private Sprite lArm;
    [SerializeField] private Sprite rArm;

    //public Sprite[] Img => img;

    public List<string> ArmorSkillsName => armorSkillsName;
    public float BaseArmorHP { get { return baseArmorHP; } set { baseArmorHP = value; } }
    public float BaseArmorDEF { get { return baseArmorDEF; } set { baseArmorDEF = value; } }
    public float BaseArmorElementRes { get { return baseArmorElementRes; } set { baseArmorElementRes = value; } }
    public ArmorSkills ArmorSkills { get { return armorSkills; } set { armorSkills = value; } }
    public Sprite Head { get { return head; } set { head = value; } }
    public Sprite Body { get { return body; } set { body = value; } }
    public Sprite LArm { get { return lArm; } set { lArm = value; } }
    public Sprite RArm { get { return rArm; } set { rArm = value; } }
}
