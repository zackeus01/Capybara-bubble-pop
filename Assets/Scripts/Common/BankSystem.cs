public class BankSystem : Singleton<BankSystem>
{
    public int Gem { get { return CurrencyDataController.Instance.CurrencyData.Gem; } }
    public int Coin { get { return CurrencyDataController.Instance.CurrencyData.Coin; } }

    public void DepositGem(int value)
    {
        CurrencyDataController.Instance.CurrencyData.Gem += value;
        SaveData();
    }

    public void DepositCoin(int value)
    {
        CurrencyDataController.Instance.CurrencyData.Coin += value;
        SaveData();
    }


    public bool WithdrawGem(int value)
    {
        if (CurrencyDataController.Instance.CurrencyData.Gem < value)
        {
            return false;
        }
        CurrencyDataController.Instance.CurrencyData.Gem -= value;
        SaveData();
        return true;
    }


    public bool WithdrawCoin(int value)
    {
        if (CurrencyDataController.Instance.CurrencyData.Coin < value)
        {
            return false;
        }
        CurrencyDataController.Instance.CurrencyData.Coin -= value;
        SaveData();
        return true;
    }

    public void SaveData()
    {
        UIEvent.OnCurrencyChanged.Invoke();
        CurrencyDataController.Instance.SaveData();
    }

    public bool CheckCoinWithValue(int value)
    {
        if (CurrencyDataController.Instance.CurrencyData.Coin < value) { return false; }
        return true;
    }
}
