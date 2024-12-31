using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
public class PopupSetting : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Button BGMButton;
    [SerializeField] private Sprite BGMPlaySprite;
    [SerializeField] private Sprite BGMMuteSprite;
    
    [Space]
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Button SFXButton;
    [SerializeField] private Sprite SFXPlaySprite;
    [SerializeField] private Sprite SFXMuteSprite;

    [Space]
    [Header("Language")]
    [SerializeField] private Button m_btLanguage;
    [SerializeField] private TextMeshProUGUI btnLanguages;
    private void OnEnable()
    {
        InitSettingsUI();
    }
    private void Start()
    {
        switch (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE))
        {
            case 0:
                btnLanguages.text = "English";
                //eng
                break;
            case 1:
                btnLanguages.text = "Tiếng Việt";
                //vie
                break;

        }
    }
    private void InitSettingsUI() {
        BGMSlider.maxValue = 1f;
        BGMSlider.minValue = 0f;
        SFXSlider.maxValue = 1f;
        SFXSlider.minValue = 0f;

        BGMSlider.value = SoundManager.Instance.BGMSource.volume;
        SFXSlider.value = SoundManager.Instance.SFXSource.volume;

        UpdateBGMButton();
        UpdateSFXButton();
    }

    public void ToggleBGM()
    {
        SoundManager.Instance.ToggleBGMMute();
        UpdateBGMButton();
    }

    public void ToggleSFX()
    {
        SoundManager.Instance.TogggleSFXMute();
        UpdateSFXButton();
    }

    public void UpdateBGMButton()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsConst.MUTEBGM) == 1 || PlayerPrefs.GetFloat(PlayerPrefsConst.VOLUMEBGM) == 0f)
        {
            BGMButton.image.sprite = BGMMuteSprite;
        }
        else
        {
            BGMButton.image.sprite = BGMPlaySprite;
        }
    }
    public void UpdateSFXButton()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsConst.MUTESFX) == 1 || PlayerPrefs.GetFloat(PlayerPrefsConst.VOLUMESFX) == 0f)
        {
            SFXButton.image.sprite = SFXMuteSprite;
        }
        else
        {
            SFXButton.image.sprite = SFXPlaySprite;
        }
    }

    public void OnBGMChangeValue()
    {
        SoundManager.Instance.SetBGMVolume(BGMSlider.value);
        UpdateBGMButton();
    }
    public void OnSFXChangeValue()
    {
        SoundManager.Instance.SetSFXVolume(SFXSlider.value);
        UpdateSFXButton();
    }

    public void OnButtonLanguageClick()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.CHANGELANGUAGE, 1);
            btnLanguages.text = "Tiếng Việt";
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.CHANGELANGUAGE, 0);
            btnLanguages.text = "English";
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
    }
}
