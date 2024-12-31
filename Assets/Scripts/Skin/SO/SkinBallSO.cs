using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinBall_", menuName = "Bubble Shooter/SkinBallSO")]
public class SkinBallSO : SkinSO
{
    [Header("Ball")]
    [SerializeField] private BallType ballType;
    [SerializeField] private BallColor color;

    public SkinBallSO(string id, string name, SkinType skinType, BallType ballType, BallColor color) : base(id, name, skinType)
    {
        this.ballType = ballType;
        this.color = color;
    }

    public BallType BallType { get { return ballType; } }
    public BallColor BallColor { get { return color; } }


}
