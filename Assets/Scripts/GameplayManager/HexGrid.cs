using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private HexCell _cellPrefab;
    [SerializeField] private GameObject TopWall;
    [Header("Debug")]
    [SerializeField] private bool _debugMode = true;
    [SerializeField] private Text _cellLabelPrefab;

    public HexCell[] _cells;
    private Canvas _gridCanvas;
    public int Width
    {
        get { return _width; }
    }
    public int Height
    {
        get { return _height; }
    }

    public HexCell[] Cells
    {
        get { return _cells; }
    }

    private void Awake()
    {
        LoadComponent();
        CreateHexGrid();
    }

    private void CreateHexGrid()
    {
        //Define and reset cell arrays
        _cells = new HexCell[_width * _height];

        int cellCount = 0;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                CreateCell(x, y, cellCount);
                cellCount++;
            }
        }
    }

    public void AddCellRowToTopGrid(int height)
    {
        //Create new array to store new cells
        HexCell[] combinedGrid = new HexCell[_width * (_height + height)];

        //Copy all old cells to new array
        Array.Copy(_cells, combinedGrid, _cells.Length);

        //Get length of old array
        int cellCount = _cells.Length;

        //Debug.Log("Old cell length = " + _cells.Length);
        //Set new array combined array to grid
        _cells = combinedGrid;

        //Debug.Log("New cell length = " + _cells.Length);

        for (int y = _height; y < (_height + height); y++)
        {
            for (int x = 0; x < _width; x++)
            {
                CreateCell(x, y, cellCount);
                cellCount++;
            }
        }

        _height = _height + height;

    }

    private void LoadComponent()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        TopWall = GameObject.Find("WallTop");
    }

    public HexCell GetCellFromPosition(Vector2 position)
    {
        //World space to local space
        position = this.transform.InverseTransformPoint(position.x, position.y, 0);

        HexCoordinates cellCoordinates = HexCoordinates.FromPosition(position);
        return _cells.FirstOrDefault(c => c.Coordinates.X == cellCoordinates.X && c.Coordinates.Y == cellCoordinates.Y);
    }

    private void CreateCell(int x, int y, int count)
    {
        Vector2 position = new Vector2((x + y * 0.5f - y / 2) * (HexMetrics.INNER_RADIUS * 2f), y * (HexMetrics.OUTER_RADIUS * 1.5f));

        HexCell cell = Instantiate(_cellPrefab);

        _cells[count] = cell;

        cell.transform.SetParent(this.transform, false);
        cell.transform.localPosition = position;
        cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, y);

        //Set neighbor for cells
        SetCellNeighbors(x, y, count, cell);

        if (_debugMode)
        {
            Text label = Instantiate(_cellLabelPrefab);
            label.rectTransform.SetParent(_gridCanvas.transform, false);
            label.rectTransform.anchoredPosition = position;
            label.text = cell.Coordinates.ToStringOnSeparateLine();
        }

    }

    private void SetCellNeighbors(int x, int y, int count, HexCell cell)
    {
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, _cells[count - 1]);
        }

        if (y > 0)
        {
            //Check if y is even or not
            if ((y & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, _cells[count - _width]);
                if (x > 0)
                    cell.SetNeighbor(HexDirection.SW, _cells[count - _width - 1]);
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, _cells[count - _width]);
                if (x < _width - 1)
                    cell.SetNeighbor(HexDirection.SE, _cells[count - _width + 1]);
            }
        }
    }
}
