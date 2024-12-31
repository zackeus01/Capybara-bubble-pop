using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTextEvent : MonoBehaviour
{
    [SerializeField] private LanguageKey m_lk;
    [SerializeField] private TMP_Text m_tx;
    private void OnEnable()
    {
        Observer.Instance.AddObserver(ObserverString.CHANGELANGUAGE_ACTION, ChangeText);
        m_tx.text = ReadCSVFile.Instance.ChangeLanguage(m_lk);
    }
    private void OnDisable()
    {
        Observer.Instance?.RemoveObserver(ObserverString.CHANGELANGUAGE_ACTION, ChangeText);
    }
    private void ChangeText(object data)
    {
        m_tx.text = ReadCSVFile.Instance.ChangeLanguage(m_lk);
    }
}
