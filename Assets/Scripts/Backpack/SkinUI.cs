using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinUI : MonoBehaviour
{
    [SerializeField] protected Image bg;

    [SerializeField] protected SkinSO skin;
    public SkinSO Skin { get { return skin; } set { skin = value; } }

    public virtual void Init(SkinSO skin)
    {
        this.skin = skin;

        bg.sprite = skin.Sprite;

    }

}