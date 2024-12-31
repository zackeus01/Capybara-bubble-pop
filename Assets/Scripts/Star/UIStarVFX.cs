using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStarVFX : MonoBehaviour
{
    [SerializeField]
    private Transform star;
    [SerializeField]
    private Image starImg;

    private bool isPlayed = false;

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        star = this.transform.GetChild(1);
        starImg = star.GetComponent<Image>();
    }

    public void PlayVFX()
    {
        if (isPlayed) return;

        Vector3 initialPos = star.localPosition;
        Vector3 initialRotation = star.localRotation.eulerAngles;

        star.localScale = new Vector3(3f, 3f, 1f);
        star.localPosition += new Vector3(0f, 30f, 0f);
        
        Vector3 targetRotation = initialRotation + new Vector3(0, 0, 180f);

        if (targetRotation.z < 0)
        {
            targetRotation = new Vector3(targetRotation.x, targetRotation.y, 360 + targetRotation.z);
        }

        star.rotation = Quaternion.Euler(targetRotation);
        // Activate the star object
        star.gameObject.SetActive(true);

        star.DOLocalMove(initialPos, 0.7f).SetEase(Ease.InQuad);

        star.DOScale(Vector3.one, 0.6f).SetEase(Ease.InOutExpo);

        starImg.DOFade(1f, 0.6f);
        
        star.DORotate(initialRotation, 0.5f, RotateMode.Fast).SetEase(Ease.Linear);

        isPlayed = true;
    }
}
