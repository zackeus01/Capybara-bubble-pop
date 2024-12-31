using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTurn;
    [SerializeField] private TextMeshProUGUI txtStack;

    private RectTransform rectTransform;

    private float expandScale = 1.2f; // Scale when expanded
    private float normalScale = 1f;   // Normal scale
    private float expandDuration = 0.3f;
    private float shrinkDuration = 0.3f;

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    // Entry animation: Expand and then shrink to normal size
    public void PlayEntryAnimation()
    {
        gameObject.SetActive(true);

        rectTransform.localScale = Vector3.zero; // Start scale at zero

        // Create sequence for entry animation
        Sequence entrySequence = DOTween.Sequence();
        entrySequence.Append(rectTransform.DOScale(expandScale, expandDuration).SetEase(Ease.OutBack))
                     .Append(rectTransform.DOScale(normalScale, shrinkDuration).SetEase(Ease.InBack));
    }

    // Exit animation: Expand a bit, then shrink to zero
    public void PlayExitAnimation()
    {
        // Create sequence for exit animation
        Sequence exitSequence = DOTween.Sequence();
        exitSequence.Append(rectTransform.DOScale(expandScale, expandDuration).SetEase(Ease.OutBack))
                    .Append(rectTransform.DOScale(0, shrinkDuration).SetEase(Ease.InBack))
                    .OnComplete(() => gameObject.SetActive(false)); // Disable GameObject after shrinking
    }

    public void UpdateUI(int turns, int stacks)
    {
        txtTurn.text = turns.ToString();
        txtStack.text = stacks.ToString();
        if (turns > 0  && stacks > 0)
        {     
            PlayEntryAnimation();
        }
        else
        {
            PlayExitAnimation();
        }
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
