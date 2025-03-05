using System;
using UnityEngine;

public abstract class LineController : MonoBehaviour
{
    protected LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public abstract void DrawLine(Vector3 startPoint, Vector3 endPoint);

    public abstract void RemoveLine();
}