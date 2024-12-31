using DG.Tweening;
using UnityEngine;

public class ArrowUiVFX : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    private Vector3 initialPos;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Tween moveTween;

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void Start()
    {
        initialPos = this.rectTransform.anchoredPosition;
        startPos = initialPos - new Vector3(0f, 1.4f, 0f);
        targetPos = initialPos + new Vector3(0f, 1.4f, 0f);

        PlayVFX();
    }

    private void OnEnable()
    {
        PlayVFX();
    }

    private void OnDisable()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
            moveTween = null;
        }
    }

    private void PlayVFX()
    {
        this.rectTransform.anchoredPosition = startPos;
        moveTween = this.rectTransform.DOAnchorPos(targetPos, 0.6f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
