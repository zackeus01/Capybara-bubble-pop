//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEditor.PlayerSettings;

//[Serializable]
//public class ArmorBaseData : EquipmentBaseData
//{
//    [SerializeField] private ArmorSkills armorSkills;
//    [SerializeField] private SpriteRenderer[] img;//0Head,1Body,2Hand,3Legs
//    [SerializeField] private float baseArmorHP;
//    [SerializeField] private float baseArmorDEF;
//    [SerializeField] private float baseArmorElementRes;

//    public ArmorBaseData()
//    {
//    }

//    public ArmorBaseData(string id, int currentLevel, ArmorSkills armorSkills, SpriteRenderer[] img, float baseArmorHP, float baseArmorDEF, float baseArmorElementRes) : base(id, currentLevel)
//    {
//        this.armorSkills = armorSkills;
//        this.img = img;
//        this.baseArmorHP = baseArmorHP;
//        this.baseArmorDEF = baseArmorDEF;
//        this.baseArmorElementRes = baseArmorElementRes;
//        base.id = id;
//        base.currentLevel = currentLevel;
//    }

//    public ArmorSkills ArmorSkills { get { return armorSkills; } set { armorSkills = value; } }
//    public SpriteRenderer[] Img => img;
//    public float BaseArmorHP { get { return baseArmorHP; } set { baseArmorHP = value; } }
//    public float BaseArmorDEF { get { return baseArmorDEF; } set { baseArmorDEF = value; } }
//    public float BaseArmorElementRes { get { return baseArmorElementRes; } set { baseArmorElementRes = value; } }

//    public void GetDataArmor(ArmorDataSO aso)
//    {
//        base.id = aso.Id;
//        base.nameEquipment = aso.Name;
//        base.description = aso.Description;
//        base.elementType = aso.ElementType;
//        base.rarity = aso.Rarity;
//        base.ways = aso.Ways;
//        base.maxLevel = aso.MaxLevel;

//        baseArmorHP = aso.BaseArmorHP;
//        baseArmorDEF = aso.BaseArmorDEF;
//        baseArmorElementRes = aso.BaseArmorElementRes;

//    }


//    public void UpgradeArmor()
//    {
//        if (IsAtMaxLevel())
//        {
//            Debug.Log("Armor is already at maximum level!");
//        }
//        else
//        {

//            Debug.Log($"Armor upgraded to level {currentLevel}.");
//        }
//    }
//    public override void EquipItem()
//    {
//        base.EquipItem();  // Gọi phương thức EquipItem từ EquipmentData
//        // Logic đặc biệt cho vũ khí khi trang bị
//    }

//    // Override UnEquipItem nếu cần thêm logic đặc biệt cho giap
//    public override void UnEquipItem()
//    {
//        base.UnEquipItem();
//        Debug.Log($"Armor {id} unequipped.");
//    }

//}
