using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupReward : MonoBehaviour
{
    [SerializeField] private Button m_btDaily;
    [SerializeField] private Button m_btMission;
    [SerializeField] private GameObject m_obDailyContent;
    [SerializeField] private GameObject m_obMissionContent;

    void Start()
    {
        if (m_btDaily != null)
            m_btDaily.onClick.AddListener(OnButtonDailyClick);
        if (m_btMission != null)
            m_btMission.onClick.AddListener(OnButtonMissionClick);
        if (m_obDailyContent != null)
            m_obDailyContent.SetActive(true);
        if (m_obMissionContent != null)
            m_obMissionContent.SetActive(false);
    }

    private void OnButtonDailyClick()
    {
        if (m_obDailyContent != null)
            m_obDailyContent.SetActive(true);
        if (m_obMissionContent != null)
            m_obMissionContent.SetActive(false);
    }
    private void OnButtonMissionClick()
    {
        if (m_obDailyContent != null)
            m_obDailyContent.SetActive(false);
        if (m_obMissionContent != null)
            m_obMissionContent.SetActive(true);
    }
}
