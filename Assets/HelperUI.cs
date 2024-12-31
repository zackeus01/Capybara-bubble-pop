using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] helpers;
    private void Start()
    {
        GameplayEvent.OnMoveGameObjectDone.AddListener(ActivateHelper);
    }

   
    private void ActivateHelper()
    {
        foreach (var helper in helpers)
        {
             helper.SetActive(true);
               

        }
    }
}
