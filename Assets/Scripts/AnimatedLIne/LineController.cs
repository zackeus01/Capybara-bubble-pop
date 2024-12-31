using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] textures;

    [SerializeField]
    private int animationStep = 0;

    [SerializeField]
    private float fps = 0.1f;

    private float fpsCounter = 0f;
    private void Awake()
    {
        Application.targetFrameRate = 90;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= (1f / fps))
        {
            animationStep++;

            if (animationStep >= textures.Length)
            {
                animationStep = 0;
            }
            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);

            fpsCounter = 0f;
        }
    }

    public void ChangeColor()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }
}
