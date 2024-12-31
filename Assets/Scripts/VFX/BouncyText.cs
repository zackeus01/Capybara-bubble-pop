using DG.Tweening;
using UnityEngine;

public class BouncyText : TextUiVFX
{
    [Header("Bound In Direction")]
    [SerializeField]
    private BoundDirection directionIn;
    public override void DoTextVFX()
    {
        if (directionIn == BoundDirection.Down)
        {
            textImg.transform.position += new Vector3(0f, 8f, 0f);
        }
        else
        {
            textImg.transform.position -= new Vector3(0f, 8f, 0f);
        }

        textImg.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(textImg.transform.DOMove(initialPos, 1.3f).SetEase(Ease.OutBounce))
            .Join(textImg.transform.DOScale(initialScale, 0.5f))
            .Append(textImg.DOFade(0f, 0.5f).SetEase(Ease.InQuart));

        //.AppendInterval(0.5f)
        textSequence.OnComplete(() =>
        {
            ResetValue();
        });
    }
}

public enum BoundDirection
{
    Up,
    Down
}
