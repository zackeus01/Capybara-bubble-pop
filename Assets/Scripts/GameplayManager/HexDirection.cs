using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexDirection
{
    NE,//(North East) trên phải 
    E,//(East) phải
    SE,//(South East) dưới phải
    SW,//(South West) dưới trái
    W,//(West) trái
    NW//(North West) trên trái
}

public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection hexDirection)
    {
        return (int)hexDirection < 3 ? (hexDirection + 3) : (hexDirection - 3);
    }
}
