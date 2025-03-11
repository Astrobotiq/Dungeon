using Unity.Mathematics;
using UnityEngine;

public class StraightLineController : LineController
{
    public override void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[2]{startPoint,endPoint};
        
        TargetTexture.SetActive(true);
        TargetTexture.transform.position = new Vector3(endPoint.x, 0.5f, endPoint.z);

        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }
    
    public override void RemoveLine()
    {
        TargetTexture.SetActive(false);
        _lineRenderer.positionCount = 0;
    }
}