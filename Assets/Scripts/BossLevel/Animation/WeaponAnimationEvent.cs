using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class WeaponAnimationEvent : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private Animator bowAnimator;
    [SerializeField] private Animator magicSealAnimator;

    [Header("Arrow")]
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform arrowContainer;
    [SerializeField] private Transform arrowTarget;
    [SerializeField] private ArrowSO arrowSO;

    private ElementType element;

    private ObjectPool<Arrow> pool;

    private readonly string swordSlash = "slash";
    private readonly string wiggle = "wiggle";
    private readonly string raise = "raise";

    private static readonly UnityEvent<Transform> onArrowShoot = new UnityEvent<Transform>();
    public static UnityEvent<Transform> OnArrowShoot { get { return onArrowShoot; } }
    
    #region ObjectPool
    private void SetupPool()
    {
        ClearPool();

        // Create an object pool with specific pool size and default actions
        pool = new ObjectPool<Arrow>(
            createFunc: () => Instantiate(arrowPrefab, arrowContainer),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 5
        );

    }

    public void ClearPool()
    {
        // Loop through all active objects in the pool and release them
        for (int i = arrowContainer.childCount - 1; i >= 0; i--)
        {
            var child = arrowContainer.GetChild(i).GetComponent<Arrow>();
            if (child != null && child.isActiveAndEnabled)
            {
                pool.Release(child);
            }
        }

    }
    #endregion

    private void OnEnable()
    {
        SetupPool();
        element = EquipmentDataController.Instance.GetEquippedWeapon().ElementType;
    }

    private void OnDisable()
    {
        ClearPool();
    }

    //Animation event : Make sword slash when attack
    public void SwordSlash()
    {
        swordAnimator.SetTrigger(swordSlash);
    }    
    
    public void WiggleStaff()
    {
        magicSealAnimator.SetTrigger(wiggle);
    }    

    public void RaiseBow()
    {
        //bowAnimator.SetTrigger(raise);
        Arrow arrow = pool.Get();
        arrow.SetSprite(arrowSO.GetArrowSprite(element));
    }

    public void ShootArrow()
    {
        OnArrowShoot.Invoke(arrowTarget);
    }
}
