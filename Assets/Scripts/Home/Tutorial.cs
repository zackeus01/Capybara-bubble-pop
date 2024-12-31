using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button[] disableBtn;
    public GameObject mousePref;
    public RectTransform level1Pos;
    public LevelList _levelList;
    public RectTransform parent;
    public ScrollRect rect;
    private void Reset()
    {
        _levelList = FindFirstObjectByType<LevelList>();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            int check = PlayerPrefs.GetInt("CurrentLevel");
            if (check != 1)
            {
                PlayerPrefs.SetInt("DoneTutorial", 1);
            }
        }
        InitTutorial();
    }
    public void InitTutorial()
    {
        if (!PlayerPrefs.HasKey("DoneTutorial"))
        {
            for (int i = 0; i < disableBtn.Length; i++)
            {
                disableBtn[i].interactable = false;
            }
            rect.horizontal = false;
            rect.vertical = false;
            level1Pos = _levelList.level1Pos.mousePos.GetComponent<RectTransform>();
            GameObject initMouse = Instantiate(mousePref, level1Pos);
            initMouse.transform.position = new Vector3(level1Pos.position.x, level1Pos.position.y, 1f);
        } 
        else
        {
            for (int i = 0; i < disableBtn.Length; i++)
            {
                disableBtn[i].interactable = true;
            }
            rect.horizontal = false;
            rect.vertical = true;
        }
    }
}
