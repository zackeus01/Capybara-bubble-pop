using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinDataController : Singleton<SkinDataController>
{
    [SerializeField] private List<SkinSO> skinDatas = new List<SkinSO>();

    private Dictionary<SkinType, ThemeSO> equippedSkins = new Dictionary<SkinType, ThemeSO>();

    private SkinDataCollection skinCollection;

    //public Dictionary<SkinType, List<SkinData>> EquippedSkins
    //{
    //    get { return skinCollection.GetEquippedSkin(); }
    //}

    public Dictionary<SkinType, ThemeSO> EquippedSkins
    {
        get { return equippedSkins; }
    }

    public SkinDataCollection SkinCollection
    {
        get { return skinCollection; }
    }

    public List<SkinSO> SkinDatas { get { return skinDatas; } }

    private void Start()
    {
        Load();
        InitData();
        Save();
        SkinEvent.OnNewSkinEquip.AddListener(SetEquippedSkins);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #region Save & Load
    public void Load()
    {
        SaveSystem.LoadData(GameConst.SKIN_FILE, ref skinCollection);
    }

    public void Save()
    {
        SaveSystem.SaveData(GameConst.SKIN_FILE, skinCollection);
    }

    public void InitData()
    {
        foreach (var skin in skinDatas)
        {
            SkinData existedSKin = skinCollection.SkinDatas.FirstOrDefault(x => x.Id.Equals(skin.Id));
            if (existedSKin == null)
            {
                skinCollection.AddSkinData(skin.Name, skin.Id, skin.SkinType);
            }
        }

        if (EquippedSkins == null || !EquippedSkins.ContainsKey(SkinType.BALL))
        {
            List<string> ids = skinDatas
                .Where(skin => (skin.Id.Contains("DEFAULT") || skin.Id.Contains("SPECIAL"))
                    && skin.SkinType.Equals(SkinType.BALL))
                .Select(x => x.Id)
                .ToList();

            List<SkinData> defaultSkins = skinCollection.SkinDatas
                .Where(skin => ids.Contains(skin.Id))
                .ToList();

            foreach (var defaultSkin in defaultSkins)
            {
                defaultSkin.UnlockSkin();
                defaultSkin.EquipSkin();
            }
        }

        if (EquippedSkins == null || !EquippedSkins.ContainsKey(SkinType.BACKGROUND))
        {
            SkinData defaultSkin = skinCollection.SkinDatas.Find(
                    skin => skin.Id.Equals(skinDatas.Find(skin => skin.Id.Contains("DEFAULT") && skin.SkinType.Equals(SkinType.BACKGROUND)).Id));
            defaultSkin.UnlockSkin();
            defaultSkin.EquipSkin();
        }

        if (EquippedSkins == null || !EquippedSkins.ContainsKey(SkinType.MENUBACKGROUND))
        {
            SkinData defaultSkin = skinCollection.SkinDatas.Find(
                    skin => skin.Id.Equals(skinDatas.Find(skin => skin.Id.Contains("DEFAULT") && skin.SkinType.Equals(SkinType.MENUBACKGROUND)).Id));
            defaultSkin.UnlockSkin();
            defaultSkin.EquipSkin();
        }
    }
    #endregion

    public void SetEquippedSkins()
    {
        //Debug.Log("On Equipped Skin");

        Dictionary<SkinType, List<SkinData>> equippedSkinCollection = skinCollection.GetEquippedSkin();

        foreach (var kvp in equippedSkinCollection)
        {
            List<SkinData> currentSkin = kvp.Value;

            //Create a new skin list (TO CONTAINS SPECIAL BALLS)
            List<SkinSO> listSkin = new List<SkinSO>();

            foreach (var skin in currentSkin)
            {
                //Normal Ball Skin Theme
                if (skinDatas.Find(s => s.Id.Equals(skin.Id)) is ThemeSO theme)
                {
                    foreach (var themeSkin in theme.Skins)
                    {
                        listSkin.Add(themeSkin);
                    }
                }
                //SPECIAL balls who don't have a theme
                else
                {
                    listSkin.Add(skinDatas.Find(s => s.Id.Equals(skin.Id)));
                }
            }

            //Create new Theme for the ball skins
            ThemeSO newTheme = new ThemeSO($"EQUIPPED_{kvp.Key}", $"Equipped {kvp.Key} Skins", kvp.Key, listSkin);

            if (equippedSkins.ContainsKey(kvp.Key))
            {
                equippedSkins[kvp.Key] = newTheme;
            }
            else
            {
                equippedSkins.Add(kvp.Key, newTheme);
            }
        }

        foreach (var kvp in equippedSkins)
        {
            //Debug.Log("Key: " + kvp.Key);
            ThemeSO theme = kvp.Value as ThemeSO;
            foreach (var skin in theme.Skins)
            {
                //Debug.Log("- " + skin.Id);
            }
        }
    }

    public SkinSO GetCurrentThemeSkinSO(SkinType skinType)
    {
        //switch (skinType)
        //{
        //    case SkinType.BALL:
        //        return GetCurrentBallSkin();
        //    default:
        //        return GetCurrentOtherSkin(skinType);
        //}
        return equippedSkins[skinType];
    }

    //private ThemeSO GetCurrentBallSkin()
    //{
    //    List<SkinData> currentSkin = EquippedSkins[SkinType.BALL];

    //    //Create a new skin list (TO CONTAINS SPECIAL BALLS)
    //    List<SkinSO> listSkin = new List<SkinSO>();

    //    foreach (var skin in currentSkin)
    //    {
    //        //Normal Ball Skin Theme
    //        if (skinDatas.Find(s => s.Id.Equals(skin.Id)) is ThemeSO theme)
    //        {
    //            foreach (var themeSkin in theme.Skins)
    //            {
    //                listSkin.Add(themeSkin);
    //            }
    //        }
    //        //SPECIAL balls who dont have a theme
    //        else
    //        {
    //            listSkin.Add(skinDatas.Find(s => s.Id.Equals(skin.Id)));
    //        }
    //    }

    //    //Create new Theme for the ball skins
    //    ThemeSO eqippedSkins = new ThemeSO("EQUIPPED_BALL", "Equipped Ball Skins", SkinType.BALL, listSkin);

    //    return eqippedSkins;
    //}

    //private SkinSO GetCurrentOtherSkin(SkinType skinType)
    //{
    //    List<SkinData> currentSkin = EquippedSkins[skinType];
    //    return (skinDatas.Find(s => s.Id.Equals(currentSkin[0].Id)));
    //}

}
