using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("SceneController");

    }
    public IEnumerator ChangeSceneTransition(string scene, Image image)
    {
        // Bắt đầu tải scene không đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;  // Ngăn scene tự động kích hoạt
        Debug.Log("SceneController1");

        // Vòng lặp kiểm tra tiến trình tải scene
        while (asyncLoad.progress < 0.9f)  // asyncLoad.progress sẽ đạt tối đa 0.9 trước khi allowSceneActivation
        {
            yield return null;  // Chờ đến frame tiếp theo
        }

        // Khi tải đạt gần hoàn tất (0.9), bắt đầu fade hình ảnh
        image.DOFade(1f, 0.5f).OnComplete(() =>
        {
            // Kích hoạt scene khi hình ảnh đã fade xong
            asyncLoad.allowSceneActivation = true;
        });

        // Đợi cho đến khi scene được kích hoạt
        while (!asyncLoad.isDone)
        {
            Debug.Log("SceneController2");

            yield return null;
        }
    }
}
