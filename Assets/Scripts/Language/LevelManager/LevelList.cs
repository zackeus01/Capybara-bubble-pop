using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelList : MonoBehaviour
{
    [Header("LevelSelectionArea")]
    [SerializeField] private RectTransform levelArea;      // The container to hold level circles

    [Header("Level")]
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private UILevel _uiLevelPrefab;
    [SerializeField] private RectTransform levelContainer;      // The container to hold level circles

    [Header("Wave")]
    [SerializeField] private float waveAmplitude = 50f;     // Amplitude of the wave
    [SerializeField] private float waveFrequency = 0.5f;    // Frequency of the wave
    [SerializeField] private float verticalSpacing = 100f;  // Vertical spacing between levels

    [Header("Rock")]
    [SerializeField] private GameObject rockPrefab;           //Prefab for rocks
    [SerializeField] private int rocksPerSpace;               //Number of rocks between each levels
    [SerializeField] private RectTransform rockContainer;      // The container to hold rocks

    [Header("DialogueBox")]
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private GameObject Gacha;


    private readonly List<GameObject> uiObjects = new List<GameObject>();
    private RectTransform currentLevelTransform;
    public UILevel level1Pos;
    public GameObject transition;
    public Image imgGo;
    private void OnEnable()
    {
        //PrefabUtility.UnpackPrefabInstance(homeObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
        CreateLevelButtons();
    }
    private void OnDisable()
    {
        RemoveLevelListMenu();
    }

    private void CreateLevelButtons()
    {
        int currentLevel = PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 0);

        if (currentLevel == 0)
        {
            ++currentLevel;
            PlayerPrefs.SetInt(PlayerPrefsConst.CURRENT_LEVEL, 1);
        }

        if (_levels == null || _levels.Length <= 0)
            return;

        // Clear existing level circles
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }

        // Clear existing rocks
        foreach (Transform child in rockContainer)
        {
            Destroy(child.gameObject);
        }

        var numOfObjects = _levels.Length * (1 + rocksPerSpace);
        SetAreaHeight(numOfObjects); //Set the Level Container's height

        int index = 0;
       
        for (int i = 0; i < _levels.Length; i++)
        {
            // Instantiate a new level circle
            UILevel levelCircle = Instantiate(_uiLevelPrefab, levelContainer);
            levelCircle.LevelData = _levels[i];
            levelCircle._transition = transition;
            levelCircle._image = imgGo;
            levelCircle.LoadData();
            // Check if the level is a boss level
            if ((i + 1) % 10 == 0) 
            {
                levelCircle.BossLevel(); 
            }
            bool isBossLevel = (i + 1) % 10 == 0;
            if (isBossLevel)
            {
                // Show unlockIconBoss for boss levels that are unlocked
                levelCircle.unlockIconBoss.SetActive(currentLevel >= (i + 2));
            }

            if (i == 0)
            {
                //Debug.LogWarning("Here");
                level1Pos = levelCircle;
            }
            uiObjects.Add(levelCircle.gameObject);
            //--------------
            levelCircle.button.enabled = (i <= currentLevel - 1);
            if (!isBossLevel)
            {
                levelCircle.starHolder.SetActive((i <= currentLevel - 1));
                levelCircle.unlockIcon.SetActive((i <= currentLevel - 1));
            }

            //if (i <= currentLevel - 1)
            //{
            //    levelCircle.unlockIcon.SetActive(true);
            //    levelCircle.starHolder.SetActive(true);
            //    //thêm if nếu người dùng finished level và đạt 3 sao hoặc gì đó ở đây
            //}
            //else
            //{
            //    levelCircle.unlockIcon.SetActive(false);
            //    levelCircle.starHolder.SetActive(false);
            //    levelCircle.button.enabled = false;
            //}

            //Re-position levels
            RepositionLevels(levelCircle.gameObject.GetComponent<RectTransform>(), index, numOfObjects);

            if (i == currentLevel - 1)
            {
                currentLevelTransform = levelCircle.GetComponent<RectTransform>();
                levelCircle.ShowVfx();
            }
            if (currentLevel == 10)
            {
                if (PlayerPrefs.GetInt("DialogueShown", 0) == 0)
                {
                    DialogueBox.SetActive(true);
                    PlayerPrefs.SetInt("DialogueShown", 1);
                    PlayerPrefs.Save();
                }
            }
            if(currentLevel >= 10)
            {
                Gacha.SetActive(true);
            }

            for (int j = 0; j < rocksPerSpace; j++)
            {
                GameObject rock = Instantiate(rockPrefab, rockContainer);
                uiObjects.Add(rock);
                index += 1;
                RepositionLevels(rock.gameObject.GetComponent<RectTransform>(), index, numOfObjects);
            }

            index += 1;
        }

        SetLevelAreaPosition();
    }

    private void SetAreaHeight(int numberOfLevels)
    {
        // Calculate total height needed for the LevelContainer
        float totalHeight = numberOfLevels * verticalSpacing;
        //Debug.Log("Total height: " + totalHeight);

        //// Set the position of the LevelContainer
        levelArea.anchorMin = new Vector2(0.5f, 0f); // Middle bottom
        levelArea.anchorMax = new Vector2(0.5f, 0f); // Middle bottom

        // Set the height of the LevelContainer
        levelArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    private void SetLevelAreaPosition()
    {

        // Calculate the visible height of the viewport (the screen area that shows the levels)
        float visibleHeight = levelArea.parent.GetComponent<RectTransform>().rect.height;

        // Calculate the offset needed to center the current level
        float offset = currentLevelTransform.anchoredPosition.y - (visibleHeight / 3);

        // Ensure the offset doesn't go beyond the limits of the content
        offset = Mathf.Clamp(offset, 0, levelArea.GetComponent<RectTransform>().rect.height - visibleHeight);

        //Debug.Log("offset: " + offset);

        // Adjust the level container's position
        levelArea.anchoredPosition = new Vector2(levelArea.anchoredPosition.x, -(offset));
    }

    private void RepositionLevels(RectTransform levelRect, int order)
    {
        // Calculate the position for the wave
        float yPos = order * verticalSpacing; // Vertical spacing between levels
        float xPos = Mathf.Sin(order * waveFrequency) * waveAmplitude; // Calculate wave effect

        // Set the anchor to the middle bottom of the parent
        levelRect.anchorMin = new Vector2(0.5f, 0f); // Middle bottom
        levelRect.anchorMax = new Vector2(0.5f, 0f); // Middle bottom

        // Set the pivot to the middle bottom
        levelRect.pivot = new Vector2(0.5f, 0f); // Middle bottom

        // Set the anchored position to (0, 0) to align it with the anchor
        levelRect.anchoredPosition = new Vector2(0, 0);

        levelRect.anchoredPosition = new Vector2(xPos, yPos);
    }

    private void RepositionLevels(RectTransform rect, int i, int numberOfLevels)
    {
        // Calculate the position for each level
        float yPos = i * verticalSpacing; // Vertical spacing between levels

        // Apply damping only near the edges (first 10% and last 10% of levels)
        float normalizedPosition = (float)i / (numberOfLevels - 1); // Range from 0 (start) to 1 (end)
        float dampingFactor = 1f;

        if (normalizedPosition < 0.1f)  // First 10% of levels
        {
            dampingFactor = Mathf.Lerp(0.5f, 1f, normalizedPosition / 0.1f);  // Gradually increase amplitude
        }
        else if (normalizedPosition > 0.9f)  // Last 10% of levels
        {
            dampingFactor = Mathf.Lerp(1f, 0.5f, (normalizedPosition - 0.9f) / 0.1f);  // Gradually decrease amplitude
        }

        // Calculate the wave effect with the damping applied at the edges
        float xPos = Mathf.Sin(i * waveFrequency) * waveAmplitude * dampingFactor;

        // If we're not at the last level, apply a further damping to the next level
        if (i < numberOfLevels - 1)
        {
            // Calculate the next level's damping factor
            float nextDampingFactor = 1f;
            float nextNormalizedPosition = (float)(i + 1) / (numberOfLevels - 1);

            if (nextNormalizedPosition < 0.1f) // Next level is in the first 10%
            {
                nextDampingFactor = Mathf.Lerp(0.5f, 1f, nextNormalizedPosition / 0.1f);
            }
            else if (nextNormalizedPosition > 0.9f) // Next level is in the last 10%
            {
                nextDampingFactor = Mathf.Lerp(1f, 0.5f, (nextNormalizedPosition - 0.9f) / 0.1f);
            }

            // Apply additional offset for the next level
            float nextXPos = Mathf.Sin((i + 1) * waveFrequency) * waveAmplitude * nextDampingFactor;
            xPos += (nextXPos - xPos) * 0.5f; // Average the position with a small offset
        }

        // Set the anchor to the middle bottom of the parent
        rect.anchorMin = new Vector2(0.5f, 0f); // Middle bottom
        rect.anchorMax = new Vector2(0.5f, 0f); // Middle bottom

        // Set the pivot to the middle bottom
        rect.pivot = new Vector2(0.5f, 0f); // Middle bottom

        // Set the anchored position to (0, 0) to align it with the anchor
        rect.anchoredPosition = new Vector2(0, 0);

        // Apply the final calculated position
        rect.anchoredPosition = new Vector2(xPos, yPos);
    }

    private void RemoveLevelListMenu()
    {
        for (int i = 0; i < uiObjects.Count; i++)
        {
            Destroy(uiObjects[i].gameObject);
        }

        uiObjects.Clear();
    }

    //// This method is called whenever a serialized field in the inspector changes
    //private void OnValidate()
    //{
    //    Debug.Log("Value changed");
    //    // Only trigger when in Play mode
    //    if (Application.isPlaying)
    //    {
    //        Debug.Log("Recreate");
    //        // Call CreateLevelButtons when values change in Play mode
    //        CreateLevelButtons();
    //    }
    //}

}
