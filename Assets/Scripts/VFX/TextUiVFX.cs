using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextUiVFX : MonoBehaviour
{
    [SerializeField]
    protected Image textImg;

    [SerializeField]
    protected Vector3 initialPos;

    [SerializeField]
    protected Vector3 initialScale;

    [SerializeField]
    protected Color initialColor;

    private void Reset()
    {
        this.LoadComponent();
    }

    protected virtual void LoadComponent()
    {
        textImg = GetComponentInChildren<Image>();
    }

    public virtual void PlayVFX()
    {
        SetUpValue();
        textImg.gameObject.SetActive(true);
        DoTextVFX();
    }

    public virtual void DoTextVFX()
    {

    }

    protected virtual void SetUpValue()
    {
        initialPos = textImg.transform.position;
        initialScale = textImg.transform.localScale;
        initialColor = textImg.color;
    }

    protected virtual void ResetValue()
    {
        textImg.gameObject.SetActive(false);
        textImg.transform.position = initialPos;
        textImg.transform.localScale = initialScale;
        textImg.color = initialColor;
    }
}
