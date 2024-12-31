using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Dialogue/SkillInformation")]
public class InformationSkillSo : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField][TextArea] private string skillKey;

    public Sprite Icon {  get { return icon; } set { icon = value; } }
    public string Skillkey => skillKey;
}
