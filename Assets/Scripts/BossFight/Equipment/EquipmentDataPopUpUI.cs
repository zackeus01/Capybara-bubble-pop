using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LTGUI;

public class EquipmentDataPopUpUI : MonoBehaviour
{
    [Header("tab")]
    [SerializeField] private GameObject armorDataPopUp;
    [SerializeField] private GameObject weaponDataPopUp;

    [Header("Infor")]
    [SerializeField] private string id;
    [SerializeField] private int lv;

    [SerializeField] private TextMeshProUGUI nameEquipment;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Image avatar;
    [SerializeField] private Image rarity;

    [SerializeField] private TextMeshProUGUI priceUpgrade;
    [SerializeField] private bool isEquiped;
    [SerializeField] private EquipmentType equipmentType;

    [SerializeField] private ElementType elementType;
    [SerializeField] private Image imageEquielementType;

    [Header("Armor")]
    [SerializeField] private TextMeshProUGUI baseArmorHP;
    [SerializeField] private TextMeshProUGUI baseArmorDEF;
    [SerializeField] private TextMeshProUGUI baseArmorElementRes;
    [SerializeField] private TextMeshProUGUI armorSkillsTxt;
    [SerializeField] private ArmorSkills armorSkills;
    [SerializeField] private GameObject statsAS;

    [Header("Weapon")]
    [SerializeField] private TextMeshProUGUI baseWeaponATK;
    [SerializeField] private TextMeshProUGUI baseWeaponCrit;
    [SerializeField] private TextMeshProUGUI baseWeaponElementATK;

    [Header("Button UI")]
    [SerializeField] private Button Equip;
    [SerializeField] private TextMeshProUGUI EquipText;
    [SerializeField] private Button Upgrade;
    [SerializeField] private TextMeshProUGUI UpgradeText;
    public void SetupPopupEquipmentDetail(string id)
    {
        this.id = id;
        Equipment e = EquipmentDataController.Instance.GetEquipment(id);
        EquipmentDataBaseDTO eDTO = EquipmentDataController.Instance.InventoryDataManager.GetEquipmentDataDTO(id);
        this.lv = eDTO.CurrentLevel;

        //Name vs DES
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            //Debug.Log(e.NameEquipment[0]);
            this.description.text = e.Description[0];
            this.nameEquipment.text = e.NameEquipment[0];
        }
        else
        {
            this.description.text = e.Description[1];
            this.nameEquipment.text = e.NameEquipment[1];
        }


        //Level
        if (eDTO.CurrentLevel == e.MaxLevel)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
            {
                this.txtLevel.text = $"MaxLevel";
                this.priceUpgrade.text = $"MaxLevel";
                this.UpgradeText.text = $"MaxLevel";
                Upgrade.interactable = false;
            }
            else
            {
                this.txtLevel.text = $"Tối Đa";
                this.priceUpgrade.text = $"Tối Đa";
                this.UpgradeText.text = $"Tối Đa";
                Upgrade.interactable = false;
            }

        }
        else
        {
            this.txtLevel.text = $"{eDTO.CurrentLevel}/{e.MaxLevel}";
            if (BankSystem.Instance.CheckCoinWithValue(CalculateCoinCost(eDTO.CurrentLevel)))
            {
                this.priceUpgrade.text = $"{CalculateCoinCost(eDTO.CurrentLevel)}";
                this.priceUpgrade.color = Color.white;
            }
            else
            {
                this.priceUpgrade.text = $"{CalculateCoinCost(eDTO.CurrentLevel)}";
                this.priceUpgrade.color = Color.red;
            }

            if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
            {
                this.UpgradeText.text = $"Upgrade";
            }
            else
            {
                this.UpgradeText.text = $"Nâng Cấp";

            }
            Upgrade.interactable = true;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {  // Equip
            if (!eDTO.IsEquipped)
            {
                this.EquipText.text = $"Equip";
            }
            else
            {
                this.EquipText.text = $"Equipped";
            }
        }
        else
        {
            this.EquipText.text = $"Equipped";
            { // Equip
                if (!eDTO.IsEquipped)
                {
                    this.EquipText.text = $"Trang Bị";
                }
                else
                {
                    this.EquipText.text = $"Đã Trang Bị";
                }
            }
        }



        rarity.sprite = e.ListImage[0];

        imageEquielementType.sprite = e.ListImage[1];


        this.isEquiped = eDTO.IsEquipped;
        equipmentType = e.EquipmentType;
        //Debug.Log(equipmentType);
        SetupDataUIEquipmentType(equipmentType, eDTO.CurrentLevel);

        //Button Equip
        Equip.onClick.RemoveListener(OnClickEquip);
        Equip.onClick.AddListener(OnClickEquip);
        //Button Upgrade
        Upgrade.onClick.RemoveListener(OnUpGradeLevel);
        Upgrade.onClick.AddListener(OnUpGradeLevel);

    }
    private void SetupDataUIEquipmentType(EquipmentType e, int curLv)
    {
        switch (equipmentType)
        {
            case EquipmentType.Armor:
                armorDataPopUp.SetActive(true);
                weaponDataPopUp.SetActive(false);

                ArmorDataSO a = EquipmentDataController.Instance.GetArmorDataSO(id);

                if (a != null)
                {
                    //Data Armor with Rarity 5
                    if (a.Rarity.Equals(Rarity.Rarity5))
                    {
                        statsAS.SetActive(true);
                        switch (a.ArmorSkills)
                        {
                            case ArmorSkills.BuffATK:
                                DisplayArmorPassiveSkills(a);
                                break;
                            case ArmorSkills.BuffElementATK:
                                DisplayArmorPassiveSkills(a);
                                break;
                            case ArmorSkills.DirtShield:
                                DisplayArmorPassiveSkills(a);
                                break;
                            case ArmorSkills.VirtualShield:
                                DisplayArmorPassiveSkills(a);
                                break;
                            case ArmorSkills.Healing:
                                DisplayArmorPassiveSkills(a);
                                break;
                        }
                    }
                    else
                    {
                        statsAS.SetActive(false);
                    }

                    if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                    {
                        baseArmorHP.text = $"HP +{CalculateBaseHP(curLv, a.BaseArmorHP)}";
                        baseArmorDEF.text = $"DEF +{CalculateBaseDEF(curLv, a.BaseArmorDEF)}";
                        baseArmorElementRes.text = $"Elemental Resist +{CalculateBaseRES(curLv, a.BaseArmorElementRes)}";
                    }
                    else
                    {
                        baseArmorHP.text = $"Máu +{CalculateBaseHP(curLv, a.BaseArmorHP)}";
                        baseArmorDEF.text = $"Phòng Thủ +{CalculateBaseDEF(curLv, a.BaseArmorDEF)}";
                        baseArmorElementRes.text = $"Kháng Nguyên Tố +{CalculateBaseRES(curLv, a.BaseArmorElementRes)}";
                    }

                }
                else
                {
                    Debug.Log("armor null");
                }
                avatar.transform.rotation = Quaternion.Euler(0, 0, 0);
                avatar.sprite = a.Avatar;

                break;

            case EquipmentType.Weapon:
                armorDataPopUp.SetActive(false);
                weaponDataPopUp.SetActive(true);

                WeaponDataSO w = EquipmentDataController.Instance.GetWeaponDataSO(id);

                if (w != null)
                {
                    DataWeapon(w, curLv);
                }
                else
                {
                    Debug.Log("weapon null");
                }

                switch (w.WeaponType)
                {
                    case WeaponTypeInGame.MagicSeal:
                        avatar.transform.rotation = Quaternion.Euler(0, 0, -45);
                        avatar.sprite = w.Avatar;
                        break;
                    case WeaponTypeInGame.Sword:
                        avatar.transform.rotation = Quaternion.Euler(0, 0, -45);
                        avatar.sprite = w.Avatar;
                        break;
                    case WeaponTypeInGame.Bow:
                        avatar.transform.rotation = Quaternion.Euler(0, 0, 0);
                        avatar.sprite = w.Avatar;

                        break;
                }

                break;


        }
    }
    private void DataWeapon(WeaponDataSO w, int CurrentLevel)
    {
        //Base weapon
        float baseDamgeATK = w.BaseWeaponATK;
        float baseDamgeCrit = w.BaseWeaponCrit;
        float baseDamgeElement = w.BaseWeaponElementATK;
        //Base armor



        switch (w.WeaponType)
        {
            case WeaponTypeInGame.Sword:

                if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                {
                    if (CurrentLevel == 1)
                    {
                        baseWeaponATK.text = $"ATK +{baseDamgeATK}";
                    }
                    else
                    {
                        baseWeaponATK.text = $"ATK +{CalculateBaseDameAtkSword(CurrentLevel, baseDamgeATK)}";
                    }
                    baseWeaponCrit.text = $"CRIT +{w.BaseWeaponCrit}";
                    baseWeaponElementATK.text = $"Elemental ATK +{w.BaseWeaponElementATK}";
                }
                else
                {
                    if (CurrentLevel == 1)
                    {
                        baseWeaponATK.text = $"Sát Thương +{baseDamgeATK}";
                    }
                    else
                    {
                        baseWeaponATK.text = $"Sát Thương +{CalculateBaseDameAtkSword(CurrentLevel, baseDamgeATK)}";
                    }
                    baseWeaponCrit.text = $"Chí Mạng +{w.BaseWeaponCrit}";
                    baseWeaponElementATK.text = $"Sát Thương Nguyên Tố +{w.BaseWeaponElementATK}";
                }




                break;
            case WeaponTypeInGame.Bow:
                if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                {
                    if (CurrentLevel == 1)
                    {
                        baseWeaponATK.text = $"ATK +{baseDamgeATK}";
                    }
                    else
                    {
                        baseWeaponATK.text = $"ATK +{CalculateBaseDameAtkBow(CurrentLevel, baseDamgeATK)}";
                    }
                    baseWeaponCrit.text = $"CRIT +{CalculateBaseDameCritBow(CurrentLevel, baseDamgeCrit)}";
                    baseWeaponElementATK.text = $"Elemental ATK +{CalculateBaseDameElementBow(CurrentLevel, baseDamgeElement)}";
                }
                else
                {
                    if (CurrentLevel == 1)
                    {
                        baseWeaponATK.text = $"Sát Thương +{baseDamgeATK}";
                    }
                    else
                    {
                        baseWeaponATK.text = $"Sát Thương +{CalculateBaseDameAtkBow(CurrentLevel, baseDamgeATK)}";
                    }
                    baseWeaponCrit.text = $"Chí Mạng +{CalculateBaseDameCritBow(CurrentLevel, baseDamgeCrit)}";
                    baseWeaponElementATK.text = $"Sát Thương Nguyên Tố +{CalculateBaseDameElementBow(CurrentLevel, baseDamgeElement)}";
                }

                break;

            case WeaponTypeInGame.MagicSeal:
                if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
                {
                    baseWeaponATK.text = $"ATK +{w.BaseWeaponATK}";
                    baseWeaponCrit.text = $"CRIT +{w.BaseWeaponCrit}";
                    baseWeaponElementATK.text = $"Elemental ATK +{CalculateBaseDameElementMagic(CurrentLevel, baseDamgeElement)}";
                }
                else
                {
                    baseWeaponATK.text = $"Sát Thương +{w.BaseWeaponATK}";
                    baseWeaponCrit.text = $"Chí Mạng +{w.BaseWeaponCrit}";
                    baseWeaponElementATK.text = $"Sát Thương Nguyên Tố +{CalculateBaseDameElementMagic(CurrentLevel, baseDamgeElement)}";
                }




                break;
        }
    }
    //-----------------------Calculator-----------------
    private int CalculateCoinCost(int level)
    {
        return EquipmentCalculator.CalculateCoinCost(level);
    }
    private float CalculateBaseDameAtkSword(int level, float baseDamage)
    {
        return EquipmentCalculator.CalculateBaseDameAtkSword(level, baseDamage);
    }
    private float CalculateBaseDameAtkBow(int level, float baseDamage)
    {
        return EquipmentCalculator.CalculateBaseDameAtkBow((int)level, baseDamage);
    }
    private float CalculateBaseDameCritBow(int level, float baseDamage)
    {
        return EquipmentCalculator.CalculateBaseDameCritBow((int)level, baseDamage);
    }
    private float CalculateBaseDameElementBow(int level, float baseDamage)
    {
        return EquipmentCalculator.CalculateBaseDameElementBow((int)level, baseDamage);
    }
    private float CalculateBaseDameElementMagic(int level, float baseDamage)
    {
        return EquipmentCalculator.CalculateBaseDameElementMagic((int)level, baseDamage);
    }
    private float CalculateBaseHP(int level, float hpBase)
    {
        return (float)EquipmentCalculator.CalculateBaseHP((int)level, hpBase);
    }
    private float CalculateBaseDEF(int level, float defBase)
    {
        return EquipmentCalculator.CalculateBaseDEF((int)level, defBase);
    }
    private float CalculateBaseRES(int level, float resBase)
    {
        return EquipmentCalculator.CalculateBaseRES((int)level, resBase);
    }
    //--------------------------End------------------------
    private void OnClickEquip()
    {
        UIEvent.OnEquipedEquipment.Invoke(id, equipmentType);
        UIEvent.OnUpdatePlayerMirroring.Invoke();

        EquipmentDataController.Instance.Save();
        SetupPopupEquipmentDetail(id);
    }
    private void OnUpGradeLevel()
    {
        UIEvent.OnUpgradeEquipment.Invoke(id, lv);
        EquipmentDataController.Instance.Save();
        SetupPopupEquipmentDetail(id);
    }
    private void DisplayArmorPassiveSkills(ArmorDataSO a)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            armorSkillsTxt.text = a.ArmorSkillsName[0];
        }
        else
        {
            armorSkillsTxt.text = a.ArmorSkillsName[1];
        }
    }


}
