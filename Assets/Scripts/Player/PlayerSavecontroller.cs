using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSavecontroller : Singleton<PlayerSavecontroller>

{
    [SerializeField] private PlayerDataCollection playerDataCollection;
    public PlayerDataCollection PlayerDataCollection { get { return playerDataCollection; } }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadData();
    }

    public void SaveDataPlayer(float currentLv, float currenHp, float currentAtk, float currentDef, float currentElDame, float currentRes,  float currentCrit, float currentMana)
    {
        playerDataCollection.CurrentLv = currentLv;
        playerDataCollection.CurrentDef = currentDef;
        playerDataCollection.CurrentRes = currentRes;
        playerDataCollection.CurrentMana = currentMana;
        playerDataCollection.CurrentCrit = currentCrit;
        playerDataCollection.CurrenHp = currenHp;
        playerDataCollection.CurrentAtk = currentAtk;
        playerDataCollection.CurrentElDame = currentElDame;

        SaveData();

    }

    #region Save & Load
    private void LoadData()
    {
        SaveSystem.LoadData(GameConst.PLAYERSTATUS_FILE, ref playerDataCollection);
    }

    private void SaveData()
    {
        SaveSystem.SaveData(GameConst.PLAYERSTATUS_FILE, playerDataCollection);
    }

    #endregion

}
