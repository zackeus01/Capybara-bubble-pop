using DG.Tweening;
using UnityEngine;

public class PopUpText : TextUiVFX
{
    public override void DoTextVFX()
    {
        textImg.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        textImg.DOFade(0f, 0f);

        Vector3 destination = textImg.transform.localPosition + new Vector3(0f, 1.5f, 0f);

        Sequence textSequence = DOTween.Sequence();

        textSequence.Append(textImg.transform.DOScale(initialScale, 1.2f).SetEase(Ease.OutBack))
            .Join(textImg.DOFade(1f, 1f).SetEase(Ease.OutQuart))
            .Append(textImg.DOFade(0f, 0.3f))
            .Join(textImg.transform.DOMove(destination, 0.3f));

        textSequence.OnComplete(() =>
        {
            ResetValue();
        });
    }
}
