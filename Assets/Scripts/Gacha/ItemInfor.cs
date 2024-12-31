using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfor : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private GameObject Coin;
    [SerializeField] private TextMeshProUGUI amount;

    private string id;
    public EquipmentDataSO equipment;


    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (equipment != null)
        {
            img.sprite = equipment.Avatar;
            id = equipment.Id;
        }
    }
    public void Duplicate(Rarity rarity)
    {
        if (Coin != null)
        {
            Coin.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Coin GameObject is already destroyed or missing.");
        }

        if (img != null)
        {
            img.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Image component is already destroyed or missing.");
        }

        switch (rarity)
        {
            case Rarity.Rarity1:
                amount.text = "2";
                break;
            case Rarity.Rarity2:
                amount.text = "3";
                break;
            case Rarity.Rarity3:
                amount.text = "3";
                break;
            case Rarity.Rarity4:
                amount.text = "5";
                break;
            case Rarity.Rarity5:
                amount.text = "20";
                break;
        }
    }

}
