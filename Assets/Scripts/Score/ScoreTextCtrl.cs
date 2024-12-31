using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextCrtl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    private void Awake()
    {
        GameplayEvent.OnTotalScoreChanged.AddListener(SetText);
    }

    private void OnDisable()
    {
        GameplayEvent.OnTotalScoreChanged.RemoveListener(SetText);
    }
    private void SetText(int score)
    {
        scoreText.text = score.ToString();
    }
}
