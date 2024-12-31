using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IAPPack : MonoBehaviour
{
    public int rewardGem;
    public int rewardCoin;
    public RewardType rewardType;
    public TextMeshProUGUI boomtxt;
    public TextMeshProUGUI rainbowtxt;
    public TextMeshProUGUI ziczactxt;
    public TextMeshProUGUI fireworktxt;

  

    [Header("With draw")]
    [SerializeField] public TextMeshProUGUI Coin;
    [SerializeField] public TextMeshProUGUI Gem;



    [Flags]
    public enum RewardType
    {
        None = 0,
        COIN = 1 << 0,           
        GEM = 1 << 1,            
        RemoveADS = 1 << 2,      
        IAPCOIN = 1 << 3,         
        IAPGEM = 1 << 4,
        HELPER = 1 << 5,
        SKIN = 1 << 6,
        START = 1 << 7,
        EXCOIN = 1 << 8,
    }
}
