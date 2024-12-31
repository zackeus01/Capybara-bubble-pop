using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundKey soundKey;
    public AudioClip soundClip;
}

public enum SoundKey
{
    //BGM
    MainMenu,
    Ingame,
    BossMusic1,
    BossMusic2,
    //SFX
    BubblePop,
    BuyItem,
    RewardItem,
    SpinWheel,
    Victory,
    Defeat,
    Click,
    Claw1,
    Claw2,
    SwordHit1,
    SwordHit2,
    Healing
}