using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment", menuName = "Boss/Equipment")]
public class EquipmentDataSO : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private List<string> nameEquipment;
    [SerializeField] private List<string> description;
    [SerializeField] private Sprite avatar;
    [SerializeField] private ElementType elementType;
    [SerializeField] private Rarity rarity;
    [SerializeField] private WaysToEarnEquipment ways;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private bool isLocked;
    [SerializeField] private Sprite[] listImage;
    public Sprite[] ListImage => listImage;
    public Sprite Avatar => avatar;
    public int MaxLevel { get { return maxLevel; } set { maxLevel = value; } }
    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
    public bool IsLocked { get { return isLocked; } }
    public string Id { get { return id; } set { id = value; } }
    public List<string> Name { get { return nameEquipment; } set { nameEquipment = value; } }
    public List<string> Description { get { return description; } set { description = value; } }
    public Rarity Rarity { get { return rarity; } set { rarity = value; } }
    public WaysToEarnEquipment Ways { get { return ways; } set { ways = value; } }
    public ElementType ElementType { get { return elementType; } set { elementType = value; } }
    public EquipmentType EquipmentType { get { return equipmentType; } set { equipmentType = value; } }

}
