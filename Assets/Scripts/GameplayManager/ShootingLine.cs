using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLine : MonoBehaviour
{
    [Min(2)]
    [SerializeField] private int _maxLinePointCount = 2;

    public int MaxLinePointCount { get => _maxLinePointCount; }

    #region Cache Components
    private LineRenderer _trajectoryLineRenderer;
    public LineRenderer TrajectoryLineRenderer
    {
        get
        {
            if (_trajectoryLineRenderer == null)
                _trajectoryLineRenderer = GetComponent<LineRenderer>();
            return _trajectoryLineRenderer;
        }
    }
    #endregion

    public void GenerateTrajectoryLine(List<Vector3> points)
    {
        TrajectoryLineRenderer.positionCount = points.Count;
        TrajectoryLineRenderer.SetPositions(points.ToArray());
    }

    public void ClearTrajectoryLinePoints()
    {
        TrajectoryLineRenderer.positionCount = 0;
    }

    public void UpdateLineScale()
    {
        TrajectoryLineRenderer.material.mainTextureScale = new Vector2(1f / TrajectoryLineRenderer.startWidth, 1f);
    }
}
