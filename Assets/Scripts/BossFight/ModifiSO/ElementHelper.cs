using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementHelper
{
    private static readonly Dictionary<ElementType, ElementType> counterRelations = 
        new Dictionary<ElementType, ElementType>
    {
        { ElementType.Fire, ElementType.Plant },   // Hỏa khắc Thảo
        { ElementType.Water, ElementType.Fire },  // Thủy khắc Hỏa
        { ElementType.Thunder, ElementType.Water }, // Lôi khắc Thủy
        { ElementType.Earth, ElementType.Thunder }, // Thổ khắc Lôi
        { ElementType.Plant, ElementType.Earth }     // Thảo khắc Thổ
    };
    public static bool IsCounter(ElementType elementA, ElementType elementB)
    {
        return counterRelations.ContainsKey(elementA) && counterRelations[elementA] == elementB;
    }
    public static ElementType? GetCounter(ElementType element)
    {
        if (counterRelations.ContainsKey(element))
        {
            return counterRelations[element];
        }
        return null; 
    }
}
    
