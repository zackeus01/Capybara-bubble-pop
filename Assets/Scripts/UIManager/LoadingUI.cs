using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Slider slider;
    private float duration = 5f;
    private float elapsedTime = 0f;
    private void Start()
    {
        slider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
        if (slider != null )
        {
            slider.value = 0f;
            Debug.Log("Find slider");
        }
    }
    private void Update()
    {
        if (slider != null && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsedTime / duration);
            if(elapsedTime >= duration)
            {
                Debug.Log("Load xong");
            }
        }
    }
}
