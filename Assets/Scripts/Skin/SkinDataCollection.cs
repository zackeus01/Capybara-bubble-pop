using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SkinDataCollection
{
    [SerializeField] private List<SkinData> skinDatas;

    public SkinDataCollection()
    {
        skinDatas = new List<SkinData>();
    }

    public List<SkinData> SkinDatas { get { return skinDatas; } }

    public void AddSkinData(string name, string id, SkinType type)
    {
        if (skinDatas.FirstOrDefault(skin => skin.Id.Equals(id)) != null) return;
        skinDatas.Add(new SkinData(name, id, type));
    }
    public void UnlockSkin(string id)
    {
        Debug.Log("Unlock Skin: " + id);
        SkinData bd = skinDatas.FirstOrDefault(_ => _.Id == id);
        if (bd != null)
        {
            bd.UnequipSkin();
            bd.UnlockSkin();
        }
    }
    public void EquipSkin(string id, SkinType type)
    {
        // Find the skin to be equipped
        SkinData skinToEquip = skinDatas.FirstOrDefault(skin => skin.Id.Equals(id) && skin.Type.Equals(type));
        if (skinToEquip == null) return;

        skinDatas.FirstOrDefault(skin => skin.IsEquipped && skin.Type == type && !skin.Id.Contains("SPECIAL"))?.UnequipSkin();
        skinToEquip.EquipSkin();
    }
    public bool GetSkinUnlockStatus(string id)
    {
        SkinData bd = skinDatas.FirstOrDefault(_ => _.Id != null && _.Id.Equals(id));
        return (bd != null) ? bd.IsUnlocked : false;
    }
    public bool GetSkinEquipStatus(string id)
    {
        SkinData bd = skinDatas.FirstOrDefault(_ => _.Id.Equals(id));
        return (bd != null) ? bd.IsEquipped : false;
    }
    public Dictionary<SkinType, List<SkinData>> GetEquippedSkin()
    {
        Dictionary<SkinType, List<SkinData>> dic = SkinDatas
            .Where(skin => skin.IsEquipped) // Filter only equipped skins
            .GroupBy(skin => skin.Type) // Group by SkinType
            .ToDictionary(group => group.Key, group => group.ToList()); // Create a list of SkinData for each SkinType

        return dic;
    }
}
