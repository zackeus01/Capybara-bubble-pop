using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyController : MonoBehaviour
{
    [SerializeField]
    private GameObject survey;
    [SerializeField]
    private GameObject bg;
    void Start()
    {
        if(PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL) >= 11)
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsConst.ISSHOWSURVEY))
            {
                bg.SetActive(true);
                survey.SetActive(true);
                PlayerPrefs.SetInt(PlayerPrefsConst.ISSHOWSURVEY, 1);
            }
            
        }
    }
    public void CloseBtn()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenLink()
    {
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSdBkrHXtxTRZ1pu0fFqOwJ1xQm-9IOQlRAT7U74GE8E4SA96g/viewform?usp=sf_link";
        Application.OpenURL(url);
    }
}
