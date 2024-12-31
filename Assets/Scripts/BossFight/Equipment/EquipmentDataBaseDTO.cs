using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentDataBaseDTO
{

    [SerializeField] private string id;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int currentLevel;
    [SerializeField] private bool isEquiped;
    //[SerializeField] protected List<string> nameEquipment;
    //[SerializeField] protected string description;
    //[SerializeField] protected ElementType elementType;
    //[SerializeField] protected Rarity rarity;
    //[SerializeField] protected WaysToEarnEquipment ways;
    //[SerializeField] private bool isLocked;
    //[SerializeField] private int priceUpgrade;
    //[SerializeField] private int maxLevel;

    public string Id { get { return id; } }
    //public List<string> Name { get { return nameEquipment; } }
    //public string Description { get { return description; } }
    //public ElementType ElementType { get { return elementType; } set { elementType = value; } }
    public EquipmentType EquipmentType { get { return equipmentType; } }
    //public Rarity Rarity { get { return rarity; } set { rarity = value; } }
    //public WaysToEarnEquipment Ways { get { return ways; } set { ways = value; } }
    //public int MaxLevel { get { return maxLevel; } set { maxLevel = value; } }
    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
    public bool IsEquipped { get { return isEquiped; } }
    //public bool IsLocked { get { return isLocked; } }
    //public int PriceUpgrade { get { return priceUpgrade; } set { priceUpgrade = value; } }

    //public EquipmentDataBaseDTO(string id, int currentLevel, EquipmentType equipmentType, int priceUpgrade, bool IsEquipped, int maxLevel)
    //{
    //    this.id = id;
    //    this.equipmentType = equipmentType;
    //    this.currentLevel = currentLevel;
    //    this.priceUpgrade = priceUpgrade;
    //    this.isEquiped = IsEquipped;
    //    this.MaxLevel = maxLevel;
    //}

    public EquipmentDataBaseDTO(string id, EquipmentType equipmentType, int currentLevel, bool IsEquipped)
    {
        this.id = id;
        this.currentLevel = currentLevel;
        this.isEquiped = IsEquipped;
        this.equipmentType = equipmentType;
    }


    // Phương thức trang bị đồ
    public virtual void EquipItem()
    {
        if (!isEquiped)
        {
            isEquiped = true;
            Debug.Log($"{id} has been equipped.");
        }
        else
        {
            Debug.Log($"{id} is already equipped.");
        }
    }

    public virtual void UnEquipItem()
    {
        if (isEquiped)
        {
            isEquiped = false;
            Debug.Log($"{id} has been unequipped.");
        }
        else
        {
            Debug.Log($"{id} is not equipped.");
        }
    }


    public int LevelUp()
    {
        return currentLevel++;
    }
    
}
