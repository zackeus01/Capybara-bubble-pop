using DG.Tweening;
using UnityEngine;

public class StarVFX : MonoBehaviour
{
    [SerializeField]
    private Transform star;

    public bool isRewarded = false;
    private void Reset()
    {
        star = this.transform.GetChild(0);
    }

    public void GetStar()
    {
        //Debug.Log("Get Star");
        if (isRewarded) return;
        star.gameObject.SetActive(false);
        isRewarded = true;
        star.localRotation = Quaternion.Euler(0, 0, 100f);
        star.localScale = new Vector3(2f, 2f, 1f);
        star.gameObject.SetActive(true);
        star.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutExpo);
        star.DORotate(new Vector3(0f, 0f, 0f), 0.4f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }
    public void GetStarNoVFX()
    {
        if (isRewarded) return;
        star.gameObject.SetActive(false);
        isRewarded = true;
        star.gameObject.SetActive(true);
    }
}
