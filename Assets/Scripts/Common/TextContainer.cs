using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContainer : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> _easyBlocks = new List<TextAsset>();
    public List<TextAsset> EasyBlock { get => _easyBlocks; }
    [SerializeField]
    private List<TextAsset> _mediumBlocks = new List<TextAsset>();
    public List<TextAsset> MediumBlock { get => _mediumBlocks; }
    [SerializeField]
    private List<TextAsset> _hardBlocks = new List<TextAsset>();
    public List<TextAsset> HardBlock { get => _hardBlocks; }
}
