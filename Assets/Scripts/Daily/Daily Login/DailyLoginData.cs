using System;
using UnityEngine;

[Serializable]
public class DailyLoginData
{
    [SerializeField] private int id;
    [SerializeField] private bool isClaimed;
    [SerializeField] private bool isUnlocked;

    public int Id { get { return id; } }
    public bool IsClaimed { get { return isClaimed; } }
    public bool IsUnlocked { get { return isUnlocked; } }

    public DailyLoginData(int id)
    {
        this.id = id;
        isClaimed = false;
        isUnlocked = false;
    }

    public void UnlockDailyLogin()
    {
        isUnlocked = true;
    }

    public void ClaimDailyLogin()
    {
        isClaimed = true;
    }

    public void ResetDailyLogin()
    {
        isClaimed = false;
        isUnlocked = false;
    }
}
