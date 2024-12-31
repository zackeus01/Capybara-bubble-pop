using UnityEngine;
using UnityEngine.UI;

public class LuckySpinPopupController : MonoBehaviour
{
    [SerializeField] private Button luckySpinButton;
    [SerializeField] private GameObject luckySpinPanel;

    void Start()
    {
        // Add event for button
        luckySpinButton.onClick.AddListener(OpenLuckySpin);
    }

    void OpenLuckySpin()
    {
        // Show lucky spin popup when click on the button
        Debug.Log("Popup opened");
        luckySpinPanel.SetActive(true);
    }
}