using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeMultiplier", menuName = "Player/PlayerUpgradeMultiplier")]
public class PlayerUpgradeMultiplier :ScriptableObject
{
    [SerializeField] private float hpMultiplier = 10f;
    [SerializeField] private float attackMultiplier = 1f;
    [SerializeField] private float defenseMultiplier = 3f;
    [SerializeField] private float eleAtkMultiplier = 0f;
    [SerializeField] private float eleResMultiplier = 2f;
    [SerializeField] private float manaResMultiplier = 5f;
    [SerializeField] private float critMultiplier = 5f;
    [SerializeField] private int coinMultiplier = 10;

    public float HpMultiplier { get { return hpMultiplier; } set { hpMultiplier = value; } }
    public float AttackMultiplier { get { return attackMultiplier; } set { attackMultiplier = value; } }
    public float DefenseMultiplier { get { return defenseMultiplier; } set { defenseMultiplier = value; } }
    public float EleAtkMultiplier { get { return eleAtkMultiplier; } set { eleAtkMultiplier = value; } }
    public float EleResMultiplier { get { return eleResMultiplier; } set { eleResMultiplier = value; } }
    public float ManaMultiplier { get { return manaResMultiplier; } set { manaResMultiplier = value; } }
    public float CritMultiplier { get { return critMultiplier; } set { critMultiplier = value; } }
    public int CoinMultiplier { get { return coinMultiplier; } set { coinMultiplier = value; } }

}
