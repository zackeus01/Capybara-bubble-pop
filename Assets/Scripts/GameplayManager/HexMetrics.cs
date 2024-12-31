using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    //Distance from center of hexagon to vertex
    public const float OUTER_RADIUS = 1.2f;
    //Distance from center of hexagon to edge
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f;
}
