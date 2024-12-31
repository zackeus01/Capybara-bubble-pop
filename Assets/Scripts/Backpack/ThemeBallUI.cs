using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ThemeBallUI : ThemeUI, ISkinUnlockable
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

    private ObjectPool<SkinUI> pool;

    #region ObjectPool
    private void SetupPool()
    {
        ClearPool();

        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<SkinUI>(
            createFunc: () => Instantiate(SkinUIPrefab, SkinContainer),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 6
        );

    }

    public void ClearPool()
    {
        // Loop through all active objects in the pool and release them
        for (int i = SkinContainer.childCount - 1; i >= 0; i--)
        {
            var child = SkinContainer.GetChild(i).GetComponent<SkinUI>();
            if (child != null && child.isActiveAndEnabled)
            {
                pool.Release(child);
            }
        }

    }
    #endregion

    private void OnEnable()
    {
        SetupPool();
        UIEvent.OnChangeMenuTab.AddListener(OnChangeMenuTab);
    }

    private void OnDisable()
    {
        ClearPool();
        UIEvent.OnChangeMenuTab.RemoveListener(OnChangeMenuTab);
    }

    public override void Init(ThemeSO theme)
    {
        ClearPool();

        Theme = theme;
        //skin.AddSkinData();

        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        { Title.text = theme.Name; }
        else
        {
            Debug.Log(theme.VieName);
            Title.text = theme.VieName;
        }



        // Set unlock and equip status based on skin type
        if (theme.IsHidden) this.gameObject.SetActive(false);
        gameObject.SetActive(true);

        foreach (var skin in theme.Skins)
        {
            pool.Get().GetComponent<SkinUI>().Init(skin);
        }

        Tick.SetActive(false);

        // Set unlock and equip status based on skin type
        GetUnlockAndEquipStatus(theme.Id);

        // Lock or unlock UI based on the skin's unlock status
        Lock.SetActive(!IsUnlocked);

        btn.onClick.RemoveAllListeners();
        if (!IsUnlocked)
        {
            btn.onClick.AddListener(OpenPurchase);
        }
        else
        {
            //btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(EquipSkin);
        }

        //Debug.Log(this.skin.name + " " + skin.id + " " + isEquiped);
        // If equipped, show the tick and hide it for other skins of the same type
        if (IsEquiped)
        {
            SetSkinEquippedState();
        }
        gameObject.SetActive(true);
    }

    public void OpenPurchase()
    {
        Debug.Log("OK");
        BackpackUIController.Instance.OpenPurchase(this);
    }

    public void EquipSkin()
    {
        List<ThemeBallUI> themeUIs = FindObjectsOfType<ThemeBallUI>()
            .Where(_ => _.Theme.SkinType == Theme.SkinType)
            .ToList();
        themeUIs.First(skin => skin.Tick.active).Tick.SetActive(false);
        Tick.SetActive(true);
        SetEquippedSkin();
    }

    public void GetUnlockAndEquipStatus(string getId)
    {
        isUnlocked = SkinDataController.Instance.SkinCollection.GetSkinUnlockStatus(getId);
        isEquiped = SkinDataController.Instance.SkinCollection.GetSkinEquipStatus(getId);
    }

    public void SetSkinEquippedState()
    {
        List<ThemeBallUI> skinUIs = FindObjectsOfType<ThemeBallUI>()
            .Where(ui => ui.theme != null && ui.theme.SkinType == theme.SkinType)
            .ToList();

        // Reset all ticks and enable only the equipped one
        foreach (ThemeBallUI ui in skinUIs)
        {
            if (ui.tick != null)
            {
                ui.tick.SetActive(false);
            }
        }

        if (tick != null)
        {
            tick.SetActive(true);
        }

        SetEquippedSkin();
    }


    public void SetEquippedSkin()
    {
        SkinDataController.Instance.SkinCollection.EquipSkin(theme.Id, theme.SkinType);
    }


    public void Equip()
    {
        List<ThemeBallUI> skinUIs = FindObjectsOfType<ThemeBallUI>()
            .Where(_ => _.theme.SkinType == theme.SkinType)
            .ToList();
        skinUIs.First(skin => skin.tick.active).tick.SetActive(false);
        tick.SetActive(true);

        SetEquippedSkin();
    }

    public void UnlockSkin()
    {
        Debug.Log("Unlock ball theme: " + this.Title.text);
        SkinDataController.Instance.SkinCollection.UnlockSkin(theme.Id);

        GetUnlockAndEquipStatus(theme.Id);

        _lock.SetActive(!isUnlocked);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Equip);
    }

    private void OnChangeMenuTab(int id)
    {
        //Debug.Log(id, this);
        if (id == 1) Init(Theme);
    }
}
