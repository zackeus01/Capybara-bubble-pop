using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridUI : MonoBehaviour
{
    [SerializeField] private BallType ballType = BallType.Normal;
    [SerializeField] private BallColor ballColor = BallColor.None;
    [SerializeField] private bool hasBall;
    [SerializeField] private Image Ball;
    [SerializeField] private Image Cover;
    [SerializeField] private Image cellImage;

    public BallColor BallColor { get { return ballColor; } }
    public BallType BallType { get { return ballType; } }
    public bool HasBall { get { return hasBall; } }

    public void SetBall(BallColor ballColor, BallType ballType)
    {
        this.ballColor = ballColor;
        this.ballType = ballType;
    }

    public void SelectBall()
    {
        LevelEditorFieldUI.Instance.SelectedCell = this;
        //Debug.Log(LevelEditorFieldUI.Instance.SelectedCell, LevelEditorFieldUI.Instance.SelectedCell);

        if (LevelEditorFieldUI.Instance.CurrentBallColor.Equals(BallColor.None)
            && LevelEditorFieldUI.Instance.CurrentBallType.Equals(BallType.Normal))
        {
            ResetGrid();
            return;
        }

        SetBallColor(LevelEditorFieldUI.Instance.CurrentBallColor);
        SetBallType(LevelEditorFieldUI.Instance.CurrentBallType);
    }

    public void ChangeHaveBall()
    {
        hasBall = !hasBall;

        cellImage.gameObject.SetActive(!hasBall);

        if (!hasBall)
        {
            ballColor = BallColor.None;
            ballType = BallType.Normal;
            Ball.gameObject.SetActive(false);
            if (ballType.Equals(BallType.Normal)) Cover.gameObject.SetActive(false);
        }
    }

    public void ResetGrid()
    {
        hasBall = false;
        ballColor = BallColor.None;
        ballType = BallType.Normal;
        cellImage.gameObject.SetActive(true);
        Ball.gameObject.SetActive(false);
        if (ballType.Equals(BallType.Normal)) Cover.gameObject.SetActive(false);
    }

    public void SetSprite()
    {
        hasBall = true;
        cellImage.gameObject.SetActive(false);
        Cover.gameObject.SetActive(!ballType.Equals(BallType.Normal));
        Ball.gameObject.SetActive(!ballColor.Equals(BallColor.None));
        Cover.sprite = LevelEditorFieldUI.Instance.CoverImage.First(type => type.BallType.Equals(ballType)).Sprite;
        Ball.sprite = LevelEditorFieldUI.Instance.BallImage.First(type => type.BallColor.Equals(ballColor)).Sprite;
    }

    public void SetBallColor(BallColor color)
    {
        if (ballType.Equals(BallType.Normal) || ballType.Equals(BallType.Grass) || ballType.Equals(BallType.Web))
        {
            ballColor = color;
            Ball.gameObject.SetActive(true);
            SetSprite();
        }
        else
        {
            ballColor = BallColor.None;
            Ball.gameObject.SetActive(true);
            SetSprite();
        }
    }

    public void SetBallType(BallType type)
    {
        ballType = type;

        if (!(ballType.Equals(BallType.Normal)
            || ballType.Equals(BallType.Grass)
            || ballType.Equals(BallType.Web)))
        {
            ballColor = BallColor.None;
            SetSprite();
        }

        Cover.gameObject.SetActive(true);
        SetSprite();
    }
}
