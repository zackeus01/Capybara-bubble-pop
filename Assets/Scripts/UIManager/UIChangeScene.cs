using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIChangeScene : MonoBehaviour
{
    [SerializeField] public string _changingScene = GameScenes.MenuScene;
    [SerializeField] public Image _image;

    #region Cache Component
    private Button _changeSceneButton;
    public Button ChangeSceneButton
    {
        get
        {
            if (_changeSceneButton == null)
                _changeSceneButton = GetComponent<Button>();
            return _changeSceneButton;
        }
    }
    #endregion

    private void Awake()
    {
        EventSubscribe();
    }

    private void OnDestroy()
    {
        EventUnsubscribe();
    }

    public void LoadChangingScene()
    {
        StartCoroutine(SceneController.Instance.ChangeSceneTransition(_changingScene, _image));
        //StartCoroutine(LoadChangingSceneCoroutine());
    }

    private IEnumerator LoadChangingSceneCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(_changingScene, LoadSceneMode.Single);
        yield break;
    }

    protected virtual void EventSubscribe()
    {
        ChangeSceneButton.onClick.AddListener(LoadChangingScene);
    }

    protected virtual void EventUnsubscribe()
    {
        ChangeSceneButton.onClick.RemoveListener(LoadChangingScene);
    }
}
