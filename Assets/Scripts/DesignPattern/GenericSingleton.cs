using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object _lock = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {

                        GameObject go = new GameObject();
                        instance = go.AddComponent<T>();
                        go.name = "Singleton";
                        DontDestroyOnLoad(go);
                    }
                }
                else if (instance != FindObjectOfType<T>())
                {
                    Destroy(FindObjectOfType<T>());
                }
                return instance;
            }
        }
    }
    private static bool applicationIsQuitting = false;
    public void OnDestroy()
    {
        Debug.Log("Gets destroyed");
        applicationIsQuitting = true;
    }
}