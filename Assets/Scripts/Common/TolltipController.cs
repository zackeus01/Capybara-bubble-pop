using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using TMPro;

public class TolltipController : MonoBehaviour
{
    [SerializeField] GameObject skillItemPrefab;
    [SerializeField] Transform content;
    [SerializeField] List<InformationSkillSo> skillList;

    private StringTable stringTable;

    void Start()
    {
        StartCoroutine(LoadAndPopulateSkills());
    }

    private IEnumerator LoadAndPopulateSkills()
    {
        var tableRequest = LocalizationSettings.StringDatabase.GetTableAsync("MainScene");
        yield return tableRequest;
        stringTable = tableRequest.Result;
        PopulateSkills();
    }

    void PopulateSkills()
    {
        foreach (var skill in skillList)
        {
            GameObject newItem = Instantiate(skillItemPrefab, content);
            Image skillIcon = newItem.transform.GetChild(0).GetComponent<Image>();
            TMP_Text skillDesc = newItem.transform.GetChild(1).GetComponent<TMP_Text>();

            skillIcon.sprite = skill.Icon;

            string localizedText = GetLocalizedText(skill.Skillkey);
            skillDesc.text = localizedText;
        }
    }

    private string GetLocalizedText(string key)
    {
        if (stringTable != null)
        {
            var entry = stringTable.GetEntry(key);
            if (entry != null)
            {
                return entry.LocalizedValue;
            }
        }

        return "Localization Error";
    }

}
