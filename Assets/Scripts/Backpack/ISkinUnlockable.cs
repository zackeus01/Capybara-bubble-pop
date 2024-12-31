using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISkinUnlockable
{
    public GameObject Tick { get; }
    public GameObject Lock { get; }

    public Button Button { get; }

    public bool IsUnlocked { get; set; }
    public bool IsEquiped { get; set; }

    public void GetUnlockAndEquipStatus(string getId);
    public void SetSkinEquippedState();
    public void SetEquippedSkin();
    public void Equip();
    public void UnlockSkin();


}
