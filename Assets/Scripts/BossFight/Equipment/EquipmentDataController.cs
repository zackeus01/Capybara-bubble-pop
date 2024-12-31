using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentDataController : Singleton<EquipmentDataController>
{
    [Header("--------------------List EquipmentSO-----------------")]
    [SerializeField] List<EquipmentDataSO> _equipmentDataSOs = new List<EquipmentDataSO>();
    public List<EquipmentDataSO> equipmentDataSOs => _equipmentDataSOs;
    [Header("--------------------List ArmorSO-----------------")]
    [SerializeField] List<ArmorDataSO> _armorDataSOs = new List<ArmorDataSO>();
    public List<ArmorDataSO> armorDataSOs => _armorDataSOs;
    [Header("--------------------List WeaponSO-----------------")]
    [SerializeField] List<WeaponDataSO> _weaponDataSOs = new List<WeaponDataSO>();
    public List<WeaponDataSO> weaponDataSOs => _weaponDataSOs;

    [SerializeField] private InventoryDataManager _inventoryDataManager;

    public InventoryDataManager InventoryDataManager
    {
        get { return _inventoryDataManager; }
    }

    private readonly List<Equipment> _equipmentDataList = new();
    public List<Equipment> EquipmentDataList => _equipmentDataList;

    void Awake()
    {
        _equipmentDataSOs = Resources.LoadAll<EquipmentDataSO>("Equipment").ToList();
        _armorDataSOs = Resources.LoadAll<ArmorDataSO>("Equipment").ToList();
        _weaponDataSOs = Resources.LoadAll<WeaponDataSO>("Equipment").ToList();
        InitData();
    }

    public WeaponDataSO GetEquippedWeapon()
    {
        string searchId = _inventoryDataManager.GetEquippedEquipmentId(EquipmentType.Weapon);
        return GetWeaponDataSO(searchId);
    }

    public ArmorDataSO GetEquippedArmor()
    {
        string searchId = _inventoryDataManager.GetEquippedEquipmentId(EquipmentType.Armor);
        return GetArmorDataSO(searchId);
    }

    public Equipment GetEquipment(string id)
    {
        return _equipmentDataList.Find(i => i.Id.Equals(id));
    }

    public ArmorDataSO GetArmorDataSO(string id)
    {
        //Debug.Log(id);
        //_armorDataSOs.ForEach(ar => Debug.Log(ar.Id));
        //Debug.Log(_armorDataSOs.FirstOrDefault(i => i.Id.Equals(id)));
        //ArmorDataSO a = _armorDataSOs.FirstOrDefault(i => i.Id.Equals(id));
        //Debug.Log(a.Id);
        return _armorDataSOs.FirstOrDefault(i => i.Id.Equals(id));
    }

    public WeaponDataSO GetWeaponDataSO(string id)
    {
        WeaponDataSO w = _weaponDataSOs.FirstOrDefault(i => i.Id.Equals(id));
        return w;
    }

    #region Save, Load
    public void Load()
    {
        SaveSystem.LoadData(GameConst.INVENTORY_FILE, ref _inventoryDataManager);
    }

    public void Save()
    {
        //foreach (EquipmentBaseData equipment in InventoryDataManager.equipmentDatas)
        //{
        //    Debug.Log($"Saving Equipment: ID = {equipment.Id}, CurrentLevel = {equipment.CurrentLevel}");
        //}
        //Debug.Log(_inventoryDataManager.equipmentDatas.Count);
        SaveSystem.SaveData(GameConst.INVENTORY_FILE, _inventoryDataManager);
    }

    #endregion
    private void InitData()
    {
        //Khoi tao List Equipment Lay tu SO
        _equipmentDataSOs.ForEach(so => _equipmentDataList.Add(new Equipment(so)));
        Debug.Log("List equipment clone SO" + _equipmentDataList.Count);
        Load();
        //Khoi tao inventory de lay equipmentDto
        _inventoryDataManager ??= new InventoryDataManager();
        //Add vao list equipment
        foreach (var equipment in _equipmentDataList)
        {
            EquipmentDataBaseDTO ExistedEquipment = InventoryDataManager.equipmentDatasDTO
                .FirstOrDefault(e => e.Id.Equals(equipment.Id));

            //Debug.Log(equipment.IsLocked);

            if (ExistedEquipment == null && equipment.IsLocked)
            {
                Debug.Log(equipment.Id);
                InventoryDataManager.AddEquipmentData(equipment.Id, equipment.CurrentLevel, equipment.EquipmentType, false);
            }
        }
        //

        _inventoryDataManager.equipmentDatasDTO.ForEach(d => _equipmentDataList.Find(s => s.Id.Equals(d.Id)).GetData(d));
        Save();
    }

    public void UnLockEquipment(string id)
    {
        Equipment e = EquipmentDataList.Find(i => i.Id.Equals(id));

     //   DuplicateEquipment(e.Id);

        InventoryDataManager.AddEquipmentData(e.Id, e.CurrentLevel, e.EquipmentType, false);

        Save();

    }

    public bool DuplicateEquipment(string id)
    {        
        EquipmentDataBaseDTO eExists = InventoryDataManager.equipmentDatasDTO.Find(i => i.Id.Equals(id));

        if (eExists == null)
        {          
            Debug.Log("Gacha get new equipment");
            return false;
        }
        else
        {
            Debug.Log("Equipment duplicate: " + eExists.Id);
            Equipment e = EquipmentDataList.Find(i => i.Id.Equals(eExists.Id));
            switch (e.Rarity)
            {
                case Rarity.Rarity1:
                    BankSystem.Instance.DepositGem(2);
                    break;
                case Rarity.Rarity2:
                    BankSystem.Instance.DepositGem(3);
                    break;
                case Rarity.Rarity3:
                    BankSystem.Instance.DepositGem(3); 
                    break;
                case Rarity.Rarity4:
                    BankSystem.Instance.DepositGem(5);
                    break;
                case Rarity.Rarity5:
                    BankSystem.Instance.DepositGem(20);
                    break;

            }
            return true;
        }



    }



}
