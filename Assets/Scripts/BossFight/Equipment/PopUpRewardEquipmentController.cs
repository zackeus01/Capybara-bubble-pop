using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpRewaidEquipmentController : MonoBehaviour
{

    [SerializeField]
    private List<EquipmentDataSO> equipmentSetLevel9 = new();

    [SerializeField] private Image Rarityweapon;
    [SerializeField] private Image Rarityweapon1;
    [SerializeField] private Image Rarityweapon2;
    [SerializeField] private Image RarityRarmor;

    [SerializeField] private Image weaponAvatar;
    [SerializeField] private Image weaponAvatar1;
    [SerializeField] private Image weaponAvatar2;
    [SerializeField] private Image armorAvatar;

    //[SerializeField] private TextMeshProUGUI nameWeapon;
    //[SerializeField] private TextMeshProUGUI nameArmor;
    [SerializeField] private TextMeshProUGUI nameBundle;



    private void Start()
    {
        SetupUiReward();
    }


    public void SetupUiReward()
    {
        nameBundle.text = $"Bundle Plant";

        foreach (var item in equipmentSetLevel9)
        {

            if (item.EquipmentType == EquipmentType.Weapon)
            {
                switch (item.Id)
                {
                    case "BWoR1":
                        weaponAvatar.sprite = item.Avatar;
                        Rarityweapon.sprite = item.ListImage[0];
                        //nameWeapon.text = item.Name[0];
                        break;
                    case "MWoR1":
                        weaponAvatar1.transform.rotation = Quaternion.Euler(0, 0, -45);
                        weaponAvatar1.sprite = item.Avatar;
                        Rarityweapon1.sprite = item.ListImage[0];
                        break;
                    case "SWoR1":
                        weaponAvatar2.transform.rotation = Quaternion.Euler(0, 0, -45);
                        weaponAvatar2.sprite = item.Avatar;
                        Rarityweapon2.sprite = item.ListImage[0];
                        break;

                }
            }
            else
            {
                RarityRarmor.sprite = item.ListImage[0];
                armorAvatar.sprite = item.Avatar;
                //nameArmor.text = item.Name[0];
            }

        }
    }

    public void RewardAfterLevel9()
    {
        foreach (EquipmentDataSO e in equipmentSetLevel9)
        {
            EquipmentDataController.Instance.InventoryDataManager.AddEquipmentData(e.Id, e.CurrentLevel, e.EquipmentType, false);
        }
    }



}
