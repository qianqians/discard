using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tilemap项处理器
/// </summary>
public class TilemapItemHandler : MonoBehaviour
{
    /// <summary>
    /// 单例实例
    /// </summary>
    public static TilemapItemHandler Instance { get; private set; }
    
    /// <summary>
    /// 当前选择的Tilemap项类型
    /// </summary>
    public TilemapItemType CurrentItemType { get; private set; }
    
    /// <summary>
    /// 对象Tilemap（用于实例化预制体）
    /// </summary>
    [SerializeField]
    private Tilemap _objectsTilemap;
    
    /// <summary>
    /// 地面Tilemap（用于绘制Tile）
    [SerializeField]
    private Tilemap _groundTilemap;
    
    /// <summary>
    /// Tile Palette预制体
    [SerializeField]
    private GameObject _tilePalettePrefab;
    
    private void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 设置当前Tilemap项类型
    /// </summary>
    public void SetItemType(TilemapItemType itemType)
    {
        CurrentItemType = itemType;
        
        // 可选：切换工具提示或其他UI反馈
        Debug.Log($"当前工具已切换至: {itemType}");
    }
    
    /// <summary>
    /// 处理鼠标点击事件
    /// </summary>
    public void OnMouseClick()
    {
        // 获取鼠标世界坐标
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        // 转换为Tilemap坐标
        Vector3Int tilemapPosition = _groundTilemap.WorldToCell(worldPosition);
        
        switch (CurrentItemType)
        {
            case TilemapItemType.ObjectsTilemapItem:
                // 处理对象放置
                PlaceObject(tilemapPosition);
                break;
                
            case TilemapItemType.GroundTilemapItem:
                // 处理地面绘制
                PaintGroundTile(tilemapPosition);
                break;
                
            default:
                Debug.LogWarning($"未处理的TilemapItemType: {CurrentItemType}");
                break;
        }
    }
    
    /// <summary>
    /// 放置对象到指定位置
    /// </summary>
    private void PlaceObject(Vector3Int position)
    {
        // 获取Tile Palette预制体
        GameObject tilePalette = Instantiate(_tilePalettePrefab, position, Quaternion.identity);
        tilePalette.name = $"Object_{position.x}_{position.y}";
        
        // 可选：添加额外配置
        // tilePalette.GetComponent<SomeComponent>().Initialize(...);
    }
    
    /// <summary>
    /// 绘制地面Tile
    /// </summary>
    private void PaintGroundTile(Vector3Int position)
    {
        // 获取Tile Palette的Tilemap组件
        Tilemap tilemap = _tilePalettePrefab.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tile Palette预制体缺少Tilemap组件");
            return;
        }
        
        // 获取第一个Tile的位置（假设Tile位于TilePalette的原点）
        Vector3Int tilePosition = new Vector3Int(0, 0, 0);
        TileBase tile = tilemap.GetTile(tilePosition);
        
        if (tile != null)
        {
            // 在目标Tilemap上设置Tile
            _groundTilemap.SetTile(position, tile);
            
            // 可选：显示调试信息
            Debug.Log($"在位置 {position} 放置Tile");
        }
        else
        {
            Debug.LogError("无法获取Tile Palette的Tile");
        }
    }
    
    // 可扩展：添加更多辅助方法
}