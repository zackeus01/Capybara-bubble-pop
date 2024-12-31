using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private bool isEquiped;
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject popupEquipment;
    [SerializeField] private int priceUpgrade;

    public void SetupEquipmentUI(string id)
    {

        this.id = id;
        Equipment e = EquipmentDataController.Instance.GetEquipment(id);
        EquipmentDataBaseDTO eDTO = EquipmentDataController.Instance.InventoryDataManager.GetEquipmentDataDTO(id);


        this.isEquiped = eDTO.IsEquipped;

        Image backgroundImage = backGround.GetComponent<Image>();

        //backgroundImage.sprite = e.ListImage[0];

        //Bao giờ có khung rarity thì comment cái switch này
        switch (e.Rarity)
        {
            case Rarity.Rarity1:
                //backgroundImage.color = Color.gray;
                backgroundImage.sprite = e.ListImage[0];
                break;
            case Rarity.Rarity2:
                //backgroundImage.color = Color.green;
                backgroundImage.sprite = e.ListImage[0];
                break;
            case Rarity.Rarity3:
                //backgroundImage.color = Color.blue;
                backgroundImage.sprite = e.ListImage[0];
                break;
            case Rarity.Rarity4:
                //backgroundImage.color = Color.magenta;
                backgroundImage.sprite = e.ListImage[0];
                break;
            case Rarity.Rarity5:
                //backgroundImage.color = Color.red;
                backgroundImage.sprite = e.ListImage[0];
                break;
            default:
                //backgroundImage.color = Color.white;
                backgroundImage.sprite = e.ListImage[0];
                break;

        }

        currentLevel.text = $"{eDTO.CurrentLevel}";


        if (e.EquipmentType == EquipmentType.Weapon)
        {
            WeaponDataSO w = EquipmentDataController.Instance.GetWeaponDataSO(id);
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
        }
        else
        {
            avatar.transform.rotation = Quaternion.Euler(0, 0, 0);
            avatar.sprite = e.Avatar;
        }

        priceUpgrade = e.PriceUpgrade;
    }


    public void ActivePopupequipment()
    {
        UIEvent.OnOpenEquipmentPopup.Invoke(id);
    }



}
