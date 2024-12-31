using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScripts : MonoBehaviour
{
    public Image img;
    void Start()
    {
        img.gameObject.SetActive(true);
        img = gameObject.GetComponent<Image>();
        img.DOFade(0f, 0.5f).OnComplete(() => { 
            this.gameObject.SetActive(false);
        });
    }
}
