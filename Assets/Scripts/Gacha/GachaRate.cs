using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum R
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]

public class GachaRate
{
    [SerializeField] private string rarity;
    [SerializeField][Range(1,100)] private int rate;
    [SerializeField] private EquipmentDataSO[] reward;
    public R _rarity;

    public int Rate { get { return rate; } set {  rate = value; } }
    public string Rarity { get { return rarity; } set { rarity = value; } }

    public EquipmentDataSO[] Reward { get {  return reward; } set {  reward = value; } }
}
