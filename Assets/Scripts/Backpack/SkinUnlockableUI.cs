using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class SkinUnlockableUI : SkinUI, ISkinUnlockable
{
    [SerializeField] private GameObject tick;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Button btn;

    private bool isUnlocked = false;
    private bool isEquiped = false;

    public GameObject Tick { get { return tick; } }
    public GameObject Lock { get { return _lock; } }
    public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; } }
    public bool IsEquiped { get { return isEquiped; } set { isEquiped = value; } }

    public Button Button { get { return btn; } }

    public override void Init(SkinSO skin)
    {
        tick.SetActive(false);
        this.skin = skin;

        bg.sprite = skin.Sprite;

        // Set unlock and equip status based on skin type
        GetUnlockAndEquipStatus(skin.Id);

        Debug.Log("isUnlocked: " + isUnlocked);
        // Lock or unlock UI based on the skin's unlock status
        _lock.SetActive(!isUnlocked);

        if (skin.IsHidden && !isUnlocked) this.gameObject.SetActive(false);

        // Clear previous button listeners and add appropriate ones
        btn.onClick.RemoveAllListeners();
        if (!isUnlocked)
        {
            //btn.onClick.AddListener(() => BackpackUIController.Instance.OpenPurchase(this));
            btn.onClick.AddListener(OpenPurchase);
            Debug.Log("Add btn listener unlock");
        }
        else
        {
            btn.onClick.AddListener(Equip);
            Debug.Log("Add btn listener equip");
        }

        //Debug.Log(this.skin.name + " " + skin.id + " " + isEquiped);
        // If equipped, show the tick and hide it for other skins of the same type
        Debug.Log("isEquiped: " + isEquiped);
        if (isEquiped)
        {
            SetSkinEquippedState();
        }
        gameObject.SetActive(true);
    }

    private void OpenPurchase()
    {
        Debug.Log("OK");
        BackpackUIController.Instance.OpenPurchase(this);
    }

    public void GetUnlockAndEquipStatus(string getId)
    {
        isUnlocked = SkinDataController.Instance.SkinCollection.GetSkinUnlockStatus(getId);
        isEquiped = SkinDataController.Instance.SkinCollection.GetSkinEquipStatus(getId);
    }

    public void SetSkinEquippedState()
    {
        List<SkinUnlockableUI> skinUIs = FindObjectsOfType<SkinUnlockableUI>()
            .Where(ui => ui.skin != null && ui.skin.SkinType == skin.SkinType)
            .ToList();

        // Reset all ticks and enable only the equipped one
        foreach (SkinUnlockableUI ui in skinUIs)
        {
            if (ui.tick != null)
            {
                ui.tick.SetActive(false);
                Debug.Log("tick set False");
            }
        }

        if (tick != null)
        {
            tick.SetActive(true);
            Debug.Log("tick set true: " +this.Skin.Name);
        }

        SetEquippedSkin();
    }


    public void SetEquippedSkin()
    {
        SkinDataController.Instance.SkinCollection.EquipSkin(skin.Id, skin.SkinType);
    }


    public void Equip()
    {
        List<SkinUnlockableUI> skinUIs = FindObjectsOfType<SkinUnlockableUI>()
            .Where(_ => _.skin.SkinType == skin.SkinType)
            .ToList();
        skinUIs.First(skin => skin.tick.active).tick.SetActive(false);
        tick.SetActive(true);

        SetEquippedSkin();
    }

    public void UnlockSkin()
    {
        Debug.Log("Unlock skin: " + this.name);
        SkinDataController.Instance.SkinCollection.UnlockSkin(skin.Id);

        GetUnlockAndEquipStatus(skin.Id);

        _lock.SetActive(!isUnlocked);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Equip);
    }
}

