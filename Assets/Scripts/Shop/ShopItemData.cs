using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Type
{
    Gold,
    Gem

}

[CreateAssetMenu(menuName = "Shopdata")]

public class ShopItemData : ScriptableObject
{


    public string bundleName;
    public SpriteRenderer spriteRenderer;
    public Type type;


}
