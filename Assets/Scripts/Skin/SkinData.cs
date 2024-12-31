using System;
using UnityEngine;

[Serializable]
public class SkinData
{
    [SerializeField] private string id;
    [SerializeField] private string name;
    [SerializeField] private SkinType type;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isEquipped;

    public string Id { get { return id; } }
    public string Name { get { return name; } }
    public SkinType Type { get { return type; } }
    public bool IsUnlocked { get { return isUnlocked; } }
    public bool IsEquipped { get { return isEquipped; } }

    public SkinData() { }

    public SkinData(string name, string id, SkinType type)
    {
        this.name = name;
        isUnlocked = false;
        this.type = type;
        isEquipped = false;
        this.id = id;
    }

    public SkinData(string name, string id, SkinType type, bool isUnlocked, bool isEquiped) : this(name, id, type)
    {
        this.isUnlocked = isUnlocked;
        this.isEquipped = isEquiped;
    }

    public void UnlockSkin()
    {
        isUnlocked = true;
    }

    public void EquipSkin()
    {
        isEquipped = true;
        SkinEvent.OnNewSkinEquip.Invoke();
    }

    public void UnequipSkin()
    {
        isEquipped = false;
    }
}
