using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [Header("VFX Parameter")]
    [SerializeField]
    private Vector3 zoomIn = new Vector3();
    [SerializeField]
    private float zoomInDuration = 1f;
    [SerializeField]
    private Vector3 zoomOut = new Vector3();
    [SerializeField]
    private float zoomOutDuration = 1f;
    [SerializeField]
    private Vector3 transformUp = new Vector3();
    [SerializeField]
    private float upDuration = 1f;
    [SerializeField]
    private float fadeDuration = 1f;

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        scoreText = this.GetComponentInChildren<TMP_Text>();
    }

    public void PlayVFX(ScoreType scoreType)
    {
        //Save origin scale
        //Transform initialParent = this.transform.parent;
        transformUp = this.transform.position + transformUp;

        scoreText.text = ScoreController.Instance.GetScore(scoreType).ToString();

        Vector3 initialPosition = this.transform.position;
        Vector3 initialScale = this.transform.localScale;
        Color initialColor = scoreText.color;

        //Do VFX
        Sequence popUpSequence = DOTween.Sequence();
        popUpSequence.Append(this.transform.DOScale(zoomIn, zoomInDuration))
                     .Append(this.transform.DOScale(zoomOut, zoomOutDuration));
        popUpSequence.Append(this.transform.DOMove(transformUp, upDuration).SetEase(Ease.OutSine))
          .Join(scoreText.DOFade(0f, fadeDuration));

        //On complete
        popUpSequence.OnComplete(() =>
        {
            this.gameObject.SetActive(false);

            // Reset position, scale, and color
            this.transform.position = initialPosition;
            this.transform.localScale = initialScale;
            scoreText.color = initialColor;
            //this.transform.parent = initialParent;
        });
    }
}
