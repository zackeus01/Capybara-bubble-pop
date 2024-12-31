using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour 
{
    [Header("Infor")]
    [SerializeField] private string id;
    [SerializeField] private List<string> nameEquipment;
    [SerializeField] private List<string> description;
    
    [SerializeField] private ElementType elementType;
    [SerializeField] private Rarity rarity;
    [SerializeField] private WaysToEarnEquipment ways;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private bool isEquiped;
    [SerializeField] private bool isLocked;
    [SerializeField] private int priceUpgrade;
    [Header("UI Image")]
    [SerializeField] private Sprite avatar;
    [SerializeField] private Sprite[] listImage;

    //[Header("Armor")]
    //[SerializeField] private Sprite[] img;//0Head,1Body,2Hand,3Legs
    //[SerializeField] private float baseArmorHP;
    //[SerializeField] private float baseArmorDEF;
    //[SerializeField] private float baseArmorElementRes;
    //[SerializeField] private ArmorSkills armorSkills;
    //public Sprite[] Img => img;


    //public float BaseArmorHP { get { return baseArmorHP; } set { baseArmorHP = value; } }
    //public float BaseArmorDEF { get { return baseArmorDEF; } set { baseArmorDEF = value; } }
    //public float BaseArmorElementRes { get { return baseArmorElementRes; } set { baseArmorElementRes = value; } }
    //public ArmorSkills ArmorSkills { get { return armorSkills; } set { armorSkills = value; } }

    //[Header("Weapon")]
    //[SerializeField] private WeaponTypeInGame type;

    //[SerializeField] private Sprite[] imgW;
    //[SerializeField] private float baseWeaponATK;
    //[SerializeField] private float baseWeaponCrit;
    //[SerializeField] private float baseWeaponElementATK;


    //public WeaponTypeInGame WeaponType { get { return type; } set { type = value; } }

    //public Sprite[] Image => imgW;
    //public float BaseWeaponATK { get { return baseWeaponATK; } set { baseWeaponATK = value; } }
    //public float BaseWeaponCrit { get { return baseWeaponCrit; } set { baseWeaponCrit = value; } }
    //public float BaseWeaponElementATK { get { return baseWeaponElementATK; } set { baseWeaponElementATK = value; } }


    //------------------------------------------------------------------------------------------------------------
    public Sprite[] ListImage => listImage;
    public Sprite Avatar => avatar;
    public string Id { get { return id; } }
    public List<string> NameEquipment { get { return nameEquipment; } }
    public List<string> Description { get { return description; } }
    public ElementType ElementType { get { return elementType; } set { elementType = value; } }
    public EquipmentType EquipmentType { get { return equipmentType; } }
    public Rarity Rarity { get { return rarity; } set { rarity = value; } }
    public WaysToEarnEquipment Ways { get { return ways; } set { ways = value; } }
    public int MaxLevel { get { return maxLevel; } set { maxLevel = value; } }
    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
    public bool IsEquipped { get { return isEquiped; } }
    public bool IsLocked { get { return isLocked; } }
    public int PriceUpgrade { get { return priceUpgrade; } set { priceUpgrade = value; } }


    public Equipment(EquipmentDataSO so)
    {
        id = so.Id;
        nameEquipment = so.Name;
        description = so.Description;
        elementType = so.ElementType;
        equipmentType = so.EquipmentType;
        rarity = so.Rarity;
        ways = so.Ways;
        maxLevel = so.MaxLevel;
        isLocked = so.IsLocked;
        currentLevel = so.CurrentLevel;
        avatar = so.Avatar;
        listImage = so.ListImage;

    }




    public void GetData(EquipmentDataBaseDTO dto)
    {
        id = dto.Id;
        equipmentType = dto.EquipmentType;
        currentLevel = dto.CurrentLevel;
        isEquiped = dto.IsEquipped;
    }






}
