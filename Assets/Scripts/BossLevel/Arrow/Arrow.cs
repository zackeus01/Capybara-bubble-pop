using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        WeaponAnimationEvent.OnArrowShoot.AddListener(Shoot);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDisable()
    {
        WeaponAnimationEvent.OnArrowShoot.RemoveListener(Shoot);
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void Shoot(Transform targetTransform)
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 20);

        Sequence sequence = DOTween.Sequence();

        // Calculate the control point for the parabolic curve
        Vector3 startPoint = transform.position;
        Vector3 endPoint = targetTransform.position;

        // Create a midpoint slightly above the straight line between start and end
        Vector3 controlPoint = Vector3.Lerp(startPoint, endPoint, 0.5f) + Vector3.up * 2f;

        // Define the parabolic path
        Vector3[] path = new Vector3[] { startPoint, controlPoint, endPoint };

        // Calculate duration dynamically based on distance and speed
        float distance = Vector3.Distance(startPoint, endPoint);
        float duration = distance / moveSpeed;

        // Move along the parabolic path
        sequence.Join(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad) // Smooth easing
            .OnComplete(() => {
                Destroy(gameObject); // Destroy the arrow
                PlayerAnimationEvent.OnAttackHit.Invoke();
                PlayerAnimationEvent.OnAttackHitBoss.Invoke();
                PlayerAnimationEvent.OnAnimationEnd.Invoke();
            }));

        sequence.Join(transform.DORotate(new Vector3(0, 0, -20), duration));

        sequence.Play();
    }
}
