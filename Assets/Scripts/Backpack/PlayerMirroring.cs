using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirroring : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private SpriteRenderer sword;
    [SerializeField] private SpriteRenderer bow;
    [SerializeField] private SpriteRenderer magicSeal;

    [Header("Armor")]
    [SerializeField] private SpriteRenderer headArmor;
    [SerializeField] private SpriteRenderer bodyArmor;
    [SerializeField] private SpriteRenderer lArmArmor;
    [SerializeField] private SpriteRenderer rArmArmor;

    // Start is called before the first frame update
    void Start()
    {
        UIEvent.OnUpdatePlayerMirroring.AddListener(UpdatePlayerEquipment);
        UpdatePlayerEquipment();
    }

    public void UpdatePlayerEquipment()
    {
        sword.gameObject.SetActive(false);
        bow.gameObject.SetActive(false);
        magicSeal.gameObject.SetActive(false);

        headArmor.gameObject.SetActive(false);
        bodyArmor.gameObject.SetActive(false);
        lArmArmor.gameObject.SetActive(false);
        rArmArmor.gameObject.SetActive(false);

        WeaponDataSO weapon = EquipmentDataController.Instance.GetEquippedWeapon();
        ArmorDataSO armor = EquipmentDataController.Instance.GetEquippedArmor();

        if (weapon != null )
        {
            switch (weapon.WeaponType)
            {
                case WeaponTypeInGame.Sword:
                    sword.sprite = weapon.Avatar;
                    sword.gameObject.SetActive(true);
                    break;
                case WeaponTypeInGame.Bow:
                    bow.sprite = weapon.Avatar;
                    bow.gameObject.SetActive(true);
                    break;
                case WeaponTypeInGame.MagicSeal:
                    magicSeal.sprite = weapon.Avatar;
                    magicSeal.gameObject.SetActive(true);
                    break;
            }
        }

        if (armor != null )
        {
            headArmor.sprite = armor.Head;
            bodyArmor.sprite = armor.Body;
            lArmArmor.sprite = armor.LArm;
            rArmArmor.sprite = armor.RArm;

            headArmor.gameObject.SetActive(true);
            bodyArmor.gameObject.SetActive(true);
            lArmArmor.gameObject.SetActive(true);
            rArmArmor.gameObject.SetActive(true);
        }

    }
}
