using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _dontDestroyOnLoad;
    private static bool _createNewOnMissing = true;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_createNewOnMissing)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        if (_dontDestroyOnLoad)
                        {
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("No singleton");
                }
            }
            return _instance;
        }
    }

    protected Singleton()
    {
        _dontDestroyOnLoad = true;
        _createNewOnMissing = true;
    }

    protected Singleton(bool dontDestroyOnLoad = true)
    {
        _dontDestroyOnLoad = dontDestroyOnLoad;
    }

    protected Singleton(bool dontDestroyOnLoad = true, bool createNewOnMissing = true)
    {
        _dontDestroyOnLoad = dontDestroyOnLoad;
        _createNewOnMissing = createNewOnMissing;
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDisable()
    {
        Destroy(gameObject);
    }
}
