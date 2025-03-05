using UnityEngine;

public class StraightLineController : LineController
{
    public override void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[2]{startPoint,endPoint};
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }
    
    public override void RemoveLine()
    {
        _lineRenderer.positionCount = 0;
    }
}