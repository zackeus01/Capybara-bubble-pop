using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BallTypePrefab
{
    [SerializeField] private string _id;
    [SerializeField] private Ball _prefab;

    public string Id
    {
        get { return _id; }
    }

    public Ball Prefab
    {
        get { return _prefab; }
    }
}
