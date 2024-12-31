using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    [Header("FPS Buffer")]
    [SerializeField] private int _frameRange = 60;

    private int[] _fpsBuffer;
    private int _fpsBufferIndex;

    public int AverageFPS
    {
        get;

        private set;
    }

    private void Awake()
    {
        if (_fpsBuffer == null || _frameRange <= 0)
            IninitializeBuffer();
    }

    private void Update()
    {
        UpdateBuffer();
        CalculateFPS();
    }

    private void IninitializeBuffer()
    {
        if (_frameRange <= 0)
            _frameRange = 1;


        _fpsBufferIndex = 0;
        _fpsBuffer = new int[_frameRange];
    }

    private void UpdateBuffer()
    {
        _fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);

        if (_fpsBufferIndex >= _frameRange)
            _fpsBufferIndex = 0;
    }

    private void CalculateFPS()
    {
        int sum = 0;

        for (int i = 0; i < _frameRange; i++)
        {
            sum += _fpsBuffer[i];
        }

        AverageFPS = sum / _frameRange;
    }
}
