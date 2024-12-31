using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TwinklingLight : MonoBehaviour
{
    [SerializeField]
    private Light2D pointLight;

    [SerializeField]
    private float speed = 1f;

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        pointLight = this.GetComponent<Light2D>();
    }

    private void Update()
    {
        float midpoint = (0.5f + 0.7f) / 2.0f;
        float range = (0.7f - 0.5f) / 2.0f;
        pointLight.falloffIntensity = midpoint + Mathf.Sin(Time.time * speed) * range;
    }
}
