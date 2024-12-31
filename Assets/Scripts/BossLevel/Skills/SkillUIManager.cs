using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private List<SkillUI> skillUIs;

    public List<SkillUI> SkillUIs { get { return skillUIs; } }

    private void Awake()
    {
        BossUIEvent.OnPlayerSkillUISetup.AddListener(InitializeAllSkillUIs);
    }

    public void InitializeAllSkillUIs(List<SkillSO> skills)
    {
        for (int i = 0; i < skillUIs.Count; i++)
        {
            skillUIs[i].Init(skills[i]);
        }
    }
}
