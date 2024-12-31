using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [Header("EFX Parameter")]
    [SerializeField]
    private float loadDuration = 10f; 

    [Header("Component")]
    [SerializeField]
    private Slider slider; 
    [SerializeField]
    private Image img;    

    private void Start()
    {
        StartCoroutine(LoadScene());
        Debug.Log("LoadingScene");
        PlayerPrefs.SetInt(PlayerPrefsConst.CHANGELANGUAGE, 0);
    }

    private IEnumerator LoadScene()
    {
        slider.value = 0; 
        float elapsedTime = 0f;
        Debug.Log("LoadingScene1");


        while (elapsedTime < loadDuration)
        {
            Debug.Log("LoadingScene2");

            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(0, slider.maxValue, elapsedTime / loadDuration);
            yield return null;
        }

        slider.value = slider.maxValue;
        Debug.Log("LoadingScene3");

        yield return StartCoroutine(SceneController.Instance.ChangeSceneTransition("MenuScene", img));
        Debug.Log("LoadingScene4");

    }
}
