using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneSoundManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundKey.MainMenu);   
    }
}
