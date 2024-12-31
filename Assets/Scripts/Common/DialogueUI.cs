using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject rewardpop;
    [SerializeField] private DialogueObject dia;
    [SerializeField] private DialogueObject diaPackpack;
    [SerializeField] private Button btnPackback;
    private TypewriteEffect typewriteEffect;
    private bool doneTutorial = false;
    private StringTable stringTable;

    private void Start()
    {
        typewriteEffect = GetComponent<TypewriteEffect>();
        CloseDialogueBox();
        LoadDialogueStringTable();
        ShowDialogue(dia);
        PlayerPrefs.SetInt("donedia", 0);
    }

    private void LoadDialogueStringTable()
    {
        var table = LocalizationSettings.StringDatabase.GetTableAsync("MainScene");
        table.Completed += (handle) => { stringTable = handle.Result; };
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        if(DialogueBox != null)
        {
      DialogueBox.SetActive(true);
        }
        StartCoroutine(StepDialogue(dialogueObject));
    }

    private IEnumerator StepDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogueKey in dialogueObject.DialogueKeys)
        {
            string localizedText = GetLocalizedText(dialogueKey);
            yield return typewriteEffect.Run(localizedText, textLabel);
            yield return new WaitUntil(() => Input.touchCount > 0);
        }
        CloseDialogueBox();
        
        if (PlayerPrefs.GetInt("donedia", 0) == 0)
        {
            if(!doneTutorial)
            {
                overlayPanel.SetActive(true);
                btnPackback.onClick.AddListener(() =>
                {
                    overlayPanel.SetActive(false);
                    doneTutorial = true;
                    ShowDialogue(diaPackpack);
                   


                });
                foreach (string dialogueKey in diaPackpack.DialogueKeys)
                {
                    string localizedText = GetLocalizedText(dialogueKey);
                    yield return typewriteEffect.Run(localizedText, textLabel);
                    yield return new WaitUntil(() => Input.touchCount > 0);
                }
                rewardpop.SetActive(true);
                PlayerPrefs.SetInt("donedia", 1);
                Destroy(DialogueBox);
            }
           
        }
        
    }

    private string GetLocalizedText(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("MainScene", key);
    }

    private void CloseDialogueBox()
    {
        if (DialogueBox != null)
        {
            DialogueBox.SetActive(false);
            textLabel.text = string.Empty;
        }
        
    }
}
