using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class InventoryDataManager
{

    [SerializeField] private List<EquipmentDataBaseDTO> _equipmentDatasDTO;

    public InventoryDataManager()
    {
        _equipmentDatasDTO = new List<EquipmentDataBaseDTO>();
    }

    public List<EquipmentDataBaseDTO> equipmentDatasDTO { get { return _equipmentDatasDTO; } }

    public void AddEquipmentData(string id, int currentLevel, EquipmentType equipmentType, bool IsEquipped)
    {
        if (_equipmentDatasDTO.FirstOrDefault(e => e.Id.Equals(id)) != null) return;

        _equipmentDatasDTO.Add(new EquipmentDataBaseDTO(id, equipmentType, currentLevel,  IsEquipped));
    }

    public EquipmentDataBaseDTO GetEquipmentDataDTO(string id)
    {
        return _equipmentDatasDTO.FirstOrDefault(i => i.Id.Equals(id));
    }

    public string GetEquippedEquipmentId(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                EquipmentDataBaseDTO equippedWeapon = _equipmentDatasDTO.FirstOrDefault(e => e.EquipmentType == EquipmentType.Weapon && e.IsEquipped);
                if (equippedWeapon == null) return null;
                else return equippedWeapon.Id;
            case EquipmentType.Armor:
                EquipmentDataBaseDTO equippedArmor = _equipmentDatasDTO.FirstOrDefault(e => e.EquipmentType == EquipmentType.Armor && e.IsEquipped);
                if (equippedArmor == null) return null;
                else return equippedArmor.Id;
            default:
                Debug.LogWarning($"Unsupported EquipmentType: {type}");
                return null;
        }
    }

    public void EquipedEquipment(string id, EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                EquipmentDataBaseDTO weaponToEq = _equipmentDatasDTO.FirstOrDefault(e => e.Id.Equals(id) && e.EquipmentType == EquipmentType.Weapon);
                if (weaponToEq == null) return;

                if (weaponToEq.IsEquipped)
                {
                    weaponToEq.UnEquipItem();
                }
                else
                {
                    EquipmentDataBaseDTO equippedWeapon = _equipmentDatasDTO.FirstOrDefault(e => e.IsEquipped && e.EquipmentType == EquipmentType.Weapon);
                    equippedWeapon?.UnEquipItem();

                    weaponToEq.EquipItem();
                }
                DisplayListToSave();
                break;

            case EquipmentType.Armor:
                EquipmentDataBaseDTO armorToEq = _equipmentDatasDTO.FirstOrDefault(e => e.Id.Equals(id) && e.EquipmentType == EquipmentType.Armor);
                if (armorToEq == null) return;

                if (armorToEq.IsEquipped)
                {

                    armorToEq.UnEquipItem();
                }
                else
                {
                    EquipmentDataBaseDTO equippedArmor = _equipmentDatasDTO.FirstOrDefault(e => e.IsEquipped && e.EquipmentType == EquipmentType.Armor);
                    equippedArmor?.UnEquipItem();

                    armorToEq.EquipItem();
                }
                break;

            default:
                Debug.LogWarning($"Unsupported EquipmentType: {type}");
                break;
        }
    }
    public void UpGrade(string id, int lv)
    {
        EquipmentDataBaseDTO e =  _equipmentDatasDTO.Find(e => e.Id == id);
        int coinCost = CalculateCoinCost(lv);
        if (e == null) return;
        bool checkBank = BankSystem.Instance.WithdrawCoin(coinCost);

        if (checkBank) {
            e.LevelUp();
        } else
        {
            Debug.Log("Not enough coin");
        }
    }
    private int CalculateCoinCost(int level)
    {
        int baseCost = 100; // Lượng tiêu thụ tài nguyên tại cấp độ 1
        int coinCost = baseCost;

        for (int i = 2; i <= level; i++)
        {
            coinCost += (i + 1) * 10;
        }

        return coinCost;
    }
    public void DisplayListToSave()
    {
        foreach (var item in _equipmentDatasDTO)
        {
            Debug.Log($"Id: {item.Id} / isEquip: {item.IsEquipped}");
        }

    }
}
