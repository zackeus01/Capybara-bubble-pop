//using System;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//[Serializable]
//public class WeaponBaseData : EquipmentBaseData
//{
//    [SerializeField] private WeaponTypeInGame type;
//    [SerializeField] private SpriteRenderer img;
//    [SerializeField] private float baseWeaponATK;
//    [SerializeField] private float baseWeaponCrit;
//    [SerializeField] private float baseWeaponElementATK;

//    public WeaponBaseData()
//    {
//    }

//    public WeaponBaseData(string id, int currentLevel, EquipmentType equipmentType, WeaponTypeInGame type, SpriteRenderer img, float baseWeaponATK,
//        float baseWeaponCrit, float baseWeaponElementATK) : base(id, currentLevel)
//    {
//        this.type = type;
//        this.img = img;
//        BaseWeaponATK = baseWeaponATK;
//        BaseWeaponCrit = baseWeaponCrit;
//        BaseWeaponElementATK = baseWeaponElementATK;
        
//    }

//    public WeaponTypeInGame TypeInGame { get { return type; } }
//    public SpriteRenderer Image => img;
//    public float BaseWeaponATK { get => baseWeaponATK; set => baseWeaponATK = value; }
//    public float BaseWeaponCrit { get => baseWeaponCrit; set => baseWeaponCrit = value; }
//    public float BaseWeaponElementATK { get => baseWeaponElementATK; set => baseWeaponElementATK = value; }


//    public void GetDataWeapon(WeaponDataSO wso)
//    {
//        base.id = wso.Id;
//        base.nameEquipment = wso.Name;
//        base.description = wso.Description;
//        base.elementType = wso.ElementType;
//        base.rarity = wso.Rarity;
//        base.ways = wso.Ways;
//        base.maxLevel = wso.MaxLevel;

//        img = wso.Image;
//        baseWeaponATK = wso.BaseWeaponATK;
//        baseWeaponCrit = wso.BaseWeaponCrit;
//        baseWeaponElementATK = wso.BaseWeaponElementATK;

//    }


//    public void UpgradeWeapon()
//    {
//        if (IsAtMaxLevel())
//        {
//            Debug.Log("Weapon is already at maximum level!");
//        }
//        else
//        { 
            
//            Debug.Log($"Weapon upgraded to level {currentLevel}.");
//        }
//    }
//    public override void EquipItem()
//    {
//        base.EquipItem();  // Gọi phương thức EquipItem từ EquipmentData
//        // Logic đặc biệt cho vũ khí khi trang bị

//        Debug.Log($"Weapon {id} equipped with ATK: {baseWeaponATK} and Crit: {baseWeaponCrit}");
//    }

//    // Override UnEquipItem nếu cần thêm logic đặc biệt cho vũ khí
//    public override void UnEquipItem()
//    {
//        base.UnEquipItem();
//        Debug.Log($"Weapon {id} unequipped.");
//    }
//}
