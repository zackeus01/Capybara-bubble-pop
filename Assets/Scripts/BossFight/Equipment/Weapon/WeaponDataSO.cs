using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "1_", menuName = "Boss/Equipment/Weapon")]
public class WeaponDataSO : EquipmentDataSO
{

    [SerializeField] private WeaponTypeInGame type;

    //[SerializeField] private Sprite[] img;
    [SerializeField] private float baseWeaponATK;
    [SerializeField] private float baseWeaponCrit;
    [SerializeField] private float baseWeaponElementATK;

    public WeaponTypeInGame WeaponType { get { return type; } set { type = value; } }

    //public Sprite[] Image => img;
    public float BaseWeaponATK { get { return baseWeaponATK; } set { baseWeaponATK = value; } }
    public float BaseWeaponCrit { get { return baseWeaponCrit; } set { baseWeaponCrit = value; } }
    public float BaseWeaponElementATK { get { return baseWeaponElementATK; } set { baseWeaponElementATK = value; } }
}