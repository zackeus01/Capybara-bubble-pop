using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

using UnityEngine;
using UnityEngine.UI;


public class EquipmentWeaponButton : MonoBehaviour
{
    public WeaponDataSO weaponData;
    [Header("InputField")]
    public Image img;
    public InputField id;
    public InputField nameEN;
    public InputField nameVI;
    public InputField ATK;
    public InputField CRIT;
    public InputField ElementATK;
    public InputField maxLevel;
    public InputField nameScriptableObject;
    public TextMeshProUGUI isCounter;
    //dropdown
    public TMP_Dropdown type;
    public TMP_Dropdown element;
    public TMP_Dropdown EarnWays;
    public TMP_Dropdown Rarity;
    private void Start()
    {
        SetDropDownVal();
        SetUpData(weaponData);
    }
    public void SetUpData(WeaponDataSO data)
    {
        nameScriptableObject.text = data.name;
      
        id.text = data.Id;
        //nameEN.text = data.WeaponName[0];
        //nameVI.text = data.WeaponName[1];
        ATK.text = data.BaseWeaponATK.ToString();
        CRIT.text = data.BaseWeaponCrit.ToString();
        ElementATK.text = data.BaseWeaponElementATK.ToString();
        maxLevel.text = data.MaxLevel.ToString();
        type.value = (int)data.WeaponType;
        element.value = (int)data.ElementType;
        EarnWays.value = (int)data.Ways;
        Rarity.value = (int)data.Rarity;
        isCounter.text = $"{data.ElementType.ToString()} khắc {ElementHelper.GetCounter(data.ElementType).ToString()}";
    }
    public void SetDropDownVal()
    {
        int weaponTypeCount = Enum.GetValues(typeof(WeaponTypeInGame)).Length;
        for (int i = 0; i < weaponTypeCount; i++)
        {
            string data = Enum.GetValues(typeof(WeaponTypeInGame)).GetValue(i).ToString();
            type.options.Add(new TMP_Dropdown.OptionData(data));
        }
        int elementcount = Enum.GetValues(typeof(ElementType)).Length;
        for (int i = 0; i < elementcount; i++)
        {
            string data = Enum.GetValues(typeof(ElementType)).GetValue(i).ToString();
            element.options.Add(new TMP_Dropdown.OptionData(data));
        }
        int EarnWaysCount = Enum.GetValues(typeof(WaysToEarnEquipment)).Length;
        for (int i = 0; i < EarnWaysCount; i++)
        {
            string data = Enum.GetValues(typeof(WaysToEarnEquipment)).GetValue(i).ToString();
            EarnWays.options.Add(new TMP_Dropdown.OptionData(data));
        }
        int RarityCount = Enum.GetValues(typeof(Rarity)).Length;
        for (int i = 0; i < RarityCount; i++)
        {
            string data = Enum.GetValues(typeof(Rarity)).GetValue(i).ToString();
            Rarity.options.Add(new TMP_Dropdown.OptionData(data));
        }
    }
    public void CheckDataWeapon()
    {
        if (nameScriptableObject.text != weaponData.name)
        {
            weaponData.name = nameScriptableObject.text;
        }
        if (id.text != weaponData.Id)
        {
            Debug.Log("Id khac");
            weaponData.Id = id.text;
        }
        //if (nameEN.text != weaponData.WeaponName[0])
        //{
        //    Debug.Log("EN khac");
        //    weaponData.WeaponName[0] = nameEN.text;
        //}
        //if (nameVI.text != weaponData.WeaponName[1])
        //{
        //    Debug.Log("VI khac");
        //    weaponData.WeaponName[1] = nameVI.text;
        //}
        if (ATK.text != weaponData.BaseWeaponATK.ToString())
        {
            Debug.Log("ATK khac");
            weaponData.BaseWeaponATK = float.Parse(ATK.text);
        }
        if (CRIT.text != weaponData.BaseWeaponCrit.ToString())
        {
            Debug.Log("CRIT khac");
            weaponData.BaseWeaponCrit = float.Parse(CRIT.text);
        }
        if (ElementATK.text != weaponData.BaseWeaponElementATK.ToString())
        {
            Debug.Log("ElementATK khac");
            weaponData.BaseWeaponElementATK = float.Parse(ElementATK.text);
        }
        if (maxLevel.text != weaponData.MaxLevel.ToString())
        {
            Debug.Log("ElementATK khac");
            weaponData.MaxLevel = int.Parse(maxLevel.text);
        }
        if ((WeaponTypeInGame)type.value != weaponData.WeaponType)
        {
            Debug.Log("Type khac");
            weaponData.WeaponType = (WeaponTypeInGame)type.value;
        }
        if ((ElementType)element.value != weaponData.ElementType)
        {
            Debug.Log("Element khac");
            weaponData.ElementType = (ElementType)element.value;
        }
        if ((Rarity)Rarity.value != weaponData.Rarity)
        {
            Debug.Log("Rarity khac");
            weaponData.Rarity = (Rarity)Rarity.value;
        }
        if ((WaysToEarnEquipment)EarnWays.value != weaponData.Ways)
        {
            Debug.Log("Ways khac");
            weaponData.Ways = (WaysToEarnEquipment)EarnWays.value;
        }
        //EditorUtility.SetDirty(weaponData);
    }
}
