using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string KEY = "BB";
    private static string dataPath;

    // Loading data from file
    public static void LoadData<T>(string fileName, ref T refData) where T : new()
    {
        dataPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(dataPath))
        {
            Debug.Log($"{fileName} does not exist!");
            ResetData<T>(ref refData);
            SaveData(fileName, refData);
            return;
        }

        try
        {
            string data = File.ReadAllText(dataPath);
            string decrypted = XOROperator(data, KEY);

            Debug.Log($"LoadData {fileName}: \n" + decrypted);

            refData = JsonUtility.FromJson<T>(decrypted);

            Debug.Log(refData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
            ResetData<T>(ref refData);
            SaveData<T>(dataPath, refData);
        }
    }

    // Saving data to file
    public static void SaveData<T>(string fileName, T data)
    {
        dataPath = Path.Combine(Application.persistentDataPath, fileName);
        string origin = JsonUtility.ToJson(data);

        Debug.Log($"Save Data {fileName}: \n" + origin);

        string encrypted = XOROperator(origin, KEY);
        File.WriteAllText(dataPath, encrypted);
        Debug.Log("Data saved to: " + dataPath);
    }

    // Reset data method
    public static void ResetData<T>(ref T refData) where T : new()
    {
        refData = new T(); // Resets data by creating a new instance of T
    }

    #region Cryptography
    public static string XOROperator(string input, string key)
    {
        if (key.Length == 0) return input;

        char[] output = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (char)(input[i] ^ key[i % key.Length]);
        }

        return new string(output);
    }
    #endregion
}

