using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrows", menuName = "Boss/Equipment/Arrow")]
public class ArrowSO : ScriptableObject
{
    [SerializeField] private Sprite fireArrow;
    [SerializeField] private Sprite earthArrow;
    [SerializeField] private Sprite waterArrow;
    [SerializeField] private Sprite lightningArrow;
    [SerializeField] private Sprite woodArrow;

    public Sprite FireArrow { get { return fireArrow; } }
    public Sprite EarthArrow { get { return earthArrow; } }
    public Sprite WaterArrow { get { return waterArrow; } }
    public Sprite LightningArrow { get { return lightningArrow; } }
    public Sprite WoodArrow { get { return woodArrow; } }

    public Sprite GetArrowSprite(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                return fireArrow;
            case ElementType.Earth:
                return earthArrow;
            case ElementType.Water:
                return waterArrow;
            case ElementType.Thunder:
                return lightningArrow;
            case ElementType.Plant:
                return woodArrow;
            default: return null;
        }
    }
}
