using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanaguagesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Lanaguage");

        if (PlayerPrefs.HasKey(PlayerPrefsConst.CHANGELANGUAGE))
        {
            Debug.Log("Lanaguage1");

            if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
            {
                Debug.Log("Lanaguage2");

                //English
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            } else
            {
                Debug.Log("Lanaguage3");

                //Vietnamese
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            }
        }
        else
        {
            Debug.Log("Lanaguage4");

            PlayerPrefs.SetInt(PlayerPrefsConst.CHANGELANGUAGE, 0);
        }
    }

}
