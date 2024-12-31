
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EquipmentUIController : MonoBehaviour
{
    [SerializeField] private Sprite defaulImgWea;
    [SerializeField] private Sprite defaulKhung;
    private float angelWeapon = -45;
    private float angelDefault = 0;
    [SerializeField] private EquipmentUI equipmentPrefab;
    [Header("Player Section")]
    [SerializeField] private Image player;
    [Header("Equipment Section")]
    [SerializeField] private Button armorButton;
    [SerializeField] private Image armorImage;
    [SerializeField] private Image arRarity;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image weRarity;
    //private string currentArmorId;
    //private string currentWeaponId;
    [Header("Inventory Section")]
    [SerializeField] private Transform contentTransform;
    private ObjectPool<EquipmentUI> pool;
    [SerializeField] private EquipmentDataPopUpUI equipmentPopup;
    private void Start()
    {
        SetupUIPlayerEquipeds();
    }
    private void Awake()
    {
        SetupPool();
    }
    private void OnEnable()
    {
        SetupUI();
        InitEvents();
    }
    private void OnDisable()
    {
        ClearPool();
        RemoveEvents();
    }
    private void SetupUI()
    {
        //TODO:
        // ClearPool before Init Equipment UI
        ClearPool();
        //Debug.Log(EquipmentDataController.Instance.InventoryDataManager.equipmentDatasDTO.Count);
        EquipmentDataController.Instance.InventoryDataManager.equipmentDatasDTO.ForEach(data => pool.Get().SetupEquipmentUI(data.Id));
    }
    private void InitEvents()
    {
        UIEvent.OnEquipedEquipment.AddListener(OnEquipedEquipment);
        UIEvent.OnOpenEquipmentPopup.AddListener(OpenEquipmentPopupUI);
        UIEvent.OnUpgradeEquipment.AddListener(OnUpgradeEquipment);
    }
    private void RemoveEvents()
    {
        UIEvent.OnEquipedEquipment.RemoveListener(OnEquipedEquipment);
        UIEvent.OnOpenEquipmentPopup.RemoveListener(OpenEquipmentPopupUI);
        UIEvent.OnUpgradeEquipment.RemoveListener(OnUpgradeEquipment);
    }
    public void OnEquipedEquipment(string id, EquipmentType eType)
    {
        Debug.Log("id:" + id);
        EquipmentDataController.Instance.InventoryDataManager.EquipedEquipment(id, eType);
        SetupUIPlayerEquipeds();
    }
    public void OnUpgradeEquipment(string id, int lv)
    {
        Debug.Log("id:" + id);
        EquipmentDataController.Instance.InventoryDataManager.UpGrade(id, lv);
        SetupUI();
    }
    private void OpenEquipmentPopupUI(string id)
    {
        equipmentPopup.gameObject.SetActive(true);
        equipmentPopup.SetupPopupEquipmentDetail(id);
    }
    #region ObjectPool
    private void SetupPool()
    {
        //ClearPool();
        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<EquipmentUI>(
            createFunc: () => Instantiate(equipmentPrefab, contentTransform),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 6
        );

    }
    public void ClearPool()
    {
        //Debug.Log("clear");
        //Debug.Log("childcount" + contentTransform.childCount, contentTransform);

        // Loop through all active objects in the pool and release them
        for (int i = contentTransform.childCount - 1; i >= 0; i--)
        {

            var btn = contentTransform.GetChild(i).GetComponent<EquipmentUI>();
            //Debug.Log(btn.isActiveAndEnabled, btn);
            if (btn != null && btn.gameObject.activeSelf)
            {
                pool.Release(btn);
            }
        }
    }
    #endregion
    public void SetupUIPlayerEquipeds()
    {
        //Find equipment equiping
        List<EquipmentDataBaseDTO> eDTO = EquipmentDataController.Instance.InventoryDataManager.equipmentDatasDTO
            .Where(i => i.IsEquipped.Equals(true)).ToList();

        // Set defaul null for weaponImage & armorImage
        weaponImage.sprite = defaulImgWea;
        armorImage.sprite = null;

        arRarity.sprite = defaulKhung;
        weRarity.sprite = defaulKhung;

        bool hasWeapon = false;
        bool hasArmor = false;

        foreach (var item in eDTO)
        {
            // Find EquipmentDataSO with Id of item
            EquipmentDataSO e = EquipmentDataController.Instance.equipmentDataSOs.Find(i => i.Id == item.Id);

            if (e != null)
            {
                switch (e.EquipmentType)
                {
                    case EquipmentType.Weapon:

                        WeaponDataSO weapon = EquipmentDataController.Instance.weaponDataSOs.Find(i => i.Id == item.Id);

                        switch (weapon.WeaponType)
                        {
                            case WeaponTypeInGame.MagicSeal:
                                weaponImage.transform.rotation = Quaternion.Euler(0, 0, angelWeapon);
                                weaponImage.sprite = e.Avatar;
                                weRarity.sprite = e.ListImage[0];
                                hasWeapon = true;
                                break;
                            case WeaponTypeInGame.Sword:
                                weaponImage.transform.rotation = Quaternion.Euler(0, 0, angelWeapon);
                                weaponImage.sprite = e.Avatar;
                                weRarity.sprite = e.ListImage[0];
                                hasWeapon = true;
                                break;
                            case WeaponTypeInGame.Bow:
                                weaponImage.sprite = e.Avatar;
                                weRarity.sprite = e.ListImage[0];
                                weaponImage.transform.rotation = Quaternion.Euler(0, 0, angelDefault);
                                hasWeapon = true;
                                break;
                        }

                        break;

                    case EquipmentType.Armor:
                        armorImage.sprite = e.Avatar;
                        arRarity.sprite = e.ListImage[0];
                        hasArmor = true;
                        break;
                }
            }
            else
            {
                Debug.Log("Null list equip");
            }
        }

        if (hasWeapon && hasArmor)
        {
            Debug.Log("Equipped both weapon and armor.");
        }
        else if (hasWeapon)
        {
            Debug.Log("Equipped only weapon.");
            armorImage.sprite = defaulKhung;
            arRarity.sprite = defaulKhung;
        }
        else if (hasArmor)
        {
            Debug.Log("Equipped only armor.");
            weaponImage.sprite = defaulImgWea;
            weRarity.sprite = defaulKhung;
        }
        else
        {
            Debug.Log("No equipment equipped.");
            weaponImage.sprite = defaulImgWea;
            armorImage.sprite = defaulKhung;
            arRarity.sprite = defaulKhung;
            weRarity.sprite = defaulKhung;
        }
    }


}

