using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    private void Awake()
    {
        GameplayEvent.OnGameWin.AddListener(SaveLevelState);
    }

    private void OnDestroy()
    {
        GameplayEvent.OnGameWin.RemoveListener(SaveLevelState);
    }

    private void SaveLevelState()
    {
        string levelId = LevelDataHolder.LevelData.LevelId;

        if (PlayerPrefs.GetInt(levelId) > 0)
            return;

        PlayerPrefs.SetInt(levelId, 1);
        PlayerPrefs.Save();
    }
}
