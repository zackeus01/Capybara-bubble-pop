using UnityEngine;

[CreateAssetMenu(fileName = "Skin_", menuName = "Bubble Shooter/SkinSO")]
public class SkinSO : ScriptableObject
{
    [Header("Attribute")]
    [SerializeField] private string id;
    [SerializeField] private string name;
    [SerializeField] private string vieName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private SkinType skinType;

    [Header("Sell Price")]
    [SerializeField] private int goldPrice;
    [SerializeField] private int gemPrice;
    [SerializeField] private bool canUnlockByAds;
    [SerializeField] private bool isHidden = false;

    public string VieName { get { return vieName; } }
    public string Id { get { return id; } }
    public string Name { get { return name; } }
    public Sprite Sprite { get { return sprite; } }
    public SkinType SkinType { get { return skinType; } }
    public int GoldPrice { get { return goldPrice; } }
    public int GemPrice { get { return gemPrice; } }
    public bool CanUnlockByAds { get { return canUnlockByAds; } }
    public bool IsHidden { get { return isHidden; } }

    public SkinSO(string id, string name, SkinType skinType)
    {
        this.id = id;
        this.name = name;
        this.skinType = skinType;
    }
}
