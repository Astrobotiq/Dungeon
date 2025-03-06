using UnityEngine;

public class ParabolicLineController : LineController
{
    public float peakHeight = 1.5f; // Parabolün en yüksek noktası
    public int segmentCount = 10; 
    public override void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3[] points = new Vector3[segmentCount];

// X ve Z eksenlerinde başlangıç ve bitiş noktalarını belirle
        float x1 = startPoint.x;
        float x2 = endPoint.x;
        float z1 = startPoint.z;
        float z2 = endPoint.z;

// Zirve noktasını orta noktada belirle
        float peakX = (x1 + x2) / 2;
        float peakZ = (z1 + z2) / 2;

// Parabol katsayısı hesaplama (Yüksekliği ayarlamak için)
        float a = -4 * peakHeight / ((x1 - x2) * (x1 - x2) + (z1 - z2) * (z1 - z2));

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
    
            // X ve Z ekseninde doğrusal interpolasyon
            float x = Mathf.Lerp(x1, x2, t);
            float z = Mathf.Lerp(z1, z2, t);

            // Y ekseni için parabolik eğri
            float y = a * ((x - x1) * (x - x2) + (z - z1) * (z - z2)) + peakHeight;

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