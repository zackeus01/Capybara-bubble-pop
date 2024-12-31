using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private Vector2 _referenceResolution = new Vector2(1080, 1920);
    [Range(0f, 1f)]
    [SerializeField] private float _match = 0f;
    private float _initialSize;
    private float _targetAspect;

    public Vector2 WorldViewportMax
    {
        get
        {
            return _camera.ViewportToWorldPoint(_camera.rect.max);
        }
    }
    public Vector2 WorldViewportMin
    {
        get
        {
            return _camera.ViewportToWorldPoint(_camera.rect.min);
        }
    }


    #region Cache Components
    private Camera _camera;
    public Camera Camera
    {
        get
        {
            if (_camera == null)
                _camera = GetComponent<Camera>();
            return _camera;
        }
    }
    #endregion


    private void Awake()
    {
        if (Camera == null)
        {
            Debug.LogError("Camera is not found!");
            return;
        }

        _initialSize = Camera.orthographicSize;
        _targetAspect = _referenceResolution.x / _referenceResolution.y;
    }

    private void Update()
    {
        if (Camera.orthographic)
        {
            float constantWidthSize = _initialSize * (_targetAspect / Camera.aspect);
            Camera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _match);
        }
    }
}
