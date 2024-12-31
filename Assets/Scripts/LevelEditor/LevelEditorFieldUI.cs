using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorFieldUI : Singleton<LevelEditorFieldUI>
{
    [SerializeField] private int width, height;
    [SerializeField] private GridUI cellPrefab;
    [SerializeField] private ChooseButtonUI buttonPrefab;
    [SerializeField] private GameObject hexGrid;
    [SerializeField] private List<GridUI> cells;

    [Header("Ball")]
    [SerializeField] private List<SkinBallSO> ballImage;
    [SerializeField] private GameObject ballButtonSpawn;
    [Header("BallType")]
    [SerializeField] private List<SkinBallSO> coverImage;
    [SerializeField] private GameObject typeButtonSpawn;

    [Header("Current Selection")]
    [SerializeField] private Image currentBallImage;
    [SerializeField] private TextMeshProUGUI currentBallText;
    [SerializeField] private Image currentTypeImage;
    [SerializeField] private TextMeshProUGUI currentTypeText;
    [SerializeField] private BallColor currentBallColor;
    [SerializeField] private BallType currentBallType;
    [SerializeField] private TMP_InputField currentInputField;

    private TextAsset levelAsset;
    [SerializeField] public GridUI SelectedCell { get; set; }
    public BallColor CurrentBallColor => currentBallColor;
    public BallType CurrentBallType => currentBallType;


    public List<SkinBallSO> CoverImage { get { return coverImage; } }
    public List<SkinBallSO> BallImage { get { return ballImage; } }

    private void Start()
    {
        InitGrid();
        InitButton();
    }

    public void UpdateGrid()
    {
        int.TryParse(currentInputField.text, out height);
        InitGrid();
    }

    private void InitGrid()
    {
        currentInputField.text = height.ToString();

        cells.ForEach((c) =>
        {
            Destroy(c.gameObject);
        });

        cells.Clear();

        for (int i = 0; i < height; ++i)
            for (int j = 0; j < width; ++j)
            {
                GridUI cell = Instantiate(cellPrefab, hexGrid.transform);
                cells.Add(cell);
            }

        cells.Reverse();
    }
    private void InitButton()
    {
        // Initialize buttons for ball colors
        for (int i = 0; i < ballImage.Count; ++i)
        {
            ChooseButtonUI button = Instantiate(buttonPrefab, ballButtonSpawn.transform);
            int index = i;  // Capture the current value of i
            button.SetButton(ballImage[index].Sprite, ballImage[index].BallColor.ToString());
            button.btn.onClick.AddListener(() =>
            {
                SetCurrentBallColor(index);
            });
        }

        // Initialize buttons for ball types
        for (int i = 0; i < coverImage.Count; ++i)
        {
            ChooseButtonUI button = Instantiate(buttonPrefab, typeButtonSpawn.transform);
            int index = i;  // Capture the current value of i
            button.SetButton(coverImage[index].Sprite, coverImage[index].BallType.ToString());
            button.btn.onClick.AddListener(() =>
            {
                SetCurrentBallType(index);
            });
        }
    }
    public void SaveGrid()
    {
        // Save file async
        StandaloneFileBrowser.SaveFilePanelAsync("Save File", "", "", "", (string path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                // Use StringBuilder for more efficient string concatenation
                StringBuilder line = new StringBuilder();

                // Process each row
                for (int rowStart = cells.Count - width; rowStart >= 0; rowStart -= width)
                {
                    // Use another StringBuilder for building the current row
                    StringBuilder tempRow = new StringBuilder();

                    // Iterate through the cells in reverse order for the current row
                    for (int j = width - 1; j >= 0; j--)
                    {
                        int index = rowStart + j;

                        if (!cells[index].HasBall)
                        {
                            tempRow.Append("ZR");
                        }
                        else
                        {
                            string color = BallColorConverter.BallColorToText(cells[index].BallColor);
                            string type = BallTypeConverter.BallTypeToText(cells[index].BallType);
                            tempRow.Append(color).Append(type);
                        }

                        // Add separator between cells except for the last one in the row
                        if (j > 0)
                        {
                            tempRow.Append("-");
                        }
                    }

                    // Append the constructed row to the final string
                    line.AppendLine(tempRow.ToString());
                }

                try
                {
                    // Write to the file at the selected path
                    File.WriteAllText(path, line.ToString());
                    Debug.Log("Grid saved successfully!");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to save grid: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("No save path selected.");
            }
        });

    }

    public void ResetGrid()
    {
        foreach (var item in cells)
        {
            item.ResetGrid();
        }
    }

    public void LoadFile()
    {
        ResetGrid();

        // Open file async
        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) =>
        {
            if (paths != null && paths.Length > 0)
            {
                // Load the file, assuming paths[0] is the path to the file
                string filePath = paths[0];

                // Read the content of the file
                string fileContent = File.ReadAllText(filePath);

                // Create a new TextAsset with the file content
                levelAsset = new TextAsset(fileContent);

                GenerateGameField();
            }
            else
            {
                Debug.LogWarning("No file selected.");
            }
        });
    }

    private void GenerateGameField()
    {
        List<string> types = new List<string>();

        StringReader content = new StringReader(levelAsset.text);
        while (content.Peek() >= 0)
        {
            types.AddRange(content.ReadLine().Split('-'));
        }
        //ztypes.ForEach(s => Debug.Log(s));

        int cellIndex = width - 1;
        int fromIndex = cells.Count - width;

        //Spawn ball from left to right, from top to bottom
        foreach (string type in types)
        {
            //if ball reach end of line -> decrease the spawn cell height by 1

            if (cellIndex == -1)
            {
                cellIndex = width - 1;
                fromIndex -= width;
            }

            //Debug.Log($"{fromIndex} {cellIndex}");

            if (type.Equals("ZR"))
            {
                cellIndex--;
                continue;
            }

            //Get cell from list
            GridUI cell = cells[fromIndex + cellIndex];
            SetCell(type, ref cell);

            cellIndex--;
        }
    }

    private void SetCell(string ballStr, ref GridUI cell)
    {
        string color = ballStr.Substring(0, 2);
        string type = ballStr.Substring(2);

        BallColor ballColor = BallColorConverter.TextToBallColor(color);
        BallType ballType = BallTypeConverter.TextToBallType(type);

        cell.SetBallType(ballType);
        cell.SetBallColor(ballColor);

        Debug.Log($"{cell.BallColor} {cell.BallType}", cell);
    }

    public void SetBallColor(string color)
    {
        SelectedCell.SetBallColor(BallColorConverter.TextToBallColor(color));
    }
    public void SetBallType(string type)
    {
        SelectedCell.SetBallType(BallTypeConverter.TextToBallType(type));
    }

    private void SetCurrentBallColor(int index)
    {
        if (currentBallType.Equals(BallType.Normal) || currentBallType.Equals(BallType.Grass) || currentBallType.Equals(BallType.Web))
        {
            currentBallColor = ballImage[index].BallColor;
            currentBallImage.sprite = ballImage[index].Sprite;
            currentBallText.text = BallColorConverter.BallColorToText(currentBallColor);
            return;
        }

        currentBallColor = ballImage[0].BallColor;
        currentBallImage.sprite = ballImage[0].Sprite;
        currentBallText.text = BallColorConverter.BallColorToText(currentBallColor);
    }

    private void SetCurrentBallType(int index)
    {
        currentBallType = coverImage[index].BallType;

        if (!(currentBallType.Equals(BallType.Normal) || currentBallType.Equals(BallType.Grass) || currentBallType.Equals(BallType.Web)))
            SetCurrentBallColor(0);

        currentTypeImage.sprite = coverImage[index].Sprite;
        currentTypeText.text = BallTypeConverter.BallTypeToText(currentBallType);
    }
}
