using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDataModify : MonoBehaviour
{
    public List<WeaponDataSO> weaponSO;
    public List<ArmorDataSO> ArmorSO;
    public Button weaponBtn;
    public Button armorBtn;
    public Button saveBtn;
    public RectTransform pos;
    public GameObject prefabWeapon;
    public GameObject prefabArmor;
    public List<EquipmentWeaponButton> listEquipmentWeapon;

    void Start()
    {
        foreach (WeaponDataSO weapon in weaponSO)
        {
            GameObject gen = Instantiate(prefabWeapon, pos);
            gen.SetActive(false);
            gen.GetComponent<EquipmentWeaponButton>().weaponData = weapon;
            gen.SetActive(true);
            listEquipmentWeapon.Add(gen.GetComponent<EquipmentWeaponButton>());
        }
    }
    public void SaveValueWeapon()
    {
        for (int i = 0; i < listEquipmentWeapon.Count; i++)
        {
            listEquipmentWeapon[i].CheckDataWeapon();
        }
        StartCoroutine(Notification());
    }
    public IEnumerator Notification()
    {
        saveBtn.image.color = Color.green;
        yield return new WaitForSeconds(2f);
        saveBtn.image.color = Color.white;
    }

}
