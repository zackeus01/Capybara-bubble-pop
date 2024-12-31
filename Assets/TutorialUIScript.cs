using System;
using TMPro;
using UnityEngine;

public class TutorialUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guidetxt;
    [SerializeField] private GameObject[] listPicture;
    [SerializeField] private TextMeshProUGUI btnText;
    [SerializeField] private Shooter shooter;
    [SerializeField] private CheckPush checkPush;
    [SerializeField] private GameObject nextButton;

    private bool isDone;
    private int currentStep;

    private void Awake()
    {
        LoadComponent();
        GameplayEvent.OnGameFieldSetupDone.AddListener(OpenCatapult);
        checkPush.isTutorialEnable = true;
    }

    private void LoadComponent()
    {
        shooter = FindObjectOfType<Shooter>();
        checkPush = FindObjectOfType<CheckPush>();
    }

    private void Start()
    {
        isDone = false;
        currentStep = 0; 
    }

    private void OpenCatapult()
    {
        CustomDebug.Log("IS done set to true", Color.yellow);
        isDone = true;
        nextButton.SetActive(true);
    }
    public void SetUpClickBtn()
    {
        if (!isDone) return; 

        currentStep++;
        if (currentStep == 1)
        {
            listPicture[0].SetActive(false);
            listPicture[1].SetActive(true);
            guidetxt.text = "Try to drop the balls!!";
            btnText.text = "I understand";
        }
        if (currentStep == 2)
        {
            checkPush.isTutorialEnable = false;
            shooter.UnlockCatapult();
            this.gameObject.SetActive(false);
        }
    }
}
