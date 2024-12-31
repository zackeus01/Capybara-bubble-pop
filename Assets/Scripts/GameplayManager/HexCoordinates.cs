using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Z
    {
        get { return -X - Y; }
    }

    public HexCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - (y / 2), y);
    }

    public static HexCoordinates FromPosition(Vector2 position)
    {
        float x = position.x / (HexMetrics.INNER_RADIUS * 2f);
        float z = -x;


        float offset = position.y / (HexMetrics.OUTER_RADIUS * 3f);
        x -= offset;
        z -= offset;


        int rX = Mathf.RoundToInt(x);
        int rZ = Mathf.RoundToInt(z);
        int rY = Mathf.RoundToInt(-x - z);

        if (rX + rY + rZ != 0)
        {
            float dX = Mathf.Abs(x - rX);
            float dZ = Mathf.Abs(z - rZ);
            float dY = Mathf.Abs(-x - z - rY);

            if (dX > dZ && dX > dY)
                rX = -rZ - rY;
            else if (dY > dZ)
                rY = -rX - rZ;
        }

        return new HexCoordinates(rX, rY);

    }

    public override string ToString()
    {
        return "(" + X.ToString() + ";" + Y.ToString() + ")";
    }

    public string ToStringOnSeparateLine()
    {
        return "X(" + X.ToString() + ")" + "\n" + "Y(" + Y.ToString() + ")";
    }
}
