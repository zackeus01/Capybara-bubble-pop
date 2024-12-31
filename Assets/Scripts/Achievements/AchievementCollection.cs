using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementCollection
{
    [SerializeField] private List<AchievementDTO> data;
    public List<AchievementDTO> Data { get { return data; } private set { data = value; } }

    public AchievementCollection()
    {
        data = new List<AchievementDTO>();
    }

    public void AddData(int id)
    {
        if (data.Exists(a => a.Id.Equals(id))) return;
        data.Add(new AchievementDTO(id));
    }

    public void UpdateData(Achievement aso)
    {
        if (!data.Exists(dto => dto.Id.Equals(aso.Id))) return;
        data.Find(dto => dto.Id.Equals(aso.Id)).GetData(aso);
    }
}
