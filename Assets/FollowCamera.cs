using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Vector3 initialPos;

    private void Reset()
    {
        cam = Camera.main.transform;
        initialPos = - cam.position + this.transform.position;
    }

    private void Update()
    {
        this.transform.position = initialPos + cam.position;
    }
}
