using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StarDataCollection
{
    [SerializeField] private List<StarData> starDataList = new List<StarData>();

    public List<StarData> StarDataList { get { return starDataList; } set { starDataList = value; } }

}
[Serializable]
public class StarData
{
    [SerializeField] private string levelId;
    [SerializeField] private float starSlider;
    [SerializeField] private bool isRewardCollected;

    public StarData(string levelId, float starSlider)
    {
        this.levelId = levelId;
        this.starSlider = starSlider;
        this.isRewardCollected = false;
    }

    public void ClaimReward()
    {
        isRewardCollected = true;
    }

    public string LevelId { get { return levelId; } set { levelId = value; } }
    public float StarSlider { get { return starSlider; } set { starSlider = value; } }
    public bool IsRewardCollected { get { return isRewardCollected; } set { isRewardCollected = value; } }
}
