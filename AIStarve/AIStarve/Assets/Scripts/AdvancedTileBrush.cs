using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 高级Tile笔刷，支持多种绘制模式
/// </summary>
public class AdvancedTileBrush : ScriptableObject
{
    public enum BrushMode
    {
        Single,
        Box,
        Circle,
        Line
    }

    [Header("Brush Settings")]
    public BrushMode mode = BrushMode.Single;
    public Vector2Int size = Vector2Int.one;
    public TileBase tile;
    
    [Header("Brush Visualization")]
    public Color brushColor = new Color(1f, 1f, 1f, 0.5f);
    
    /// <summary>
    /// 在指定位置绘制Tile
    /// </summary>
    public void Paint(Tilemap tilemap, Vector3Int position)
    {
        switch (mode)
        {
            case BrushMode.Single:
                PaintSingle(tilemap, position);
                break;
            case BrushMode.Box:
                PaintBox(tilemap, position);
                break;
            case BrushMode.Circle:
                PaintCircle(tilemap, position);
                break;
            case BrushMode.Line:
                // Line模式需要两个点，这里简化处理
                PaintSingle(tilemap, position);
                break;
        }
    }
    
    /// <summary>
    /// 绘制单个Tile
    /// </summary>
    private void PaintSingle(Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, tile);
    }
    
    /// <summary>
    /// 绘制矩形区域
    /// </summary>
    private void PaintBox(Tilemap tilemap, Vector3Int center)
    {
        Vector3Int min = center - new Vector3Int(size.x / 2, size.y / 2, 0);
        Vector3Int max = center + new Vector3Int(size.x / 2, size.y / 2, 0);
        
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, center.z), tile);
            }
        }
    }
    
    /// <summary>
    /// 绘制圆形区域
    /// </summary>
    private void PaintCircle(Tilemap tilemap, Vector3Int center)
    {
        int radius = Mathf.Max(size.x, size.y) / 2;
        
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    tilemap.SetTile(new Vector3Int(center.x + x, center.y + y, center.z), tile);
                }
            }
        }
    }
    
    /// <summary>
    /// 获取笔刷影响的区域边界
    /// </summary>
    public BoundsInt GetBounds(Vector3Int position)
    {
        Vector3Int min, max;
        
        switch (mode)
        {
            case BrushMode.Single:
                min = position;
                max = position;
                break;
            case BrushMode.Box:
                min = position - new Vector3Int(size.x / 2, size.y / 2, 0);
                max = position + new Vector3Int(size.x / 2, size.y / 2, 0);
                break;
            case BrushMode.Circle:
                int radius = Mathf.Max(size.x, size.y) / 2;
                min = position - new Vector3Int(radius, radius, 0);
                max = position + new Vector3Int(radius, radius, 0);
                break;
            default:
                min = position;
                max = position;
                break;
        }
        
        return new BoundsInt(min, max - min + Vector3Int.one);
    }
    /// <summary>
    /// Erases a tile at the specified position on the tilemap
    /// </summary>
    /// <param name="brush">The tile brush</param>
    /// <param name="tilemap">Target tilemap</param>
    /// <param name="position">Grid position to erase</param>
    public  void Erase( Tilemap tilemap, Vector3Int position)
    {
        // Safety check: ensure tilemap reference is valid
        if (tilemap == null)
        {
            Debug.LogError("Erase failed: Tilemap reference is null");
            return;
        }

        // Set the tile at the position to null to erase it
        tilemap.SetTile(position, null);
    }
}