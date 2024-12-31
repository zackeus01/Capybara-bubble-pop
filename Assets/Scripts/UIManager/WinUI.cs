using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WinUI : MonoBehaviour
{
   public void NextLevel()
    {
        int currentLevel  = PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 0);
        currentLevel++;
        PlayerPrefs.SetInt(PlayerPrefsConst.CURRENT_LEVEL, currentLevel);
        SceneManager.LoadSceneAsync("GamePlay");
    }
}
