using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
    [Header("--------------------Infor player-----------------")]
    [SerializeField] private int playerLevel = 1;
    [SerializeField] private int priceUpgrade = 300;  // price lv1-lv2
    [SerializeField] private int maxLevel = 20;

    public int PlayerLevel
    {
        get { return playerLevel; }
        private set { playerLevel = Mathf.Clamp(value, 1, maxLevel); }
    }

    public int PriceUpgrade
    {
        get { return priceUpgrade; }
    }

    public int MaxLevel
    {
        get { return maxLevel; }
    }

    //
    public PlayerDataManager()
    {
        playerLevel = 1;
        priceUpgrade = 300; 
    }

    public void UpgradeLevel()
    {
        if (playerLevel < MaxLevel)
        {
            playerLevel++;
            UpdatePriceUpgrade();
        }
        else
        {
            Debug.Log("Player has reached max level.");
        }
    }

    private void UpdatePriceUpgrade()
    {
        // Từ cấp 2 trở đi, giá nâng cấp mỗi cấp tăng thêm 100 so với cấp trước
        if (playerLevel > 1) // Chỉ thay đổi khi từ cấp 2 trở lên
        {
            priceUpgrade += 100;
        }
    }
}
