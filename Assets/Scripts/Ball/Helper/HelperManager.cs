using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HelperManager : MonoBehaviour
{
    private static BallType helperType = BallType.Normal;

    private static List<HexCell> highlightedList = new List<HexCell>();

    private static Ball currentHelperBall;

    private static Vector3 originPos;

    private void Start()
    {
        HelperEvent.OnHelperActivated.AddListener(SetHelperType);
        HelperEvent.OnHelperDeactivated.AddListener(RemoveHelperType);
        HelperEvent.OnHelperDeactivated.AddListener(RemoveCurrentHelperBall);
        HelperEvent.OnHelperBallGenerated.AddListener(SetCurrentHelperBall);
    }

    private void OnDisable()
    {
        HelperEvent.OnHelperActivated.RemoveListener(SetHelperType);
        HelperEvent.OnHelperDeactivated.RemoveListener(RemoveHelperType);
        HelperEvent.OnHelperDeactivated.RemoveListener(RemoveCurrentHelperBall);
        HelperEvent.OnHelperBallGenerated.RemoveListener(SetCurrentHelperBall);
    }

    private void RemoveCurrentHelperBall()
    {
        currentHelperBall = null;
    }

    private void SetCurrentHelperBall(Ball ball)
    {
        currentHelperBall = ball;
        originPos = ball.transform.position;
        this.transform.position = originPos;
    }

    private void SetHelperType(HelperBtn btn)
    {
        helperType = btn.Type;
    }

    private void RemoveHelperType()
    {
        helperType = BallType.Normal;
    }
    public static List<HexCell> FindBombBallTarget(HexCell targetCell)
    {
        List<HexCell> cellList = new List<HexCell>();

        foreach (var neibor in targetCell.Neighbors)
        {
            if (!neibor) continue;
            if (!cellList.Contains(neibor) && neibor.GetBall())
            {
                cellList.Add(neibor);
            }
            foreach (var cell in neibor.Neighbors)
            {
                if (!cell) continue;
                if (!cellList.Contains(cell) && cell.GetBall() != null)
                {
                    cellList.Add(cell);
                }
            }
        }
        cellList.Remove(targetCell);
        return cellList;
    }

    public static void TryHighLightBall(HexCell targetCell, bool isTrajectoryEnable)
    {
        if (highlightedList.Count != 0)
        {
            UnHighLightBall(highlightedList);
            highlightedList.Clear();
        }

        if (!isTrajectoryEnable)
        {
            return;
        }

        if (helperType == BallType.Normal)
        {
            return;
        }

        List<HexCell> toHighLightList = new List<HexCell>();

        switch (helperType)
        {
            case BallType.Bomb:
                {
                    toHighLightList = FindBombBallTarget(targetCell);
                    break;
                }
            case BallType.Ziczac:
                {
                    toHighLightList = FindZiczacBallTarget(targetCell);
                    break;
                }
            case BallType.Rainbow:
                {
                    toHighLightList = FindRainbowBallTarget(targetCell);
                    break;
                }
            case BallType.Firework:
                {
                    toHighLightList = FindMissileBallTarget();
                    break;
                }
            default:
                {
                    break;
                }
        }

        highlightedList = HighLightBall(toHighLightList);
    }
    static Vector2 pos;
    static Vector2 size;
    static float angle;

    void OnDrawGizmos()
    {
        // Save the current gizmos matrix
        Gizmos.matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0, 0, angle), Vector3.one);

        // Set the color for the gizmo (optional)
        Gizmos.color = Color.blue;

        // Draw a wireframe box at the position with the given size and angle
        Gizmos.DrawWireCube(Vector3.zero, size);

        // Reset the gizmos matrix to avoid affecting other gizmos
        Gizmos.matrix = Matrix4x4.identity;
    }
    public static List<HexCell> FindMissileBallTarget()
    {
        List<HexCell> cellList = new List<HexCell>();

        if (currentHelperBall == null)
        {
            return cellList;
        }

        if (helperType != BallType.Firework)
        {
            return cellList;
        }

        //GameObject hitbox = currentHelperBall.gameObject.GetComponent<FireworkBall>().Hitbox;

        pos = currentHelperBall.transform.position + currentHelperBall.transform.up.normalized * 18f;
        angle = currentHelperBall.gameObject.transform.rotation.eulerAngles.z;
        size = new Vector2(2f, 36f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, size, angle);

        //List<HexCell> cellCollides = new List<HexCell>();

        foreach (var item in colliders)
        {
            if (item.CompareTag("Cell"))
            {
                cellList.Add(item.gameObject.GetComponent<HexCell>());
            }
        }

        return cellList;
    }

    public static List<HexCell> FindZiczacBallTarget(HexCell targetCell)
    {
        List<HexCell> cellList = new List<HexCell>();
        GetNorthNeibor(targetCell, cellList);
        return cellList;
    }

    private static void GetNorthNeibor(HexCell targetCell, List<HexCell> toExploidList)
    {
        HexCell cellNW = targetCell.GetNeighbor(HexDirection.NW);
        HexCell cellNE = targetCell.GetNeighbor(HexDirection.NE);

        if (cellNW != null)
        {
            toExploidList.Add(cellNW);
            int countNW = 1;
            GetNorthWestNeibor(cellNW, toExploidList, countNW);
        }

        if (cellNE != null)
        {
            toExploidList.Add(cellNE);
            int countNE = 1;
            GetNorthEastNeibor(cellNE, toExploidList, countNE);
        }
    }

    private static void GetNorthEastNeibor(HexCell cellNE, List<HexCell> toExploidList, int countNE)
    {
        if (countNE > 5)
        {
            return;
        }
        HexCell ne = cellNE.GetNeighbor(HexDirection.NE);
        if (ne != null)
        {
            toExploidList.Add(cellNE);
            countNE++;
            GetNorthEastNeibor(ne, toExploidList, countNE);
        }
    }

    private static void GetNorthWestNeibor(HexCell cellNW, List<HexCell> toExploidList, int countNW)
    {
        if (countNW > 5)
        {
            return;
        }
        HexCell nw = cellNW.GetNeighbor(HexDirection.NW);
        if (nw != null)
        {
            toExploidList.Add(cellNW);
            countNW++;
            GetNorthWestNeibor(nw, toExploidList, countNW);
        }
    }

    private static List<HexCell> HighLightBall(List<HexCell> toHighLightList)
    {
        List<HexCell> highlightedBall = new List<HexCell>();

        foreach (var cell in toHighLightList)
        {
            if (cell.GetBall() != null)
            {
                cell.GetBall().IsHighLight = true;
                highlightedBall.Add(cell);
            }
        }
        return highlightedBall;
    }

    private static void UnHighLightBall(List<HexCell> toUnHighLight)
    {
        foreach (var cell in toUnHighLight)
        {
            if (cell.GetBall() != null)
            {
                cell.GetBall().IsHighLight = false;
            }
        }
    }

    public static List<HexCell> FindRainbowBallTarget(HexCell targetCell)
    {
        List<HexCell> listCells = new List<HexCell>();
        
        foreach (var cell in targetCell.Neighbors)
        {
            if (!cell) continue;
            Ball ball = cell.GetBall();
            if (!ball) continue;
            if (!listCells.Contains(cell))
            {
                listCells.Add(cell);
                GetCellWithSameColor(cell, listCells);
            }
        }
        return listCells;
    }

    private static void GetCellWithSameColor(HexCell targetCell, List<HexCell> listCells)
    {
        foreach (var cell in targetCell.Neighbors)
        {
            if (!cell) continue;

            Ball ball = cell.GetBall();
            if (!ball) continue;

            if (ball.ballType != BallType.Normal) continue;
            if (ball.color != targetCell.GetBall().color) continue;

            if (!listCells.Contains(cell))
            {
                listCells.Add(cell);
                GetCellWithSameColor(cell, listCells);
            }
        }
    }
}
