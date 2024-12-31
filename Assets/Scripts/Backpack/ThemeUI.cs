using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class ThemeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform skinContainer;
    [SerializeField] private SkinUI skinUIPrefab;
    [SerializeField] protected ThemeSO theme;

    public ThemeSO Theme { get { return theme; } set { theme = value; } }
    public TextMeshProUGUI Title { get { return title; } set { title = value; } }
    public SkinUI SkinUIPrefab { get { return skinUIPrefab; } set { skinUIPrefab = value; } }
    public Transform SkinContainer { get { return skinContainer; } set { skinContainer = value; } }

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


    public virtual void Init(ThemeSO theme)
    {
        ClearPool();

        this.theme = theme;
        //skin.AddSkinData();
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            title.text = theme.Name;
        }
        else
        {
            switch (theme.name)
            {
                case "Default":
                    title.text = "Mặc định";
                    break;
                case "Test":
                    title.text = "Kiểm tra 1";
                    break;
            }
        }
        // Set unlock and equip status based on skin type
        if (theme.IsHidden) this.gameObject.SetActive(false);
        gameObject.SetActive(true);

        foreach (var skin in theme.Skins)
        {
            SkinUnlockableUI newSkin = skinUIPrefab as SkinUnlockableUI;
            Instantiate(newSkin, skinContainer);
            newSkin.Init(skin);
        }
    }

    private void OnChangeMenuTab(int id)
    {
        //Debug.Log(id, this);
        if (id == 1) Init(Theme);
    }
}
