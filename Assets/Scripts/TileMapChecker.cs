using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapChecker : MonoBehaviour
{
    
    public Tilemap tilemap; // Tilemap referansı

    void Start()
    {
        // Tüm hücreleri dolaş ve pozisyonları yazdır
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                // Grid pozisyonunu dünya pozisyonuna çevir
                Vector3 worldPosition = tilemap.CellToWorld(pos);
                Debug.Log($"Tile at {pos} has world position {worldPosition}");
            }
        }
    }
}
