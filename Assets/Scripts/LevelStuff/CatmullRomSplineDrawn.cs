using System.Collections.Generic;
using UnityEngine;
public class CatmullRomSplineDrawn
{
    private Stack<Vector3> finalPoints = new Stack<Vector3>();
    public Vector3[] MakePath(Vector3[] points)
    {
        finalPoints.Clear();
        if (points.Length >= 4)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                if (0 == i || points.Length - 2 == i || points.Length - 1 == i)
                    continue;
                DisplayCatmullRomSpline(i, points);
            }
            return finalPoints.ToArray();
        }
        else return null;
    }
    private void DisplayCatmullRomSpline(int pos, Vector3[] points)
    {
        Vector3 p0 = points[ClampListPos(pos - 1, points)];
        Vector3 p1 = points[pos];
        Vector3 p2 = points[ClampListPos(pos + 1, points)];
        Vector3 p3 = points[ClampListPos(pos + 2, points)];
        finalPoints.Push(GetCatmullRomPosition(0.2f, p0, p1, p2, p3));
        finalPoints.Push(GetCatmullRomPosition(0.4f, p0, p1, p2, p3));
        finalPoints.Push(GetCatmullRomPosition(0.6f, p0, p1, p2, p3));
        finalPoints.Push(GetCatmullRomPosition(0.8f, p0, p1, p2, p3));
        finalPoints.Push(GetCatmullRomPosition(1.0f, p0, p1, p2, p3));
    }
    private int ClampListPos(int pos, Vector3[] points)
    {
        if (pos < 0)
            return points.Length - 1;
        if (pos < points.Length)
            return pos;
        if (pos == points.Length)
            return 0;
        return 1;
    }
    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return 0.5f * (p1 + p1 + (p2 - p0) * t + (p0 + p0 - 5.0f * p1 + 4.0f * p2 - p3) * t * t + (3.0f * p1 - p0 - 3.0f * p2 + p3) * t * t * t);
    }
}