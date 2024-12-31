using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CurrencyDataController : Singleton<CurrencyDataController>
{
    private CurrencyData currencyData;

    public CurrencyData CurrencyData { get { return currencyData; } }

    public void LoadData()
    {
        SaveSystem.LoadData(GameConst.CURRENCY_FILE, ref currencyData);
    }

    public void SaveData()
    {
        SaveSystem.SaveData(GameConst.CURRENCY_FILE, currencyData);
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

}

[Serializable]
public class CurrencyData
{
    public int Gem;
    public int Coin;

    public CurrencyData()
    {
        Gem = 0;
        Coin = 0;
    }
}
