using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSVFile : GenericSingleton<ReadCSVFile>
{
    Dictionary<LanguageKey, LanguageStruct> dic = new Dictionary<LanguageKey, LanguageStruct>();
    private void Awake()
    {
        ReadCSVLanguage();
    }
    public void ReadCSVLanguage()
    {
        //parse CSV file
        TextAsset textCSV = Resources.Load<TextAsset>("CSVFILE/Language");
        string[] stringData = textCSV.text.Split("\n");
        foreach (var item in stringData)
        {
            string[] stringKeyAndData = item.Split(",");
            LanguageKey languageKey = ParseEnum<LanguageKey>(stringKeyAndData[0]);
            LanguageStruct languageStruct = new LanguageStruct();
            languageStruct.txVN = stringKeyAndData[1];
            languageStruct.txEN = stringKeyAndData[2];
            dic.Add(languageKey, languageStruct);
        }
    }
    public string ChangeLanguage(LanguageKey languageKey)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsConst.CHANGELANGUAGE) == 0)
            return dic[languageKey].txVN;
        else
            return dic[languageKey].txEN;
    }
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
public enum LanguageKey
{
    Key, Test1, Test2
}
public struct LanguageStruct
{
    public string txVN;
    public string txEN;
}