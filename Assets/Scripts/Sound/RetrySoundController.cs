using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrySoundController : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayOneShotSFX(SoundKey.Defeat);       
    }
}
