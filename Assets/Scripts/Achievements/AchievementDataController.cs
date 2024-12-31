using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementDataController : Singleton<AchievementDataController>
{
    [SerializeField] private AchievementCollection achievementCollection;
    [SerializeField] private List<AchievementSO> achievementSos;
    private readonly List<Achievement> achievementDatas = new List<Achievement>();

    public List<Achievement> AchievementData => achievementDatas;
    private void OnEnable()
    {
        InitEvents();
        InitController();
    }

    private void OnDestroy()
    {
        SaveData();
        RemoveEvents();
    }

    public Achievement GetAchievement(int id)
    {
        return achievementDatas.Find(a => a.Id.Equals(id));
    }

    private void InitController()
    {
        achievementSos.ForEach(so => achievementDatas.Add(new Achievement(so)));

        LoadData();

        achievementCollection ??= new AchievementCollection();
        achievementDatas.ForEach(data => achievementCollection.AddData(data.Id));
        achievementCollection.Data.ForEach(data => achievementDatas.Find(d => d.Id.Equals(data.Id)).GetData(data));

        SaveData();
    }

    private void ClaimReward(int id)
    {
        achievementDatas.Find(a => a.Id.Equals(id)).ClaimAchievementReward();
        UpdateSaveData(id);
    }

    private void UpdateData(AchievementType type, int amount)
    {
        achievementDatas
            .Where(a => a.Type.Equals(type))
            .ToList()
            .ForEach(a => UpdateData(a.Id, amount));
    }

    private void ChangeData(AchievementType type, int amount)
    {
        achievementDatas
            .Where(a => a.Type.Equals(type))
            .ToList()
            .ForEach(a => ChangeData(a.Id, amount));
    }

    private void UpdateData(int id, int amount)
    {
        achievementDatas.Find(a => a.Id.Equals(id))?.AddProgress(amount);
        UpdateSaveData(id);
    }

    private void ChangeData(int id, int amountToChange)
    {
        achievementDatas.Find(a => a.Id.Equals(id))?.ChangeData(amountToChange);
        UpdateSaveData(id);
    }

    private void UpdateSaveData(int id)
    {
        achievementCollection.UpdateData(achievementDatas.Find(a => a.Id.Equals(id)));
        SaveData();
    }

    #region Events
    public void InitEvents()
    {
        AchievementEvent.OnUpdateAchievement.AddListener(UpdateData);
        AchievementEvent.OnClaimAchievementReward.AddListener(ClaimReward);
        AchievementEvent.OnChangeAchievementData.AddListener(ChangeData);
    }

    public void RemoveEvents()
    {
        AchievementEvent.OnUpdateAchievement.RemoveListener(UpdateData);
        AchievementEvent.OnClaimAchievementReward.RemoveListener(ClaimReward);
        AchievementEvent.OnChangeAchievementData.RemoveListener(ChangeData);
    }

    #endregion

    #region Save & Load
    private void LoadData()
    {
        SaveSystem.LoadData(GameConst.ACHIEVEMENT_FILE, ref achievementCollection);
    }

    private void SaveData()
    {
        SaveSystem.SaveData(GameConst.ACHIEVEMENT_FILE, achievementCollection);
    }

    #endregion
}
