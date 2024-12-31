using System.Linq;
using UnityEngine;

public class SaveStarController : MonoBehaviour
{
    [SerializeField] private StarDataCollection starDataCollection;

    public int FullStarLevelCount
    {
        get
        {
            //starDataCollection.StarDataList.ForEach(
            //    data => Debug.Log($"<color=cyan>{data.LevelId} {data.StarSlider}</color>")
            //    );

            //Debug.Log($"<color=cyan>{starDataCollection.StarDataList.Count(data => data.StarSlider >= 0.99f)}</color>");

            return starDataCollection.StarDataList.Count(data => data.StarSlider >= 0.99f);
        }
    }

    public void SaveStarData(string levelId, float starSliderValue)
    {

        LoadData();
        //Debug.Log("Value received in SaveStarData: " + starSliderValue);
        StarData existingData = starDataCollection.StarDataList.Find(data => data.LevelId == levelId);

        if (existingData != null)
        {
            //Debug.Log("Value received in SaveStarData: " + starSliderValue);
            if (starSliderValue > existingData.StarSlider)
            {
                existingData.StarSlider = starSliderValue;
            }
        }
        else
        {
            starDataCollection.StarDataList.Add(new StarData(levelId, starSliderValue));
        }
        SaveData();
    }

    public void LoadStarDataForLevel(string levelId, StarVFX[] stars)
    {
        LoadData();
        StarData data = starDataCollection.StarDataList.Find(d => d.LevelId == levelId);
        if (data != null)
        {
            if (data.StarSlider >= 0.22f)
            {
                stars[0].GetStarNoVFX();
            }
            if (data.StarSlider >= 0.62f)
            {
                stars[1].GetStarNoVFX();
            }
            if (data.StarSlider >= 0.99f)
            {
                stars[2].GetStarNoVFX();
            }
        }
    }

    public StarData LoadStarDataForLevel(string levelId)
    {
        LoadData();
        return starDataCollection.StarDataList.Find(data => data.LevelId.Equals(levelId));
    }

    public void ClaimReward(string levelId)
    {
        LoadData();
        starDataCollection.StarDataList.Find(data => data.LevelId.Equals(levelId)).ClaimReward();
        SaveData();
    }

    #region Save & Load
    private void LoadData()
    {
        SaveSystem.LoadData(GameConst.STAR_FILE, ref starDataCollection);
    }

    private void SaveData()
    {
        SaveSystem.SaveData(GameConst.STAR_FILE, starDataCollection);
    }

    #endregion
}
