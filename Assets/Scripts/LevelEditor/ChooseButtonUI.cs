using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButtonUI : MonoBehaviour
{
    [SerializeField] public Button btn;
    [SerializeField] TextMeshProUGUI text;

    public void SetButton(Sprite sprite, string name)
    {
        btn.image.sprite = sprite;
        text.text = name;
    }
}
