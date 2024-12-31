using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> actionTxt;

    private void OnEnable()
    {
        InitEvent();
    }

    private void OnDisable()
    {
        RemoveEvent();
    }

    private void Start()
    {
        UpdateHelper();
    }

    private void UpdateHelper()
    {
        actionTxt[0].text = HelperController.Instance.GetHelperCount(BallType.Bomb).ToString();
        actionTxt[1].text = HelperController.Instance.GetHelperCount(BallType.Ziczac).ToString();
        actionTxt[2].text = HelperController.Instance.GetHelperCount(BallType.Rainbow).ToString();
        actionTxt[3].text = HelperController.Instance.GetHelperCount(BallType.Firework).ToString();
    }

    private void InitEvent()
    {
        HelperEvent.OnHelperBallShooted.AddListener(UpdateHelper);
    }

    private void RemoveEvent()
    {
        HelperEvent.OnHelperBallShooted.RemoveListener(UpdateHelper);
    }
}
