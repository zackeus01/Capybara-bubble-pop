using UnityEngine;
using UnityEngine.Pool;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private AchievementButtonUI buttonPrefab;
    [SerializeField] private Transform spawnPoint;

    private ObjectPool<AchievementButtonUI> pool;

    private void OnEnable()
    {
        SetupPool();
        UIEvent.OnChangeMenuTab.RemoveListener(OnChangeMenuTab);
        UIEvent.OnChangeMenuTab.AddListener(OnChangeMenuTab);
        SetupUI();
    }

    private void OnDisable()
    {
        ClearPool();
        UIEvent.OnChangeMenuTab.RemoveListener(OnChangeMenuTab);
    }

    #region ObjectPool
    private void SetupPool()
    {
        ClearPool();

        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<AchievementButtonUI>(
            createFunc: () => Instantiate(buttonPrefab, spawnPoint),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 6
        );

    }
    public void ClearPool()
    {


        // Loop through all active objects in the pool and release them
        for (int i = spawnPoint.childCount - 1; i >= 0; i--)
        {
            var btn = spawnPoint.GetChild(i).GetComponent<AchievementButtonUI>();
            if (btn != null && btn.isActiveAndEnabled)
            {
                pool.Release(btn);
            }
        }

    }
    #endregion

    private void SetupUI()
    {
        ClearPool();

        //Debug.Log(AchievementDataController.Instance.AchievementData.Count);

        AchievementDataController.Instance.AchievementData.ForEach(data => pool.Get().SetupButton(data.Id));
    }

    private void OnChangeMenuTab(int id)
    {
        //Debug.Log(id, this);
        if (id == -1) SetupUI();
    }
}
