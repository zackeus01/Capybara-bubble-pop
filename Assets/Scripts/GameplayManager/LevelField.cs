using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelField : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private HexGrid _fieldGrid;
    [SerializeField] private Camera _camera;
    private readonly List<HexCell> cellWithBalls = new();
    private readonly List<Ball> _ballsOnField = new();
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private List<BallColor> ballColors;

    private Dictionary<BallColor, int> _ballsDestroyedInTurn = new Dictionary<BallColor, int>();
    public Dictionary<BallColor, int> BallsDestroyedInTurn { get => _ballsDestroyedInTurn; }

    private LevelData _levelData;
    private TextAsset _levelTextData;

    public bool isDropBallCompleted = false;
    public HexGrid FieldGrid { get => _fieldGrid; }
    public List<Ball> BallsOnField { get => _ballsOnField; }
    public List<BallColor> BallColorsOnFields { get => ballColors; }

    private bool canDestroyBall;

    private List<Ball> firstLineBalls = new List<Ball>();

    #region Check Push Parameter
    [Header("Check Push Parameter")]
    public Ball LastBallPos;
    public Ball FirstBallPos;
    #endregion

    #region Boss Map Block
    private TextContainer _bossBlockContainer;
    #endregion

    private void Awake()
    {
        GameplayEvent.OnActiveBallCollided.AddListener(SetActiveBallAtField);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(CheckCountBallsOnField);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(CheckLastAndFirstBall);
        GameplayEvent.OnGameFieldChanged.AddListener(CheckBallColorsOnField);
        HelperEvent.OnHelperBallCollided.AddListener(DoHelperAction);
        HelperEvent.OnAllHelperTargetDestroyed.AddListener(TryDropBalls);
        BossEvent.OnCharacterTurnStart.AddListener(ResetBossData);
        BossEvent.OnBossDestroyBall.AddListener(TryDestroyBallGroup);
        BossEvent.OnPlayerShootBallEnd.AddListener(CheckLastAndFirstBall);

        LoadComponent();
        SetUpLevelData();
    }

    private void LoadComponent()
    {
        _levelData = LevelDataHolder.LevelData;
        _bossBlockContainer = this.GetComponentInChildren<TextContainer>();
    }

    private void Start()
    {
        GenerateGameField();
        CheckLastAndFirstBall();
        GameplayEvent.OnGameFieldGenerated.Invoke(BallsOnField);
    }

    private void OnDestroy()
    {
        GameplayEvent.OnActiveBallCollided.RemoveListener(SetActiveBallAtField);
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(CheckCountBallsOnField);
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(CheckLastAndFirstBall);
        GameplayEvent.OnGameFieldChanged.RemoveListener(CheckBallColorsOnField);
        HelperEvent.OnAllHelperTargetDestroyed.RemoveListener(TryDropBalls);
        HelperEvent.OnHelperBallCollided.RemoveListener(DoHelperAction);
        BossEvent.OnCharacterTurnStart.RemoveListener(ResetBossData);
        BossEvent.OnBossDestroyBall.RemoveListener(TryDestroyBallGroup);
        BossEvent.OnPlayerShootBallEnd.RemoveListener(CheckLastAndFirstBall);
    }

    private void CheckLastAndFirstBall()
    {
        if (BallsOnField.Count <= 0) return;

        FirstBallPos = BallsOnField[0];
        LastBallPos = BallsOnField[0];

        foreach (Ball ball in _ballsOnField)
        {
            if (ball.transform.position.y > FirstBallPos.transform.position.y)
                FirstBallPos = ball;
            if (ball.transform.position.y < LastBallPos.transform.position.y)
                LastBallPos = ball;
        }
    }

    private void SetUpLevelData()
    {
        if (!_levelData)
        {
            Debug.LogWarning("Level data is not set!");
            return;
        }

        ballColors = new List<BallColor>();
        ballColors.AddRange(_levelData.BallColorsInLevel);

        //Find level data file for spawn ball
        _levelTextData = Resources.Load<TextAsset>($"{_levelData.PathToLevelFile}/{_levelData.LevelFile.name}");

        if (!_levelTextData)
        {
            Debug.LogError("Level file is not found!");
        }
    }

    private void GenerateGameField()
    {
        if (_levelData)
            GameplayEvent.OnLevelDataLoaded.Invoke(LevelDataHolder.LevelData);

        //Todo: Boss scene map spawn
        //-Read bool isBoss from levelData
        if (_levelData.IsBossLevel)
        {
            SpawnBossMap();
            return;
        }

        SpawnBallMapFromText(_levelTextData.text);
    }
    private void SpawnBossMap()
    {
        if (!_bossBlockContainer) return;

        for (int i = 0; i < 4; i++)
        {
            SpawnBlock();
        }
    }
    public void SpawnBlock()
    {
        _fieldGrid.AddCellRowToTopGrid(5);

        //Clear old first line ball
        if (firstLineBalls.Count > 0)
        {
            foreach (var ball in firstLineBalls)
            {
                ball.isFirstLineBall = false;
            }

            firstLineBalls.Clear();
        }

        BlockDifficulty difficulty = GetNextBlockDifficulty();

        TextAsset nextBlock = GetRandomBlock(difficulty);
        SpawnBallMapFromText(nextBlock.text);

        GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);
    }

    private BlockDifficulty GetNextBlockDifficulty()
    {
        int bossDiff = LevelDataHolder.LevelData.BossDifficulty;


        return BlockDifficulty.Easy;
    }

    private void SpawnBallMapFromText(string data)
    {

        List<string> types = new();

        StringReader ballData = new(data);

        while (ballData.Peek() >= 0)
        {
            types.AddRange(ballData.ReadLine().Split('-'));
        }

        int columnIndex = 0;
        int rowIndex = _fieldGrid.Cells.Length - _fieldGrid.Width;

        //Spawn ball from left to right, from top to bottom
        foreach (string type in types)
        {
            //If ball reach end of line -> continue spawn to bottom rows
            if (columnIndex == _fieldGrid.Width)
            {
                columnIndex = 0;
                rowIndex -= _fieldGrid.Width;
            }

            if (type.Equals("ZR"))
            {
                columnIndex++;
                continue;
            }

            //Get cell from list
            HexCell cell = _fieldGrid.Cells[rowIndex + columnIndex];
            Ball ball = FromStringToBall(type);
            //Check null ball
            if (!ball) continue;

            SetBallAtField(ball, cell);
            columnIndex++;
        }
    }

    private TextAsset GetRandomBlock(BlockDifficulty difficult)
    {
        List<TextAsset> blockList = new List<TextAsset>();
        switch (difficult)
        {
            case BlockDifficulty.Easy:
                {
                    blockList = _bossBlockContainer.EasyBlock;
                    break;
                }
            case BlockDifficulty.Medium:
                {
                    blockList = _bossBlockContainer.MediumBlock;
                    break;
                }
            case BlockDifficulty.Hard:
                {
                    blockList = _bossBlockContainer.HardBlock;
                    break;
                }
        }
            
        int randomIndex = Random.Range(0, blockList.Count);
        return _bossBlockContainer.EasyBlock[randomIndex];
    }

    private void CheckBallColorsOnField(List<Ball> ballOnField)
    {
        List<BallColor> colors = new List<BallColor>();

        foreach (var color in ballColors)
        {
            if (ballOnField.Exists(b => b.color.Equals(color))) continue;

            colors.Add(color);
        }

        colors.ForEach(c => ballColors.Remove(c));
    }

    private Ball FromStringToBall(string ballStr)
    {
        string color = ballStr.Substring(0, 2);
        string type = ballStr.Substring(2);

        BallColor ballColor = BallColorConverter.TextToBallColor(color);
        BallType ballType = BallTypeConverter.TextToBallType(type);

        return Instantiate(ballPrefab, this.transform).SetBall(ballColor, ballType);
    }

    private void SetBallAtField(Ball ball, HexCell cell)
    {
        if (ball == null)
        {
            Debug.LogWarning("Attached ball is null!");
            return;
        }

        if (cell == null)
        {
            Debug.LogWarning("Cell is not found");
            return;
        }

        ball.transform.SetParent(gameObject.transform);
        ball.TrailEfx.enabled = false;
        cell.SetBall(ball);
        ball.parentCell = cell;
        _ballsOnField.Add(ball);
        cellWithBalls.Add(cell);

        //Todo: fixed first line ball logic in boss scene
        if (cell.GetNeighbor(HexDirection.NW) == null && cell.GetNeighbor(HexDirection.NE) == null)
        {
            ball.isFirstLineBall = true;
            firstLineBalls.Add(ball);
        }

        ball.transform.position = cell.transform.position;
        ball.transform.rotation = cell.transform.rotation;
        ball.AddJoint(cell.transform.position);

        ball.rb2d.gravityScale = 1f;
        ball.rb2d.mass = 1f;
    }

    private void SetActiveBallAtField(Ball activeBall)
    {
        HexCell cell = activeBall.targetCell;

        if (cell == null)
        {
            activeBall.DestroyBall(ScoreType.Destroy, BallType.Normal);
            Debug.Log("OnAllFieldActionsEnd/LevelField/SetActiveBallAtField");
            
            if (LevelDataHolder.LevelData.IsBossLevel)
            {
                BossEvent.OnShootBallTurnEnd.Invoke(_ballsDestroyedInTurn);
            }
            else
            {
                GameplayEvent.OnAllFieldActionsEnd.Invoke();
            }
            return;
        }

        activeBall.isActiveBall = false;

        SetBallAtField(activeBall, cell);

        //Null listener
        GameplayEvent.OnActiveBallSetOnField.Invoke(activeBall);

        TryDestroyBallGroup(cell);
    }
    private void RemoveBallFromField(HexCell cell)
    {
        _ballsOnField.Remove(cell.GetBall());
        cell.RemoveBall();
        //Debug.Log(_ballsOnField.Count);
        GameplayEvent.onTotalBallLeftChange.Invoke(_ballsOnField.Count);
    }

    private void DropBallFromField(Ball dropBall)
    {
        dropBall.CoverSprite.sortingLayerName = "FallingBall";
        dropBall.BallSprite.sortingLayerName = "FallingBall";
        dropBall.PlayDropVFX();
        dropBall.rb2d.gravityScale = 4f;
        dropBall.isFalling = true;
        dropBall.parentCell = null;
        dropBall.CircleCollider.isTrigger = true;
        dropBall.JointConnection.enabled = false;
    }

    private void TryDestroyBallGroup(HexCell ballCell)
    {
        canDestroyBall = false;

        List<HexCell> ballGroupCells = GetBallGroupCells(ballCell, ballCell, 1);

        if (ballGroupCells == null)
        {
            Debug.LogWarning("Can't get ball group cells!");
            return;
        }

        if (!canDestroyBall)
        {
            ScoreController.Instance.ResetStack();
            Debug.Log("OnAllFieldActionsEnd/LevelField/TryDestroyBallGroup/!canDestroyBall");

            GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);

            if (LevelDataHolder.LevelData.IsBossLevel)
            {
                //Boss shooting state
                BossEvent.OnShootBallTurnEnd.Invoke(_ballsDestroyedInTurn);
            }
            else
            {
                GameplayEvent.OnAllFieldActionsEnd.Invoke();
            }

            return;
        }

        ScoreController.Instance.IncreaseStack();

        AchievementEvent.OnUpdateAchievement.Invoke(AchievementType.DestroyBall, ballGroupCells.Count);
        DailyMissionEvent.OnUpdateMissionData.Invoke(DailyMissionType.DestroyBall, ballGroupCells.Count);

        //Remove list ball
        StartCoroutine(DestroyBallList(ballGroupCells));
    }

    private IEnumerator DestroyBallList(List<HexCell> ballGroupCells)
    {
        float duration = 0.5f / ballGroupCells.Count;

        List<Ball> ballDestroyed = new List<Ball>();

        //Explode balls
        foreach (HexCell cell in ballGroupCells)
        {
            Ball ball = cell.GetBall();

            if (!ball) continue;

            ballDestroyed.Add(ball);
            AddToDestroyedList(ball.color, 1);
            RemoveBallFromField(cell);
            ball.DestroyBall(ScoreType.Destroy, BallType.Normal);
            ball.parentCell = null;
            cellWithBalls.Remove(cell);
            yield return new WaitForSeconds(duration);
        }

        //Spawn new ball block in boss level
        TrySpawnNewBlock();

        //Score calculate
        ScoreController.Instance.AddScore(ballGroupCells.Count, ScoreType.Destroy);

        //For boss scene
        PlayerEvent.OnBallAreaDestroyed.Invoke(ballDestroyed);

        GameplayEvent.OnBallGroupDestroyed.Invoke(_ballsOnField);

        TryDropBalls();
    }

    private void TrySpawnNewBlock()
    {
        if (LevelDataHolder.LevelData.IsBossLevel)
        {
            if (BallsOnField.Count <= 88)
            {
                SpawnBlock();
            }
        }
    }

    private void TryDropBalls()
    {
        //Debug.Log("Try drop balls");
        isDropBallCompleted = false;
        //List drop
        List<HexCell> dropedBallsCells = new List<HexCell>();
        //List ball that might drop
        List<HexCell> supposedBallCells = new List<HexCell>();

        foreach (Ball ballOnField in _ballsOnField)
        {
            //Skip if ball is on the first line (can not fall)
            if (ballOnField.isFirstLineBall)
                continue;

            //HexCell cell = _fieldGrid.GetCellFromPosition(ballOnField.rb2d.position);
            HexCell cell = ballOnField.parentCell;

            HexCell cellNeighborNW = cell.GetNeighbor(HexDirection.NW);
            if (cellNeighborNW != null && cellNeighborNW.GetBall() != null)
                continue;

            HexCell cellNeighborNE = cell.GetNeighbor(HexDirection.NE);
            if (cellNeighborNE != null && cellNeighborNE.GetBall() != null)
                continue;

            supposedBallCells.Add(cell);
            //cell.IsHighLight = true;
        }

        //New Drop ball algorithm
        #region New Drop ball algorithm
        //HexCell firstCell = new HexCell();
        //List<HexCell> cellBalls = new List<HexCell>();

        //foreach (var cell in cellWithBalls)
        //{
        //    cellBalls.Add(cell);
        //}

        //foreach (var cell in cellWithBalls)
        //{
        //    if (!cell.GetBall()) continue;
        //    if (cell.GetBall().isFirstLineBall)
        //    {
        //        firstCell = cell;
        //    }
        //}

        ////Find block of first cell
        //cellBalls.RemoveAll(c => FindBallBlock(firstCell).Contains(c));
        //int count = 0;
        //while (cellBalls.Count > 0)
        //{
        //    count++;
        //    if (count > 5)
        //    {
        //        Debug.Log("Count break!!!!");
        //        break;
        //    }

        //    //cellBalls.RemoveAll(c => c.GetBall() == null);
        //    HexCell nextTarget = cellBalls.FirstOrDefault(c => c.GetBall() != null);
        //    if (nextTarget == null) break;

        //    List<HexCell> hexCells = FindBallBlock(nextTarget);
        //    Debug.Log("hexCells " + hexCells.Count);
        //    Debug.Log("cellWithBalls " + cellWithBalls.Count);
        //    cellBalls.RemoveAll(c => hexCells.Contains(c));

        //    if (hexCells.Any(c => c.GetBall().ballType == BallType.Web)) continue;

        //    dropedBallsCells.AddRange(hexCells);
        //    cellWithBalls.RemoveAll(c => hexCells.Contains(c));
        //    Debug.Log("cellWithBalls " + cellWithBalls.Count);
        //}
        #endregion

        bool canDrop;

        //New Drop ball 2
        while (supposedBallCells.Count > 0)
        {
            HexCell target = supposedBallCells[0];
            supposedBallCells.RemoveAt(0);

            canDrop = true;

            List<HexCell> checkedDropingBallsCells = new List<HexCell>
            {
                target
            };

            //Debug.Log(target.name, target);

            Queue<HexCell> checkingDropingBallsCells = new Queue<HexCell>();


            //Debug.LogWarning("Start finding ball");
            target._sr.color = Color.white;
            //Debug.Log("Add queue " + target, target);


            checkingDropingBallsCells.Enqueue(target);

            //BFS (breath first search)
            while (checkingDropingBallsCells.Count > 0)
            {
                HexCell cell = checkingDropingBallsCells.Dequeue();

                //Debug.Log($"<color=red> Check Cell {cell}</color>", cell);

                if (cell.GetBall().ballType.Equals(BallType.Web) || cell.GetBall().isFirstLineBall) canDrop = false;

                foreach (var checkingCell in cell.Neighbors)
                {
                    if (!checkingCell || !checkingCell.GetBall()) continue;
                    if (checkedDropingBallsCells.Contains(checkingCell)) continue;

                    if (supposedBallCells.Contains(checkingCell))
                        supposedBallCells.Remove(checkingCell);

                    checkingDropingBallsCells.Enqueue(checkingCell);
                    checkedDropingBallsCells.Add(checkingCell);
                }

                #region Old Check Direction
                //    HexCell checkingCellNE = cell.GetNeighbor(HexDirection.NE);

                //    if (checkingCellNE != null && checkingCellNE.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellNE))
                //    {
                //        if (supposedBallCells.Contains(checkingCellNE))
                //            supposedBallCells.Remove(checkingCellNE);

                //        checkingDropingBallsCells.Enqueue(checkingCellNE);
                //        checkedDropingBallsCells.Add(checkingCellNE);
                //        //Debug.Log("Add queue " + checkingCellNE, checkingCellNE);
                //    }

                //    HexCell checkingCellNW = cell.GetNeighbor(HexDirection.NW);
                //    if (checkingCellNW != null && checkingCellNW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellNW))
                //    {
                //        if (supposedBallCells.Contains(checkingCellNW))
                //            supposedBallCells.Remove(checkingCellNW);

                //        checkingDropingBallsCells.Enqueue(checkingCellNW);
                //        checkedDropingBallsCells.Add(checkingCellNW);
                //        //Debug.Log("Add queue " + checkingCellNW, checkingCellNW);
                //    }

                //    HexCell checkingCellW = cell.GetNeighbor(HexDirection.W);
                //    //Checks if West Cell exist and hasn't already been checked
                //    if (checkingCellW != null && checkingCellW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellW))
                //    {
                //        //if cell W not in supposed list
                //        //if (!supposedBallCells.Contains(checkingCellW))
                //        //{
                //        //    HexCell checkingCellWNeighborNE = checkingCellW.GetNeighbor(HexDirection.NE);
                //        //    //Break if cell NE of cell W has ball;
                //        //    if (checkingCellWNeighborNE != null && checkingCellWNeighborNE.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellWNeighborNE))
                //        //    {
                //        //        checkedDropingBallsCells.Clear();
                //        //        break;
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    supposedBallCells.Remove(checkingCellW);
                //        //    checkingCellW._sr.color = Color.green;
                //        //}

                //        //If cellW has ball
                //        if (checkingCellW.GetBall() != null)
                //        {

                //            if (supposedBallCells.Contains(checkingCellW))
                //                supposedBallCells.Remove(checkingCellW);

                //            checkingDropingBallsCells.Enqueue(checkingCellW);
                //            checkedDropingBallsCells.Add(checkingCellW);
                //            //Debug.Log("Add queue " + checkingCellW, checkingCellW);
                //        }
                //    }

                //    HexCell checkingCellE = cell.GetNeighbor(HexDirection.E);
                //    if (checkingCellE != null && !checkedDropingBallsCells.Contains(checkingCellE))
                //    {
                //        //if (!supposedBallCells.Contains(checkingCellE))
                //        //{
                //        //    HexCell checkingCellENeighborNW = checkingCellE.GetNeighbor(HexDirection.NW);

                //        //    if (checkingCellENeighborNW != null && checkingCellENeighborNW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellENeighborNW))
                //        //    {
                //        //        checkedDropingBallsCells.Clear();
                //        //        break;
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    supposedBallCells.Remove(checkingCellE);
                //        //    checkingCellE._sr.color = Color.green;
                //        //}

                //        if (checkingCellE.GetBall() != null)
                //        {
                //            if (supposedBallCells.Contains(checkingCellE))
                //                supposedBallCells.Remove(checkingCellE);

                //            checkingDropingBallsCells.Enqueue(checkingCellE);
                //            checkedDropingBallsCells.Add(checkingCellE);
                //            //Debug.Log("Add queue " + checkingCellE, checkingCellE);
                //        }
                //    }

                //    HexCell checkingCellSW = cell.GetNeighbor(HexDirection.SW);
                //    if (checkingCellSW != null && checkingCellSW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellSW))
                //    {
                //        if (supposedBallCells.Contains(checkingCellE))
                //            supposedBallCells.Remove(checkingCellE);

                //        checkingDropingBallsCells.Enqueue(checkingCellSW);
                //        checkedDropingBallsCells.Add(checkingCellSW);
                //        //Debug.Log("Add queue " + checkingCellSW, checkingCellSW);
                //    }

                //    HexCell checkingCellSE = cell.GetNeighbor(HexDirection.SE);
                //    if (checkingCellSE != null && checkingCellSE.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellSE))
                //    {
                //        if (supposedBallCells.Contains(checkingCellE))
                //            supposedBallCells.Remove(checkingCellE);

                //        checkingDropingBallsCells.Enqueue(checkingCellSE);
                //        checkedDropingBallsCells.Add(checkingCellSE);
                //        //Debug.Log("Add queue " + checkingCellSE, checkingCellSE);
                //    }
                #endregion
            }

            //Debug.Log($"<color=yellow>Checked Dropping Ball cells</color>");
            //checkedDropingBallsCells.ForEach(d => Debug.Log($"<color=green>checked</color> {d}", d));

            if (canDrop) dropedBallsCells.AddRange(checkedDropingBallsCells);
        }

        #region Old Drop ball algorithm 
        //Using flood-like and BFS to find isolation ball area

        //for (int i = 0; i < supposedBallCells.Count; i++)
        //{
        //    hasWebBall = false;

        //    List<HexCell> checkedDropingBallsCells = new List<HexCell>
        //    {
        //        supposedBallCells[i]
        //    };

        //    Debug.Log(supposedBallCells[i].name, supposedBallCells[i]);

        //    Queue<HexCell> checkingDropingBallsCells = new Queue<HexCell>();


        //    Debug.LogWarning("Start finding ball");
        //    supposedBallCells[i]._sr.color = Color.white;
        //    Debug.Log("Add queue " + supposedBallCells[i], supposedBallCells[i]);


        //    checkingDropingBallsCells.Enqueue(supposedBallCells[i]);



        //    //BFS (breath first search)
        //    while (checkingDropingBallsCells.Count > 0)
        //    {
        //        HexCell cell = checkingDropingBallsCells.Dequeue();

        //        if (cell.GetBall().ballType.Equals(BallType.Web)) hasWebBall = true;

        //        HexCell checkingCellW = cell.GetNeighbor(HexDirection.W);
        //        //Checks if West Cell exist and hasn't already been checked
        //        if (checkingCellW != null && !checkedDropingBallsCells.Contains(checkingCellW))
        //        {
        //            //if cell W not in supposed list
        //            if (!supposedBallCells.Contains(checkingCellW))
        //            {
        //                HexCell checkingCellWNeighborNE = checkingCellW.GetNeighbor(HexDirection.NE);
        //                //Break if cell NE of cell W has ball;
        //                if (checkingCellWNeighborNE != null && checkingCellWNeighborNE.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellWNeighborNE))
        //                {
        //                    checkedDropingBallsCells.Clear();
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                supposedBallCells.Remove(checkingCellW);
        //                checkingCellW._sr.color = Color.green;
        //            }

        //            //If cellW has ball
        //            if (checkingCellW.GetBall() != null)
        //            {
        //                checkingDropingBallsCells.Enqueue(checkingCellW);
        //                checkedDropingBallsCells.Add(checkingCellW);
        //            }
        //        }

        //        HexCell checkingCellE = cell.GetNeighbor(HexDirection.E);
        //        if (checkingCellE != null && !checkedDropingBallsCells.Contains(checkingCellE))
        //        {
        //            if (!supposedBallCells.Contains(checkingCellE))
        //            {
        //                HexCell checkingCellENeighborNW = checkingCellE.GetNeighbor(HexDirection.NW);

        //                if (checkingCellENeighborNW != null && checkingCellENeighborNW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellENeighborNW))
        //                {
        //                    checkedDropingBallsCells.Clear();
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                supposedBallCells.Remove(checkingCellE);
        //                checkingCellE._sr.color = Color.green;
        //            }

        //            if (checkingCellE.GetBall() != null)
        //            {
        //                checkingDropingBallsCells.Enqueue(checkingCellE);
        //                checkedDropingBallsCells.Add(checkingCellE);
        //            }
        //        }

        //        HexCell checkingCellSW = cell.GetNeighbor(HexDirection.SW);
        //        if (checkingCellSW != null && checkingCellSW.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellSW))
        //        {
        //            checkingDropingBallsCells.Enqueue(checkingCellSW);
        //            checkedDropingBallsCells.Add(checkingCellSW);
        //        }

        //        HexCell checkingCellSE = cell.GetNeighbor(HexDirection.SE);
        //        if (checkingCellSE != null && checkingCellSE.GetBall() != null && !checkedDropingBallsCells.Contains(checkingCellSE))
        //        {
        //            checkingDropingBallsCells.Enqueue(checkingCellSE);
        //            checkedDropingBallsCells.Add(checkingCellSE);
        //        }
        //    }

        //    Debug.Log("Checked Dropping Ball cells");
        //    checkedDropingBallsCells.ForEach(d => Debug.Log("checked " + d, d));

        //    if (!hasWebBall) dropedBallsCells.AddRange(checkedDropingBallsCells);
        //}
        #endregion

        //Debug.LogWarning("Start drop ball");
        //dropedBallsCells.ForEach(d => Debug.Log("Drop " + d, d));

        //Debug.Log("DropBall cell count " + dropedBallsCells.Count);

        //End of checking drop ball
        if (dropedBallsCells.Count <= 0)
        {
            float delayTime = 0.5f;
            Invoke(nameof(OnNoBallToDrop), delayTime);
            return;
        }

        //Sort list
        dropedBallsCells = SortedBallList(dropedBallsCells);
        //Debug.Log("Try drop balls/" + dropedBallsCells.Count);
        //Debug.Log("Ball on fields: " + dropedBallsCells.Count);
        GameplayEvent.OnBallsDropStarted.Invoke(dropedBallsCells);

        AchievementEvent.OnUpdateAchievement.Invoke(AchievementType.DropBall, dropedBallsCells.Count);
        DailyMissionEvent.OnUpdateMissionData.Invoke(DailyMissionType.DropBall, dropedBallsCells.Count);

        StartCoroutine(DropBallList(dropedBallsCells));
    }

    private void OnNoBallToDrop()
    {
        isDropBallCompleted = true;
        //Debug.Log("OnAllFieldActionsEnd/LevelField/TryDropBalls/dropedBallsCells.Count <= 0");

        GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);

        if (LevelDataHolder.LevelData.IsBossLevel)
        {
            BossEvent.OnShootBallTurnEnd.Invoke(_ballsDestroyedInTurn);
        }
        else
        {
            GameplayEvent.OnAllFieldActionsEnd.Invoke();
        }

        HelperEvent.OnHelperDeactivated.Invoke();

    }


    private void FindBallNeighBor(HexCell targetCell, List<HexCell> ballBlocks)
    {
        if (targetCell == null) return;
        foreach (HexCell cell in targetCell.Neighbors)
        {
            if (!cell) continue;
            if (ballBlocks.Contains(cell)) continue;
            if (!cell.GetBall()) continue;
            ballBlocks.Add(cell);
            //cell.IsHighLight = true;
            FindBallNeighBor(cell, ballBlocks);
        }
    }

    private List<HexCell> SortedBallList(List<HexCell> ballList)
    {
        return ballList
            .OrderBy(cell => cell.Coordinates.Y)
            .ThenByDescending(ball => ball.Coordinates.X)
            .ToList();
    }

    private IEnumerator DropBallList(List<HexCell> cellList)
    {
        //Debug.Log("count: " + cellList.Count);
        float dropTime = 0.5f / cellList.Count;

        foreach (HexCell dropedBallCell in cellList)
        {
            //dropedBallCell.IsHighLight = true;
            DropBallFromField(dropedBallCell.GetBall());

            AddToDestroyedList(dropedBallCell.GetBall().color, 2);

            RemoveBallFromField(dropedBallCell);
            yield return new WaitForSeconds(dropTime);
        }
        TrySpawnNewBlock();

        GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);

        ScoreController.Instance.AddScore(cellList.Count, ScoreType.Drop);

        yield return new WaitForSeconds(dropTime);

    }

    private List<HexCell> GetBallGroupCells(HexCell firstCell, HexCell ballCell, int ballCount)
    {
        if (ballCell == null || ballCell.GetBall() == null)
            return null;

        HashSet<HexCell> ballGroupCells = new HashSet<HexCell>();
        Queue<HexCell> cellsToCheck = new Queue<HexCell>();
        cellsToCheck.Enqueue(ballCell);
        ballGroupCells.Add(ballCell);

        while (cellsToCheck.Count > 0)
        {
            HexCell currentCell = cellsToCheck.Dequeue();

            foreach (HexCell neighbor in currentCell.Neighbors)
            {
                if (neighbor == null || neighbor.GetBall() == null)
                    continue;

                switch (neighbor.GetBall().ballType)
                {
                    case BallType.Web:
                    case BallType.Normal:
                        if (ballGroupCells.Contains(neighbor) || !neighbor.GetBall().color.Equals(currentCell.GetBall().color))
                            break;

                        ++ballCount;
                        ballGroupCells.Add(neighbor);
                        cellsToCheck.Enqueue(neighbor);
                        break;
                    case BallType.Grass:
                        neighbor.GetBall().DestroyCover();
                        if (ballGroupCells.Contains(neighbor) || !neighbor.GetBall().color.Equals(ballCell.GetBall().color))
                            break;

                        ++ballCount;
                        ballGroupCells.Add(neighbor);
                        cellsToCheck.Enqueue(neighbor);
                        break;

                    default:
                        if (currentCell != firstCell) break;

                        TriggerSpecialBall(ref ballGroupCells, firstCell, neighbor);
                        break;
                }
            }
        }

        if (!canDestroyBall) canDestroyBall = ballCount >= 3;
        return ballGroupCells.ToList();
    }

    #region Trigger Special Ball Algorithm
    private void TriggerSpecialBall(ref HashSet<HexCell> ballGroupCells, HexCell firstCell, HexCell ballCell)
    {

        switch (ballCell.GetBall().ballType)
        {
            case BallType.Blast:
                ballGroupCells.Add(ballCell);
                canDestroyBall = true;
                TriggerBlastBall(ref ballGroupCells, firstCell, ballCell);
                break;

            case BallType.Lightning:
                ballGroupCells.Add(ballCell);
                canDestroyBall = true;
                TriggerLightningBall(ref ballGroupCells, firstCell, ballCell);
                break;

            case BallType.Ziczac:
                ballGroupCells.Add(ballCell);
                canDestroyBall = true;
                TriggerZicZacBall(ref ballGroupCells, firstCell, ballCell);
                break;
            default:
                break;
        }

    }

    private void TriggerBlastBall(ref HashSet<HexCell> ballGroupCells, HexCell firstCell, HexCell cell)
    {
        foreach (var neighbor in cell.Neighbors)
        {
            if (neighbor == null || neighbor.GetBall() == null)
                continue;
            if (ballGroupCells.Contains(neighbor))
                continue;

            ballGroupCells.Add(neighbor);

            if (neighbor.GetBall().ballType.Equals(BallType.Normal))
                continue;

            TriggerSpecialBall(ref ballGroupCells, firstCell, neighbor);
        }
    }

    private void TriggerLightningBall(ref HashSet<HexCell> ballGroupCells, HexCell firstCell, HexCell ballCell)
    {
        HexCell randomCell;
        ballGroupCells.Add(ballCell);

        do
        {
            randomCell = FieldGrid.GetCellFromPosition(
                BallsOnField[(UnityEngine.Random.Range(0, BallsOnField.Count))].transform.position);
        } while (randomCell.Equals(ballCell) || !IsHexCellVisibleByCamera(randomCell) || ballGroupCells.Contains(randomCell));

        if (!randomCell.GetBall().ballType.Equals(BallType.Normal)) TriggerSpecialBall(ref ballGroupCells, firstCell, randomCell);
        else ballGroupCells.AddRange(GetBallGroupCells(firstCell, randomCell, 1));

        //Debug.Log(randomCell, randomCell);
    }

    private void TriggerZicZacBall(ref HashSet<HexCell> ballGroupCells, HexCell firstCell, HexCell ballCell)
    {
        HexCell curCell = ballCell;

        while (curCell != null && curCell.GetNeighbor(HexDirection.NW) != null)
        {
            curCell = curCell.GetNeighbor(HexDirection.NW);

            if (curCell.GetBall() == null) continue;

            if (!curCell.GetBall().ballType.Equals(BallType.Normal))
                TriggerSpecialBall(ref ballGroupCells, firstCell, curCell);

            if (!ballGroupCells.Contains(curCell))
                ballGroupCells.Add(curCell);
        }

        curCell = ballCell;

        while (curCell != null && curCell.GetNeighbor(HexDirection.NE) != null)
        {
            curCell = curCell.GetNeighbor(HexDirection.NE);

            if (curCell.GetBall() == null) continue;

            if (!curCell.GetBall().ballType.Equals(BallType.Normal))
                TriggerSpecialBall(ref ballGroupCells, firstCell, curCell);

            if (!ballGroupCells.Contains(curCell))
                ballGroupCells.Add(curCell);
        }
    }

    #endregion

    public bool IsHexCellVisibleByCamera(HexCell curCell)
    {
        Vector3 pos = _camera.WorldToViewportPoint(curCell.transform.position);

        if (pos.x >= 0
            && pos.x <= 1
            && pos.y >= 0
            && pos.y <= 1) return true;

        return false;
    }

    public BallColor GetRandomBallColor()
    {
        if (BallColorsOnFields.Count > 0)
            return BallColorsOnFields[UnityEngine.Random.Range(0, BallColorsOnFields.Count)];

        return BallColor.Blue;
    }

    private void CheckCountBallsOnField()
    {
        //Debug.Log(_ballsOnField.Count);

        if (_ballsOnField.Count <= 0)
        {
            int thisLevel = int.Parse(LevelDataHolder.LevelData.LevelId.Substring(4));
            bool isCurrentLevel = (PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 1) == thisLevel);

            Debug.Log(thisLevel + " " + isCurrentLevel);


            AchievementEvent.OnChangeAchievementData.Invoke(AchievementType.PassLevel, PlayerPrefs.GetInt(PlayerPrefsConst.CURRENT_LEVEL, 1) - 1);
            DailyMissionEvent.OnUpdateMissionData.Invoke(DailyMissionType.PassLevel, 1);


            GameplayEvent.OnNoBallOnField.Invoke();

            //GameplayEvent.OnGameWin.Invoke();
        }
        else GameplayEvent.OnCountBallsOnFieldChecked.Invoke();
    }
    #region Helper Ball Logic
    private void DoHelperAction(Ball ball)
    {
        if (ball.ballType != BallType.Firework)
        {
            SetBallAtField(ball, ball.targetCell);
        }
        else
        {
            if (ball.isCollided)
            {
                return;
            }
            ball.isCollided = true;
        }

        ball.BallSprite.sortingLayerName = "HelperBall";

        GameplayEvent.OnActiveBallSetOnField.Invoke(ball);

        switch (ball.ballType)
        {
            case BallType.Bomb:
                {
                    ExplodeNeiborBall(ball);
                    break;
                }
            case BallType.Ziczac:
                {
                    ExploidZiczac(ball);
                    break;
                }
            case BallType.Rainbow:
                {
                    ExploidRainbowBall(ball);
                    break;
                }
            case BallType.Firework:
                {
                    ExploidFirework(ball);
                    break;
                }
        }
    }

    private void ExploidFirework(Ball ball)
    {

        List<HexCell> toExploidList = new List<HexCell>();

        toExploidList = HelperManager.FindMissileBallTarget();

        if (toExploidList.Count <= 0) return;

        FireworkBall firework = ball.gameObject.GetComponent<FireworkBall>();
        firework.isCollided = true;

        toExploidList = SortedBallList(toExploidList);

        int count = 0;

        foreach (var cell in toExploidList)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(cell.transform.position);

            if (viewPos.y > 1)
            {
                continue;
            }

            if (cell.GetBall())
            {
                Ball destroyedBall = cell.GetBall();
                RemoveBallFromField(cell);
                destroyedBall.DestroyBall(ScoreType.Destroy, BallType.Firework);
                count++;
                //cell.IsHighLight = true;
            }
        }

        if (count == 0)
        {
            ScoreController.Instance.ResetStack();
            return;
        }

        ScoreController.Instance.IncreaseStack();

        ScoreController.Instance.AddScore(count + 1, ScoreType.Destroy);

    }

    private void ExploidRainbowBall(Ball ball)
    {
        //HexCell cell = ball.targetCell;
        //TryDestroyBallGroup(cell);

        TryDestroyRainbowBall(ball.targetCell, ball);
    }

    private void TryDestroyRainbowBall(HexCell targetCell, Ball ball)
    {
        canDestroyBall = false;

        //List<HexCell> ballGroupCells = HelperManager.FindRainbowBallTarget(targetCell);

        List<HexCell> listCells = new List<HexCell>();

        foreach (var neighbor in targetCell.Neighbors)
        {
            List<HexCell> tempList = GetBallGroupCells(neighbor, neighbor, 2);
            if (tempList != null && tempList.Count > 0)
            {
                listCells.AddRange(tempList);
            }
        }

        List<HexCell> ballGroupCells = listCells
            .GroupBy(cell => cell.GetBall().color)
            .Where(group => group.Count() >= 2)
            .SelectMany(group => group)
            .ToList();

        foreach (var neighbor in targetCell.Neighbors)
        {
            if (!ballGroupCells.Contains(neighbor))
            {
                ballGroupCells.Add(neighbor);
            }
        }

        if (ballGroupCells == null)
        {
            Debug.LogWarning("Can't get ball group cells!");
            return;
        }

        //if (!canDestroyBall)
        //{
        //    GameplayEvent.OnAllFieldActionsEnd.Invoke();
        //    GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);
        //    return;
        //}

        if (ballGroupCells.Count <= 0) return;
        //Remove list ball
        foreach (HexCell cell in ballGroupCells)
        {
            if (cell == null) continue;
            if (cell.GetBall())
            {
                Ball destroyedBall = cell.GetBall();
                RemoveBallFromField(cell);
                destroyedBall.DestroyBall(ScoreType.Destroy, BallType.Rainbow);
                //cell.IsHighLight = true;
            }
        }
        ScoreController.Instance.IncreaseStack();

        //Score calculate
        ScoreController.Instance.AddScore(ballGroupCells.Count + 1, ScoreType.Destroy);

        RemoveBallFromField(targetCell);
        ball.DestroyBall(ScoreType.Destroy, BallType.Rainbow);


        HelperEvent.OnAllHelperTargetDestroyed.Invoke();

        GameplayEvent.OnBallGroupDestroyed.Invoke(_ballsOnField);

        GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);
    }

    private void ExploidZiczac(Ball ball)
    {
        List<HexCell> toExploidList = new List<HexCell>();

        toExploidList = HelperManager.FindZiczacBallTarget(ball.targetCell);

        foreach (HexCell cell in toExploidList)
        {
            if (cell.GetBall())
            {
                Ball destroyedBall = cell.GetBall();
                RemoveBallFromField(cell);
                destroyedBall.DestroyBall(ScoreType.Destroy, BallType.Ziczac);
            }
            //cell.IsHighLight = true;
        }
        ScoreController.Instance.IncreaseStack();
        //Score calculate
        ScoreController.Instance.AddScore(toExploidList.Count + 1, ScoreType.Destroy);

        RemoveBallFromField(ball.targetCell);
        ball.DestroyBall(ScoreType.Destroy, BallType.Ziczac);

        HelperEvent.OnAllHelperTargetDestroyed.Invoke();
        //GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);
    }

    private void ExplodeNeiborBall(Ball ball)
    {
        List<HexCell> toExploidList = new List<HexCell>();
        toExploidList = HelperManager.FindBombBallTarget(ball.targetCell);

        Debug.Log("Count: " + toExploidList.Count);

        foreach (HexCell cell in toExploidList)
        {
            if (cell.GetBall())
            {
                Ball destroyedBall = cell.GetBall();
                RemoveBallFromField(cell);
                destroyedBall.DestroyBall(ScoreType.Destroy, BallType.Bomb);
            }
            //cell.IsHighLight = true;
        }

        ScoreController.Instance.IncreaseStack();

        //Score calculate
        ScoreController.Instance.AddScore(toExploidList.Count + 1, ScoreType.Destroy);

        //Remove helper
        RemoveBallFromField(ball.targetCell);
        ball.DestroyBall(ScoreType.Destroy, BallType.Bomb);

        HelperEvent.OnAllHelperTargetDestroyed.Invoke();

        //GameplayEvent.OnGameFieldChanged.Invoke(_ballsOnField);
    }

    #endregion

    private void ResetBossData()
    {
        _ballsDestroyedInTurn.Clear();
    }

    private void AddToDestroyedList(BallColor ballColor, int number)
    {
        if (_ballsDestroyedInTurn.ContainsKey(ballColor))
        {
            _ballsDestroyedInTurn[ballColor] += number;
        }
        else
        {
            _ballsDestroyedInTurn.Add(ballColor, number);
        }
    }
}
