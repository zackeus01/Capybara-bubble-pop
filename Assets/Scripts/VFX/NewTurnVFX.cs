using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(TMP_Text))]
public class NewTurnVFX : MonoBehaviour
{
    #region Component
    [Header("Component")]
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private TMP_Text newTurn;
    #endregion

    [SerializeField]
    private Vector3 initialPos;
    [SerializeField]
    private Color initialColor;
    [SerializeField]
    private Vector3 endPos;


    private void Reset()
    {
        this.LoadComponent();
        this.SetUpData();
    }

    private void LoadComponent()
    {
        rectTransform = this.GetComponent<RectTransform>();
        newTurn = this.GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        BossEvent.OnBossActionEnd.AddListener(PlayVFX);
    }

    public void PlayVFX()
    {
        ResetData();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutSine));
        sequence.Append(rectTransform.DOAnchorPos(endPos, 0.2f).SetDelay(0.3f))
            .Join(newTurn.DOFade(0f, 0.1f).SetEase(Ease.InCubic));
        sequence.OnComplete(() =>
        {
            ResetData();
        });
    }

    private void ResetData()
    {
        this.rectTransform.anchoredPosition = initialPos;
        newTurn.color = initialColor;
    }

    private void SetUpData()
    {
        initialPos = rectTransform.anchoredPosition;
        endPos = new Vector3(-initialPos.x, initialPos.y, initialPos.z);
        initialColor = newTurn.color;
    }
}
