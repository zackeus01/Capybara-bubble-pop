using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UIWinScreen : MonoBehaviour
{
    [SerializeField] private bool _hideOnAwake;

    private void Awake()
    {
        if (_hideOnAwake)
            gameObject.SetActive(false);

        //GameplayEvent.OnGameWin.AddListener(ShowWinScreen);
    }

    private void OnDestroy()
    {
        //GameplayEvent.OnGameWin.RemoveListener(ShowWinScreen);
    }

    public void ShowWinScreen()
    {
        gameObject.SetActive(true);
        UIEvent.OnClickPause.Invoke();
    }
}
