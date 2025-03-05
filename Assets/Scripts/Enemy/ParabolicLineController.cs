using UnityEngine;

public class ParabolicLineController : LineController
{
    public float peakHeight = 1.5f; // Parabolün en yüksek noktası
    public int segmentCount = 10; 
    public override void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[segmentCount];
        
        // X ekseninde başlangıç ve bitiş noktalarını belirle
        float x1 = startPoint.x;
        float x2 = endPoint.x;
        float peakX = (x1 + x2) / 2; // Zirve X ekseninde ortada olacak
        
        // Parabol katsayısı hesaplama (geçici katsayı, yüksekliği kontrol etmek için)
        float a = -4 * peakHeight / ((x1 - x2) * (x1 - x2));

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            float x = Mathf.Lerp(x1, x2, t);
            float y = a * (x - x1) * (x - x2) + peakHeight;
            float z = Mathf.Lerp(startPoint.z, endPoint.z, t);

            points[i] = new Vector3(x, y, z);
        }

        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    public override void RemoveLine()
    {
        _lineRenderer.positionCount = 0;
    }
}