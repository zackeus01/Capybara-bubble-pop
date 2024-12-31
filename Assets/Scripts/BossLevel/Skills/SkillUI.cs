using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;

    [Header("Button Properties")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtMana;
    [SerializeField] private Button skillButton;
    [SerializeField] private Image grayCover;
    [SerializeField] private GameObject light;

    private SkillState skillState;
    private bool isSkillActivated;
    private bool isManaSufficient;

    public Image Icon { get { return icon; } }
    public TextMeshProUGUI TxtMana { get { return txtMana; } }
    public SkillSO SkillSO { get { return skillSO; } }
    public Button SkillButton { get { return skillButton; } }

    public void Init(SkillSO skillSO)
    {
        this.skillSO = skillSO;

        light.SetActive(false);
        icon.sprite = skillSO.Icon;
        txtMana.text = skillSO.ManaCost.ToString();

        BossUIEvent.OnPlayerManaUpdate.AddListener(OnManaChanged);

        ButtonDisableState();
        isSkillActivated = false;
    }

    public void OnSkillCLicked()
    {
        CustomDebug.Log("Skill CLICKED!", Color.cyan);
        ButtonActivatedState();
        BossEvent.OnPlayerActivateSkill.Invoke(this.skillSO);
    }

    public void OnManaChanged(float playerMana)
    {
        CheckMana(playerMana);
        if (skillState != SkillState.Activated)
        {
            SwitchState();
        }
    }

    public void OnFinishSkill(SkillSO skillSO)
    {
        CustomDebug.Log("Skill FINISHED!", Color.cyan);
        if (this.skillSO == skillSO && skillState == SkillState.Activated)
        {
            SwitchState();
        }
    }

    private void CheckMana(float playerMana)
    {
        isManaSufficient = playerMana >= skillSO.ManaCost ? true : false;
    }

    #region States
    private void ButtonDisableState()
    {
        skillState = SkillState.Disable;
        light.SetActive(false);
        grayCover.gameObject.SetActive(true);
        this.skillButton.interactable = false;
    }

    private void ButtonAvailableState()
    {
        skillState = SkillState.Available;
        light.SetActive(false);
        grayCover.gameObject.SetActive(false);
        this.skillButton.interactable = true;
    }

    private void ButtonActivatedState()
    {
        skillState = SkillState.Activated;
        light.SetActive(true);
        grayCover.gameObject.SetActive(false);
        this.skillButton.interactable = false;
        BossUIEvent.OnPlayerFinishSkill.AddListener(OnFinishSkill);
    }

    private void SwitchState()
    {
        switch (skillState)
        {
            case SkillState.Disable:
                if (isManaSufficient) ButtonAvailableState();
                break;
            case SkillState.Available:
                if (isSkillActivated) ButtonActivatedState();
                else if (!isManaSufficient) ButtonDisableState();
                break;
            case SkillState.Activated:
                if (isManaSufficient) ButtonAvailableState();
                else ButtonDisableState();
                break;
            default: break;
        }
    }
    #endregion
}

public enum SkillState
{
    Disable,
    Available,
    Activated
}
