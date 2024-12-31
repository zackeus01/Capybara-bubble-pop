using UnityEngine;

public class GameplayBGM : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundKey.Ingame);   
    }
}
