using UnityEngine;

public class BossBGM : MonoBehaviour
{
    private void Start()
    {
        PlayBGM();
    }

    private void PlayBGM()
    {
        int i = Random.Range(0, 101);
        if (i >= 50)
        {
            SoundManager.Instance.PlayBGM(SoundKey.BossMusic1);
        }
        else
        {
            SoundManager.Instance.PlayBGM(SoundKey.BossMusic2);
        }
    }
}
