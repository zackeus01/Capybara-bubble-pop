using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUIController : Singleton<BackpackUIController>
{
    //[Header("Currency")]
    //[SerializeField] private TextMeshProUGUI txtCoin;
    //[SerializeField] private TextMeshProUGUI txtGem;

    [Header("Shop UI")]
    [SerializeField] private List<BackpackTabUI> tabs;
    [SerializeField] private BackpackTabUI currentTabUI;
    [SerializeField] private GameObject purchaseTab;


    private Dictionary<SkinType, List<SkinSO>> skinsByGroup;
    public Dictionary<SkinType, List<SkinSO>> SkinsByGroup { get { return skinsByGroup; } }

    private void Start()
    {
        purchaseTab.gameObject.SetActive(false);
        skinsByGroup = GroupSkinsByTypeAndTheme(SkinDataController.Instance.SkinDatas);
        //InitAllTabs();
        SetCurrentTab(0);
    }

    public Dictionary<SkinType, List<SkinSO>> GroupSkinsByTypeAndTheme(List<SkinSO> skins)
    {
        // Group skins by SkinType
        var groupedSkins = skins
            .GroupBy(skin => skin.SkinType)
            .ToDictionary(
                themeGroup => themeGroup.Key, // Key is the SkinType
                themeGroup => themeGroup.ToList() // List of skins under each type
            );

        return groupedSkins;
    }

    public void InitAllTabs()
    {
        foreach (var tab in tabs)
        {
            //tab.InitData();
            tab.gameObject.SetActive(true);
        }
        SetCurrentTab(0);
    }

    public void SetCurrentTab(int index)
    {
        for (int i=0; i<tabs.Count; i++)
        {
            tabs[i].gameObject.SetActive(i == index);
            if (i == index)
            {
                tabs[i].GetComponent<BackpackTabUI>().InitData();
            }
        }
    }

    public void OpenPurchase(SkinUnlockableUI skin)
    {
        Debug.Log("Open Purchase");

        // Show purchase tab
        purchaseTab.GetComponent<PurchaseWindow>().InitData(skin);
        purchaseTab.SetActive(true);
    }
    
    public void OpenPurchase(ThemeBallUI theme)
    {
        Debug.Log("Open Purchase");

        // Show purchase tab
        purchaseTab.GetComponent<PurchaseWindow>().InitData(theme);
        purchaseTab.SetActive(true);
    }

    public void ClosePurchase()
    {
        purchaseTab.SetActive(false);
    }




}
