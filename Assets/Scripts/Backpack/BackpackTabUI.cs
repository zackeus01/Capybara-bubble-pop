using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Pool;

public class BackpackTabUI : MonoBehaviour
{
    public SkinType skinType;
    public ThemeUI themeUIPrefab;
    public Transform content;

    private List<SkinSO> skinsWithoutTheme = new List<SkinSO>();

    private ObjectPool<ThemeUI> pool;

    #region ObjectPool
    private void SetupPool()
    {
        ClearPool();

        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<ThemeUI>(
            createFunc: () => Instantiate(themeUIPrefab, content),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 6
        );

    }

    public void ClearPool()
    {
        // Loop through all active objects in the pool and release them
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            var child = content.GetChild(i).GetComponent<ThemeUI>();
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
        //Debug.Log("Clear Pool");
        UIEvent.OnChangeMenuTab.RemoveListener(OnChangeMenuTab);
    }

    public void InitData()
    {
        ClearPool();

        List<SkinSO> skinList = BackpackUIController.Instance.SkinsByGroup[skinType];

        //var themeGroup = GroupSkinsByTheme(themeList);

        foreach (var skin in skinList)
        {
            ThemeSO theme = skin as ThemeSO;

            if (theme == null)
            {
                skinsWithoutTheme.Add(skin);
            }
            else
            {
                pool.Get().GetComponent<ThemeUI>().Init(theme);
            }
        }

        //if (skinsWithoutTheme.Count > 0)
        //{
        //    ThemeSO newTheme = new ThemeSO(skinType + "_NONE", skinType + "_NONE", skinType, skinsWithoutTheme);
        //    pool.Get().GetComponent<ThemeUI>().Init(newTheme);
        //}
    }

    // Function to release all SkinUI objects back to the pool when no longer needed
    public void ClearSkins()
    {
        foreach (Transform child in content)
        {
            GameObject skinUIObject = child.gameObject;
            ThemeUI themeUI = skinUIObject.GetComponent<ThemeUI>();
            if (themeUI != null)
            {
                Destroy(themeUI); // Release the GameObject back to the pool
            }
        }
    }

    private void OnChangeMenuTab(int id)
    {
        //Debug.Log(id, this);
        if (id == 1) InitData();
        else BackpackUIController.Instance.ClosePurchase();
    }
}
