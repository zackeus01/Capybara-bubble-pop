using UnityEngine;

public class HelperController : Singleton<HelperController>
{
    public int GetHelperCount(BallType type)
    {
        switch (type)
        {
            case BallType.Bomb:
                return PlayerPrefs.GetInt(PlayerPrefsConst.BOMB_HELPER, 0);
            case BallType.Firework:
                return PlayerPrefs.GetInt(PlayerPrefsConst.FIREWORK_HELPER, 0);
            case BallType.Ziczac:
                return PlayerPrefs.GetInt(PlayerPrefsConst.ZICZAC_HELPER, 0);
            case BallType.Rainbow:
                return PlayerPrefs.GetInt(PlayerPrefsConst.RAINBOW_HELPER, 0);
            default:
                return 0;
        }
    }

    public bool AddHelper(BallType type, int amount)
    {
        int value = GetHelperCount(type) + amount;

        switch (type)
        {
            case BallType.Bomb:
                PlayerPrefs.SetInt(PlayerPrefsConst.BOMB_HELPER, value);
                return true;
            case BallType.Firework:
                PlayerPrefs.SetInt(PlayerPrefsConst.FIREWORK_HELPER, value);
                return true;
            case BallType.Ziczac:
                PlayerPrefs.SetInt(PlayerPrefsConst.ZICZAC_HELPER, value);
                return true;
            case BallType.Rainbow:
                PlayerPrefs.SetInt(PlayerPrefsConst.RAINBOW_HELPER, value);
                return true;
        }

        return false;
    }

    public bool UseHelper(BallType type)
    {
        int value = GetHelperCount(type); 
        if (value <= 0) return false;    

        switch (type)
        {
            case BallType.Bomb:
                PlayerPrefs.SetInt(PlayerPrefsConst.BOMB_HELPER, value - 1);
                break;
            case BallType.Firework:
                PlayerPrefs.SetInt(PlayerPrefsConst.FIREWORK_HELPER, value - 1);
                break;
            case BallType.Ziczac:
                PlayerPrefs.SetInt(PlayerPrefsConst.ZICZAC_HELPER, value - 1);
                break;
            case BallType.Rainbow:
                PlayerPrefs.SetInt(PlayerPrefsConst.RAINBOW_HELPER, value - 1);
                break;
            default:
                return false; 
        }

        return true;
    }

}
