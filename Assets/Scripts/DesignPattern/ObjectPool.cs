using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Preallocation
{
    public GameObject gameObject;
    public int count;
    public bool expandable;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance => s_Instance;
    static ObjectPool s_Instance;
    public List<Preallocation> preAllocations;

    public List<GameObject> pooledGobjects;

    protected void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;


        pooledGobjects = new List<GameObject>();

        foreach (Preallocation item in preAllocations)
        {
            for (int i = 0; i < item.count; ++i)
                pooledGobjects.Add(CreateObject(item.gameObject));
        }
    }

    public GameObject GetObject(string name)
    {
        for (int i = 0; i < pooledGobjects.Count; ++i)
        {
            if (!pooledGobjects[i].activeSelf && pooledGobjects[i].name.Equals(name + "(Clone)"))
            {
                pooledGobjects[i].SetActive(true);
                return pooledGobjects[i];
            }
        }

        for (int i = 0; i < preAllocations.Count; ++i)
        {
            if (preAllocations[i].gameObject.name.Equals(name))
                if (preAllocations[i].expandable)
                {
                    GameObject obj = CreateObject(preAllocations[i].gameObject);
                    pooledGobjects.Add(obj);
                    obj.SetActive(true);
                    return obj;
                }
        }
        return null;
    }

    GameObject CreateObject(GameObject item)
    {
        GameObject obj = Instantiate(item, transform);
        obj.SetActive(false);
        return obj;
    }

    public void DeleteActiveObject(string name)
    {
        for (int i = 0; i < pooledGobjects.Count; ++i)
        {
            if (pooledGobjects[i].activeSelf && pooledGobjects[i].name.Equals(name + "(Clone)"))
            {
                pooledGobjects[i].SetActive(false);
            }
        }
    }
}