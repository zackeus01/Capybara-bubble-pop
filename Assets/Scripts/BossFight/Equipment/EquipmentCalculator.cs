using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipmentCalculator
{
    private const int BaseCoinCost = 100;
    private const int CoinCostIncrement = 10;

    // Calculator Cost Upgrade
    public static int CalculateCoinCost(int level)
    {
        int coinCost = BaseCoinCost;

        for (int i = 2; i <= level; i++)
        {
            coinCost += (i + 1) * CoinCostIncrement;
        }

        return coinCost;
    }
    // Calculator ATK Sword
    public static float CalculateBaseDameAtkSword(int level, float baseDamage)
    {
        float dameAtk = baseDamage;

        for (int i = 2; i <= level; i++)
        {
            dameAtk += i * 5;
        }

        return dameAtk;
    }
    // Calculator ATK Bow
    public static float CalculateBaseDameAtkBow(int level, float baseDamage)
    {
        float dameAtk = baseDamage;

        for (int i = 2; i <= level; i++)
        {
            dameAtk += i * 2;
        }

        return dameAtk;
    }
    // Calculator Crit Bow
    public static float CalculateBaseDameCritBow(int level, float baseDamage)
    {
        float dameCrit = baseDamage;

        for (int i = 2; i <= level; i++)
        {
            dameCrit += 0.03f;
        }

        return dameCrit;
    }
    // Calculator Element Bow
    public static float CalculateBaseDameElementBow(int level, float baseDamage)
    {
        float dameElement = baseDamage;

        for (int i = 2; i <= level; i++)
        {
            dameElement += i;
        }

        return dameElement;
    }
    // Calculator Element Magic
    public static float CalculateBaseDameElementMagic(int level, float baseDamage)
    {
        float dameElement = baseDamage;

        for (int i = 2; i <= level; i++)
        {
            dameElement += i * 3;
        }

        return dameElement;
    }
    // Calculator Base HP
    public static float CalculateBaseHP(int level, float hpBase)
    {
        float hp = hpBase;

        for (int i = 2; i <= level; i++)
        {
            hp += i * 10;
        }

        return hp;
    }
    // Calculator Base DEF
    public static float CalculateBaseDEF(int level, float defBase)
    {
        float def = defBase;

        for (int i = 2; i <= level; i++)
        {
            def += i * 5;
        }

        return def;
    }
    // Calculator Base RES
    public static float CalculateBaseRES(int level, float resBase)
    {
        float res = resBase;

        for (int i = 2; i <= level; i++)
        {
            res += i * 3;
        }

        return res;
    }

}
