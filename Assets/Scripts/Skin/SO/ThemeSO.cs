using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme_", menuName = "Bubble Shooter/ThemeSO")]
public class ThemeSO : SkinSO
{
    [Header("Theme")]
    [SerializeField] private List<SkinSO> skins;

    public ThemeSO(string id, string name, string vieName, SkinType skinType, List<SkinSO> skinBallList) : base(id, name, skinType)
    {
        skins = skinBallList;
    }
    public ThemeSO(string id, string name, SkinType skinType, List<SkinSO> skinBallList) : base(id, name, skinType)
    {
        skins = skinBallList;
    }

    public List<SkinSO> Skins { get { return skins; } }

    public SkinSO GetBallSkin(BallColor color, BallType type)
    {
        if (this.SkinType != SkinType.BALL)
        {
            return null;
        }
        List<SkinBallSO> ballSkins = skins.Select(x => x as SkinBallSO).ToList();
        return ballSkins.FirstOrDefault(b => b.BallColor.Equals(color) && b.BallType.Equals(type));
    }

    public Sprite GetBallColorSprite(string id)
    {
        if (this.SkinType != SkinType.BALL)
        {
            return null;
        }
        List<SkinBallSO> ballSkins = skins.Select(x => x as SkinBallSO).ToList();
        return ballSkins.First(b => b.Id.ToUpper().Contains(id.ToUpper())).Sprite;
    }

    public Sprite GetBallColorSprite(BallColor color)
    {
        if (this.SkinType != SkinType.BALL)
        {
            return null;
        }
        List<SkinBallSO> ballSkins = skins.Select(x => x as SkinBallSO).ToList();
        return ballSkins.First(b => b.BallColor.Equals(color)).Sprite;
    }

    public Sprite GetBallTypeSprite(BallType type)
    {
        if (this.SkinType != SkinType.BALL)
        {
            return null;
        }
        List<SkinBallSO> ballSkins = skins.Select(x => x as SkinBallSO).ToList();
        return ballSkins.First(b => b.BallType.Equals(type)).Sprite;
    }


}
