using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;



public class GachaController : MonoBehaviour
{
    [SerializeField] private GachaRate[] gacha;
    [SerializeField] private Transform parent, pos;
    [SerializeField] private GameObject onceTime;
    [SerializeField] private GameObject tenTime;
    [SerializeField] private Transform rewardContainer;

    GameObject Item2;
    ItemInfor itemInfor;
    EquipmentDataSO selectedEquipment = null;
    private const string GACHA_COUNTWEA_KEY = "GachaCountWeapon";
    private const string GACHA_COUNTARM_KEY = "GachaCountArmor";
    private int guaranteedRarity5Interval = 50;
    public GameObject popUpTab;
    public GameObject popUpGacha;


    private int GetGachaWeaponCount()
    {
        return PlayerPrefs.GetInt(GACHA_COUNTWEA_KEY, 0);
    }
    private int GetGachaArmorCount()
    {
        return PlayerPrefs.GetInt(GACHA_COUNTARM_KEY, 0);
    }

    private void SaveGachaWeaponCount(int count)
    {
        PlayerPrefs.SetInt(GACHA_COUNTWEA_KEY, count);
        PlayerPrefs.Save();
    }
    private void SaveGachaArmorCount(int count)
    {
        PlayerPrefs.SetInt(GACHA_COUNTARM_KEY, count);
        PlayerPrefs.Save();
    }
    public void GachaWeapon()
    {
        int gem = 20;
        bool isEnoughtCoin = BankSystem.Instance.WithdrawGem(gem);
        if (isEnoughtCoin)
        {
            //  BankSystem.Instance.WithdrawGem(20);
            popUpGacha.SetActive(true);
            if (rewardContainer != null)
            {
                foreach (Transform child in rewardContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            if (Item2 != null)
            {
                Destroy(Item2);
            }
            Item2 = Instantiate(onceTime, pos.position, Quaternion.identity) as GameObject;
            Item2.transform.SetParent(parent);
            Item2.transform.localScale = new Vector3(1, 1, 1);
            itemInfor = Item2.GetComponent<ItemInfor>();
            int gachaCount = GetGachaWeaponCount();
            gachaCount++;
            if (gachaCount % 10 == 0)
            {
                selectedEquipment = GetGuaranteedRarity4Reward();
            }
            else if (gachaCount % guaranteedRarity5Interval == 0)
            {
                selectedEquipment = GetGuaranteedRarity5Reward();
                gachaCount = 0;
                SaveGachaWeaponCount(gachaCount);
            }
            else
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                int cumulativeRate = 0;
                for (int i = 0; i < gacha.Length; i++)
                {
                    cumulativeRate += gacha[i].Rate;
                    if (rnd <= cumulativeRate)
                    {
                        selectedEquipment = Reward(gacha[i].Rarity);
                        break;
                    }
                }
            }
            itemInfor.equipment = selectedEquipment;
            bool isdupicate = EquipmentDataController.Instance.DuplicateEquipment(itemInfor.equipment.Id);
            Debug.Log(isdupicate);
            if (isdupicate)
            {
                itemInfor.Duplicate(itemInfor.equipment.Rarity);
            }
            else
            {
                EquipmentDataController.Instance.UnLockEquipment(itemInfor.equipment.Id);
            }
            SaveGachaWeaponCount(gachaCount);
        }
        else
        {
            popUpGacha.SetActive(false);
            popUpTab.SetActive(true);
        }

    }
    public void GachaArmor()
    {
        bool isEnoughtCoin = BankSystem.Instance.WithdrawGem(20);
        if (isEnoughtCoin)
        {
            popUpGacha.SetActive(true);
            if (rewardContainer != null)
            {
                foreach (Transform child in rewardContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            if (Item2 != null)
            {
                Destroy(Item2);
            }
            Item2 = Instantiate(onceTime, pos.position, Quaternion.identity) as GameObject;
            Item2.transform.SetParent(parent);
            Item2.transform.localScale = new Vector3(1, 1, 1);
            itemInfor = Item2.GetComponent<ItemInfor>();
            int gachaCount = GetGachaArmorCount();
            gachaCount++;

            if (gachaCount % 10 == 0)
            {
                selectedEquipment = GetGuaranteedRarity4Reward();
            }
            else if (gachaCount % guaranteedRarity5Interval == 0)
            {
                selectedEquipment = GetGuaranteedRarity5Reward();
                gachaCount = 0;
                SaveGachaArmorCount(gachaCount);
            }
            else
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                int cumulativeRate = 0;
                for (int i = 0; i < gacha.Length; i++)
                {
                    cumulativeRate += gacha[i].Rate;
                    if (rnd <= cumulativeRate)
                    {
                        selectedEquipment = Reward(gacha[i].Rarity);
                        break;
                    }
                }
            }
            itemInfor.equipment = selectedEquipment;
            bool isdupicate = EquipmentDataController.Instance.DuplicateEquipment(itemInfor.equipment.Id);
            Debug.Log(isdupicate);
            if (isdupicate)
            {
                itemInfor.Duplicate(itemInfor.equipment.Rarity);
            }
            else
            {
                EquipmentDataController.Instance.UnLockEquipment(itemInfor.equipment.Id);
            }
            SaveGachaArmorCount(gachaCount);
        }
        else
        {
            popUpGacha.SetActive(false);
            popUpTab.SetActive(true);
        }

    }

    EquipmentDataSO Reward(string rarity)
    {
        GachaRate gr = Array.Find(gacha, rt => rt.Rarity == rarity);
        EquipmentDataSO[] reward = gr.Reward;
        int rnd = UnityEngine.Random.Range(0, reward.Length);
        return reward[rnd];
    }
    private EquipmentDataSO GetGuaranteedRarity5Reward()
    {
        GachaRate gr = Array.Find(gacha, rt => rt.Rarity == "5");
        if (gr != null && gr.Reward.Length > 0)
        {
            int rnd = UnityEngine.Random.Range(0, gr.Reward.Length);
            return gr.Reward[rnd];
        }
        return null;
    }
    private EquipmentDataSO GetGuaranteedRarity4Reward()
    {
        GachaRate gr = Array.Find(gacha, rt => rt.Rarity == "4");
        if (gr != null && gr.Reward.Length > 0)
        {
            int rnd = UnityEngine.Random.Range(0, gr.Reward.Length);
            return gr.Reward[rnd];
        }
        return null;
    }

    public void GachaTenTimes()
    {
        bool isEnoughtCoin = BankSystem.Instance.WithdrawGem(200);
        if (isEnoughtCoin)
        {
            popUpGacha.SetActive(true);
            List<EquipmentDataSO> rewards = new List<EquipmentDataSO>();
            int gachaCount = GetGachaWeaponCount();

            for (int j = 0; j < 10; j++)
            {
                gachaCount++;
                if (gachaCount % 10 == 0)
                {
                    selectedEquipment = GetGuaranteedRarity4Reward();
                }
                else if (gachaCount % guaranteedRarity5Interval == 0)
                {
                    selectedEquipment = GetGuaranteedRarity5Reward();
                    gachaCount = 0;
                    SaveGachaWeaponCount(gachaCount);
                }
                else
                {
                    int rnd = UnityEngine.Random.Range(1, 101);
                    int cumulativeRate = 0;
                    for (int i = 0; i < gacha.Length; i++)
                    {

                        cumulativeRate += gacha[i].Rate;
                        if (rnd <= cumulativeRate)
                        {
                            selectedEquipment = Reward(gacha[i].Rarity);
                            break;
                        }
                    }
                }
                bool isdupicate = EquipmentDataController.Instance.DuplicateEquipment(itemInfor.equipment.Id);
                Debug.Log(isdupicate);
                if (isdupicate)
                {
                    itemInfor.Duplicate(itemInfor.equipment.Rarity);
                }
                else
                {
                    EquipmentDataController.Instance.UnLockEquipment(itemInfor.equipment.Id);
                }
                rewards.Add(selectedEquipment);
            }
            SaveGachaWeaponCount(gachaCount);
            DisplayGachaTenTimesResult(rewards);
        }
        else
        {
            popUpTab.SetActive(true);
            popUpGacha.SetActive(false);

        }

    }
    public void GachaTenTimesArmor()
    {
        bool isEnoughtCoin = BankSystem.Instance.WithdrawGem(200);
        if (isEnoughtCoin)
        {
            popUpGacha.SetActive(true);
            List<EquipmentDataSO> rewards = new List<EquipmentDataSO>();
            int gachaCount = GetGachaArmorCount();

            for (int j = 0; j < 10; j++)
            {
                gachaCount++;
                if (gachaCount % 10 == 0)
                {
                    selectedEquipment = GetGuaranteedRarity4Reward();
                }
                else if (gachaCount % guaranteedRarity5Interval == 0)
                {
                    selectedEquipment = GetGuaranteedRarity5Reward();
                    gachaCount = 0;
                    SaveGachaArmorCount(gachaCount);
                }
                else
                {
                    int rnd = UnityEngine.Random.Range(1, 101);
                    int cumulativeRate = 0;
                    for (int i = 0; i < gacha.Length; i++)
                    {

                        cumulativeRate += gacha[i].Rate;
                        if (rnd <= cumulativeRate)
                        {
                            selectedEquipment = Reward(gacha[i].Rarity);
                            break;
                        }
                    }
                }
                EquipmentDataController.Instance.UnLockEquipment(selectedEquipment.Id);
                bool isdupicate = EquipmentDataController.Instance.DuplicateEquipment(itemInfor.equipment.Id);
                Debug.Log(isdupicate);
                if (isdupicate)
                {
                    itemInfor.Duplicate(itemInfor.equipment.Rarity);
                }
                else
                {
                    EquipmentDataController.Instance.UnLockEquipment(itemInfor.equipment.Id);
                }
                rewards.Add(selectedEquipment);
            }
            SaveGachaArmorCount(gachaCount);
            DisplayGachaTenTimesResult(rewards);
        }
        else
        {
            popUpTab.SetActive(true);
            popUpGacha.SetActive(false);
        }

    }
    void DisplayGachaTenTimesResult(List<EquipmentDataSO> rewards)
    {
        if (Item2 != null)
        {
            Destroy(Item2);
        }
        if (rewardContainer == null)
        {
            return;
        }
        // Xóa hết các phần tử cũ trong reward container
        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var reward in rewards)
        {
            GameObject itemObject = Instantiate(onceTime, pos.position, Quaternion.identity);
            itemObject.transform.SetParent(rewardContainer, false);
            itemObject.transform.localScale = Vector3.one;
            ItemInfor itemInfor = itemObject.GetComponent<ItemInfor>();

            if (itemInfor != null)
            {
                itemInfor.equipment = reward;
                bool isDuplicate = EquipmentDataController.Instance.DuplicateEquipment(itemInfor.equipment.Id);
                Debug.Log($"Checking for duplicate: {isDuplicate}");

                if (isDuplicate)
                {
                    // Nếu item bị duplicate, bạn có thể thay ảnh hoặc xử lý khác
                    itemInfor.Duplicate(itemInfor.equipment.Rarity);
                }
                else
                {
                    // Nếu item không bị duplicate, unlock nó
                    EquipmentDataController.Instance.UnLockEquipment(itemInfor.equipment.Id);
                }

                Debug.Log($"Reward: {itemInfor.equipment}");
            }
        }
    }





}
