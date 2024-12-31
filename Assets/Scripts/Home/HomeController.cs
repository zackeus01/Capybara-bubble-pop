using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public ScrollRect scrollRect;
    //private bool isScrolling;
    public RectTransform TopElement;
    public RectTransform LeftElement;
    public RectTransform RightElement;
    public Vector2 lastTopPos;
    public Vector2 lastLeftPos;
    public Vector2 lastRightPos;
    private void Start()
    {
        GetLastPos();
    }
    void Update()
    {
        if (scrollRect.velocity != Vector2.zero)
        {
            //isScrolling = true;
            TopElement.DOAnchorPosY(890f, 0.2f).SetEase(Ease.Linear);
            LeftElement.DOAnchorPosX(-200f, 0.2f).SetEase(Ease.Linear);
            RightElement.DOAnchorPosX(200f, 0.2f).SetEase(Ease.Linear);
        }
        else
        {
            //isScrolling = false;
            TopElement.DOAnchorPos(lastTopPos, 0.2f).SetEase(Ease.Linear);
            LeftElement.DOAnchorPos(lastLeftPos, 0.2f).SetEase(Ease.Linear);
            RightElement.DOAnchorPos(lastRightPos, 0.2f).SetEase(Ease.Linear);
        }
    }
    public void GetLastPos()
    {
        if (TopElement != null)
        {
            lastTopPos = TopElement.anchoredPosition;
        }
        if (LeftElement != null)
        {
            lastLeftPos = LeftElement.anchoredPosition;
        }
        if (RightElement != null)
        {
            lastRightPos = RightElement.anchoredPosition;
        }
    }
}
