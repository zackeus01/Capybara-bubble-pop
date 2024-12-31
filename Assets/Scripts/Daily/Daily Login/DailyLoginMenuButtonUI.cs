using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyLoginMenuButtonUI : MonoBehaviour
{
    [SerializeField] private Image notification;
    [SerializeField] private MenuManager menuManager;

    private void OnEnable()
    {
        InitEvent();
        InitButton();
    }

    private void OnDisable()
    {
        RemoveEvent();
    }

    private void InitEvent()
    {
        UIEvent.OnNewDay.AddListener(StartNotification);
        UIEvent.OnClaimDailyReward.AddListener(StopNotification);
    }

    private void RemoveEvent()
    {
        UIEvent.OnNewDay.RemoveListener(StartNotification);
        UIEvent.OnClaimDailyReward.RemoveListener(StopNotification);
    }

    private void InitButton()
    {
        bool hasNoti = DailyLoginDataController.Instance.DailyLoginDataCollection.DailyLoginDataList.Exists(dld => !dld.IsClaimed && dld.IsUnlocked);

        //Debug.Log($"{hasNoti}");

        if (hasNoti) StartNotification();
    }

    private void StartNotification()
    {
        notification.gameObject.SetActive(true);
        StartCoroutine(ActivateNotificationAnimation());
    }

    private void StopNotification(int day)
    {
        notification.gameObject.SetActive(false);
        StopCoroutine(ActivateNotificationAnimation());
    }

    private IEnumerator ActivateNotificationAnimation()
    {
        while (true)
        {
            notification.gameObject.transform
                .DOScale(new Vector3(0.5f, 0.5f), 0.5f)
                .OnComplete(() =>
                    {
                        notification.gameObject.transform.DOScale(Vector3.one, 0.5f);
                    });

            yield return new WaitForSeconds(1f);
        }
    }

}
