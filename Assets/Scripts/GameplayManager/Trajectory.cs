using System;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private LevelField _levelField;
    [SerializeField] private ShootingLine _trajectoryLinePrefab;

    private Gradient originLineColor;

    [Min(1)]

    [SerializeField] private int _linesCount = 1;
    private int originLinesCount;

    public float defaultForceMagnitude = 30f;
    private readonly List<ShootingLine> _trajectoryLines = new List<ShootingLine>();
    public bool canShowLine;

    private Vector2 _lastTrajectoryPoint;

    private HexCell lastCell;

    [SerializeField] private Ball tempBall;

    [Header("Wall")]
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject bottomWall;


    #region Cache components
    private CameraSettings _cameraSettings;
    public CameraSettings CameraSettings
    {
        get
        {
            if (_cameraSettings == null)
                _cameraSettings = Camera.main.GetComponent<CameraSettings>();
            return _cameraSettings;
        }
    }
    #endregion

    private void Reset()
    {
        canShowLine = false;
    }
    private void Awake()
    {
        if (_trajectoryLinePrefab == null)
        {
            Debug.LogError("Trajectory prefab is not set!");
            return;
        }

        originLinesCount = _linesCount;
        originLineColor = _trajectoryLinePrefab.TrajectoryLineRenderer.colorGradient;

        GameplayEvent.OnActiveBallSetOnField.AddListener(HideTrajectory);
        GameplayEvent.OnActiveBallDestroyed.AddListener(HideTrajectory);
        HelperEvent.OnHelperActivated.AddListener(ModifyTrajectory);
        HelperEvent.OnHelperDeactivated.AddListener(ReturnTrajectory);
        GameplayEvent.OnActiveBallChanged.AddListener(ChangeTrajectoryLineColor);


        Transform trajectoryLines = new GameObject("TrajectoryLines").transform;
        trajectoryLines.SetParent(transform);

        for (int i = 0; i < _linesCount; i++)
        {
            ShootingLine lineRenderer = Instantiate(_trajectoryLinePrefab, trajectoryLines);
            _trajectoryLines.Add(lineRenderer);
        }
    }

    private void OnDestroy()
    {
        GameplayEvent.OnActiveBallSetOnField.RemoveListener(HideTrajectory);
        GameplayEvent.OnActiveBallDestroyed.RemoveListener(HideTrajectory);
        GameplayEvent.OnActiveBallChanged.RemoveListener(ChangeTrajectoryLineColor);
    }

    public void ShowTrajectory(Ball activeBall)
    {
        if (canShowLine)
        {
            float time = 0f;
            bool isTrajectoryEnd = false;
            List<Vector3> points = new List<Vector3>();

            //Debug.LogWarning("ShowTrajectory");
            Vector3 currentPoint = activeBall.rb2d.position;

            Vector2 forceDirection = activeBall.transform.up;
            Vector2 force = forceDirection * defaultForceMagnitude;

            float width = leftWall.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            float height = topWall.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            float ballRadius = tempBall.BallSprite.bounds.size.x / 4;

            foreach (ShootingLine trajectoryLine in _trajectoryLines)
            {
                //trajectoryLine.TrajectoryLineRenderer.startColor = activeBall.BallSpriteRenderer.color;
                //trajectoryLine.TrajectoryLineRenderer.endColor = activeBall.BallSpriteRenderer.color;
                trajectoryLine.UpdateLineScale();
                points.Add(currentPoint);

                for (int i = 0; i < trajectoryLine.MaxLinePointCount; i++)
                {
                    if (isTrajectoryEnd)
                        break;

                    currentPoint = (Vector2)points[i] + force * Time.fixedDeltaTime;

                    HexCell pointCell = _levelField.FieldGrid.GetCellFromPosition(currentPoint);

                    #region Old cell finding
                    //if (pointCell != null && pointCell.GetBall() != null)
                    //{
                    //    isTrajectoryEnd = true;
                    //    _lastTrajectoryPoint = currentPoint;
                    //    if (lastCell) lastCell.IsHighLight = false;

                    //    HexCell destinationCell = GetDestinationCell(pointCell);
                    //    if (destinationCell)
                    //    {
                    //        activeBall.targetCell = destinationCell;
                    //        activeBall.targetCell.IsHighLight = true;
                    //    }

                    //    lastCell = activeBall.targetCell;
                    //    break;
                    //}
                    #endregion

                    if (pointCell != null)
                    {
                        if (pointCell.GetBall() != null)
                        {
                            isTrajectoryEnd = true;
                            _lastTrajectoryPoint = currentPoint;
                            //Debug.Log("met ball cell");
                            //Highlight breakable ball;
                            HelperManager.TryHighLightBall(lastCell, true);
                            break;
                        }

                        //if (lastCell) lastCell.IsHighLight = false;
                        lastCell = pointCell;
                        //lastCell.IsHighLight = true;
                        activeBall.targetCell = lastCell;
                    }

                    if (currentPoint.x >= (rightWall.transform.position.x - width - ballRadius))
                    {
                        Vector2 minLinePoint = new Vector2(rightWall.transform.position.x - width - ballRadius, rightWall.transform.position.y * -1);
                        currentPoint = Intersection(points[i], currentPoint, minLinePoint, new Vector2(rightWall.transform.position.x - width - ballRadius, rightWall.transform.position.y));
                        force = Vector2.Reflect(force, Vector2.left);
                        points.Add(currentPoint);
                        //Debug.Log("> right wall");
                        break;
                    }

                    if (currentPoint.x <= leftWall.transform.position.x + width + ballRadius)
                    {
                        Vector2 minLinePoint = new Vector2(leftWall.transform.position.x + width + ballRadius, leftWall.transform.position.y * -1);
                        currentPoint = Intersection(points[i], currentPoint, minLinePoint, new Vector2(leftWall.transform.position.x + width + ballRadius, leftWall.transform.position.y));
                        force = Vector2.Reflect(force, Vector2.right);
                        points.Add(currentPoint);
                        //Debug.Log("> left wall");
                        break;
                    }

                    if (currentPoint.y >= topWall.transform.position.y - height)
                    {
                        isTrajectoryEnd = true;
                        //Debug.Log("> top wall");
                        break;
                    }

                    points.Add(currentPoint);

                    time += Time.fixedDeltaTime;
                }

                trajectoryLine.GenerateTrajectoryLine(points);
                points.Clear();
            }
        }
    }

    public void HideTrajectory(Ball activeBall)
    {
        if (lastCell != null)
        {
            HelperManager.TryHighLightBall(lastCell, false);
        }

        foreach (ShootingLine trajectoryLine in _trajectoryLines)
        {
            trajectoryLine.ClearTrajectoryLinePoints();
        }
    }

    private Vector2 Intersection(Vector2 linePointMin1, Vector2 linePointMax1, Vector2 linePointMin2, Vector2 linePointMax2)
    {
        float p = linePointMax1.x - linePointMin1.x, p1 = linePointMax2.x - linePointMin2.x;
        float q = linePointMax1.y - linePointMin1.y, q1 = linePointMax2.y - linePointMin2.y;

        float x = (linePointMin1.x * q * p1 - linePointMin2.x * q1 * p - linePointMin1.y * p * p1 + linePointMin2.y * p * p1) / (q * p1 - q1 * p);
        float y = (linePointMin1.y * p * q1 - linePointMin2.y * p1 * q - linePointMin1.x * q * q1 + linePointMin2.x * q * q1) / (p * q1 - p1 * q);

        return new Vector2(x, y);
    }

    private void ModifyTrajectory(HelperBtn helper)
    {
        if (helper.Type == BallType.Firework)
        {
            //Debug.Log("Missile in trajectory on");
            //_linesCount = 1;
        }
    }

    private void ReturnTrajectory()
    {
        _linesCount = originLinesCount;
    }

    private void ChangeTrajectoryLineColor(Ball ball)
    { 

        if (ball.hexColor == null || ball.hexColor == Color.white)
        {
            
            //Debug.Log("===============Change original line color");
            ChangeLinesColor(originLineColor);
            return;
        }

        //Debug.Log("===============Change new line color");
        ChangeLinesColor(ColorCtrl.ConvertColorToGradient(ball.hexColor));
    }

    private void ChangeLinesColor(Gradient color)
    {
        foreach (var line in _trajectoryLines)
        {
            line.TrajectoryLineRenderer.colorGradient = color;
        }
    }
}
