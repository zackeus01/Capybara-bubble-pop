using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField] private HexCoordinates _coordinates;
    [SerializeField] private HexCell[] _neighbors = new HexCell[6];
    [SerializeField] public Ball _ball;

    private bool _isHighLight;
    [SerializeField]
    public SpriteRenderer _sr;

    public bool IsHighLight
    {
        get
        {
            return _isHighLight;
        }
        set
        {
            _isHighLight = value;
            _sr.enabled = value;
        }
    }

    private void Reset()
    {
        _sr = this.GetComponent<SpriteRenderer>();
    }

    public HexCoordinates Coordinates
    {
        get
        {
            return _coordinates;
        }

        set
        {
            _coordinates = value;
        }
    }

    public HexCell[] Neighbors
    {
        get { return _neighbors; }
    }

    public HexCell GetNeighbor(HexDirection hexDirection)
    {
        return _neighbors[(int)hexDirection];
    }

    public void SetNeighbor(HexDirection hexDirection, HexCell cell)
    {
        _neighbors[(int)hexDirection] = cell;
        cell._neighbors[(int)hexDirection.Opposite()] = this;
    }

    public void SetBall(Ball ball)
    {
        if (_ball != null)
        {
            Debug.LogError("Ball has already been set and cannot be changed.");
            return;
        }
        _ball = ball;
    }

    public void RemoveBall()
    {
        _ball = null;
    }

    public Ball GetBall()
    {
        return _ball;
    }
}
