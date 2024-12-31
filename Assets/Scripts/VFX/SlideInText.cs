using DG.Tweening;
using System;
using UnityEngine;

public class SlideInText : TextUiVFX
{
    [Header("Slide In Direction")]
    [SerializeField]
    private SlideDirection directionIn;
    [Header("Slide Out Direction")]
    [SerializeField]
    private SlideDirection directionOut;

    private Vector3 destination;
    public override void DoTextVFX()
    {
        SetStartPos();

        Sequence textSequence = DOTween.Sequence();

        textSequence.Append(textImg.transform.DOMove(destination, 0.5f).SetEase(Ease.OutQuad))
            .Join(textImg.transform.DOScale(new Vector3(0.9f, 1f, 1f), 0.4f).SetDelay(0.2f))
            .Append(textImg.transform.DOMove(initialPos, 0.4f).SetEase(Ease.InQuart))
            .Join(textImg.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetDelay(0.2f))
            .Append(textImg.DOFade(0f, 0.5f));

        textSequence.OnComplete(() =>
        {
            ResetValue();
        });
    }

    private void SetStartPos()
    {
        switch (directionIn)
        {
            case SlideDirection.Left:
                {
                    textImg.transform.position += new Vector3(-5f, 0f, 0f);
                    destination = initialPos + new Vector3(1f, 0f, 0f);
                    break;
                }
            case SlideDirection.Right:
                {
                    textImg.transform.position += new Vector3(5f, 0f, 0f);
                    destination = initialPos + new Vector3(-1f, 0f, 0f);
                    break;
                }
            case SlideDirection.Up:
                {
                    textImg.transform.position += new Vector3(0f, 5f, 0f);
                    destination = initialPos + new Vector3(0f, -1f, 0f);
                    break;
                }
            case SlideDirection.Down:
                {
                    textImg.transform.position += new Vector3(0f, -5f, 0f);
                    destination = initialPos + new Vector3(0f, 1f, 0f);
                    break;
                }
        }


    }
}

public enum SlideDirection
{
    Left,
    Right,
    Up,
    Down
}
