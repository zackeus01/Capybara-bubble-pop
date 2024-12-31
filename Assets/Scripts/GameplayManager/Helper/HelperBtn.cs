using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperBtn : MonoBehaviour
{
    [SerializeField]
    private BallType type;
    [SerializeField]
    private GameObject helper;
    [SerializeField]
    private GameObject cancelBtn;

    private bool canEnable = true;
    public BallType Type => type;

    private void Awake()
    {
        HelperEvent.OnHelperDeactivated.AddListener(EnableHelper);
        HelperEvent.OnHelperBallShooted.AddListener(DisableAllHelper);
        GameplayEvent.OnActiveBallShooted.AddListener(DeactivateHelper);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(ActiveHelper);
    }

    public void OnClickBtn()
    {
        CheckHelperLeft();
        if (!canEnable) return;
        HelperEvent.OnHelperActivated.Invoke(this);
    }

    private void CheckHelperLeft()
    {
        int count = HelperController.Instance.GetHelperCount(type);
        if (count > 0)
        {
            canEnable = true;
        }
        else
        {
            canEnable = false;
        }
    }

    public void HelperEnd()
    {
        HelperEvent.OnHelperDeactivated.Invoke();
    }

    private void DisableAllHelper()
    {
        helper.SetActive(false);
        cancelBtn.SetActive(false);
    }

    private void EnableHelper()
    {
        helper.SetActive(true);
        cancelBtn.SetActive(false);
    }

    private void DeactivateHelper(Ball ball)
    {
        canEnable = false;
    }

    private void ActiveHelper()
    {
        canEnable = true;
    }
    public void popHellper()
    {
        int count = HelperController.Instance.GetHelperCount(type);
        if (count > 0)
        {
            helper.SetActive(false);
            cancelBtn.SetActive(true);
        }
        else
        {
            cancelBtn.SetActive(false);
            helper.SetActive(true);
        }
    }
}
