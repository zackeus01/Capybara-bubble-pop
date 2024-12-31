using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUIController : MonoBehaviour
{
    [SerializeField] private Ease moveType;
    [SerializeField] private Vector3 size;
    [SerializeField] private float durationOn;
    [SerializeField] private float durationOff;
    [SerializeField] private GameObject offGameObject;
    
    private void OnEnable()
    {
        this.transform.DOScale(size, durationOn).SetEase(moveType).OnComplete(() =>
        {
            this.transform.DOScale(new Vector3(1f,1f,1f), 0.2f).SetEase(moveType);
        });
    }
    private void Reset()
    {
        moveType = Ease.OutSine;
        size = new Vector3(1.2f,1.2f,1f);
        durationOn = 0.2f;
        durationOff = 0.2f;
        offGameObject = null;
    }
    public void OnTurnOff()
    {
        Vector3 vector3 = this.transform.localScale;
        this.transform.DOScale(new Vector3(0f, 0f, 1f), durationOff).SetEase(moveType).OnComplete(() =>
        {
            offGameObject.SetActive(false);
            this.transform.localScale = vector3;
        });
    }
}
