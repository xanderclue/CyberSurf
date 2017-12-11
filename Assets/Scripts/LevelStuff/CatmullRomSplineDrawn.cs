using System.Collections.Generic;
using UnityEngine;
public class CatmullRomSplineDrawn
{
    public Vector3[] MakePath(Vector3[] points)
    {
        Stack<Vector3> finalPoints = new Stack<Vector3>();
        Vector3 a, b, c;
        for (int i = 1; i < points.Length - 2; ++i)
        {
            a = (points[i] - points[i + 1]) * 3.0f - points[i - 1] + points[i + 2];
            b = points[i - 1] + points[i - 1] - points[i] * 5.0f + points[i + 1] * 4.0f - points[i + 2];
            c = points[i + 1] - points[i - 1];
            finalPoints.Push(a * 0.004f + b * 0.02f + c * 0.1f + points[i]);
            finalPoints.Push(a * 0.032f + b * 0.08f + c * 0.2f + points[i]);
            finalPoints.Push(a * 0.108f + b * 0.18f + c * 0.3f + points[i]);
            finalPoints.Push(a * 0.256f + b * 0.32f + c * 0.4f + points[i]);
            finalPoints.Push((a + b + c) * 0.5f + points[i]);
        }
        return finalPoints.ToArray();
    }
}