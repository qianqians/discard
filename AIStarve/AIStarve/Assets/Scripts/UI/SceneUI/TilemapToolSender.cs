using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

/// <summary>
/// Tilemap工具类型发送器（拖动模式）
/// </summary>
public class TilemapToolSender : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private TilemapItemType _itemType = TilemapItemType.ObjectsTilemapItem;

    private string _prefabPath;

    [SerializeField]
    private int _prefabPathId = 1;
    
    [SerializeField]
    private AdvancedTileBrush brush;

    [SerializeField]
    private Tilemap tilemap;


    private Vector2 _dragStartPos;
    private const float DragThreshold = 10f;
    
    /// <summary>
    /// 笔刷模式
    /// </summary>
    public AdvancedTileBrush.BrushMode BrushMode
    {
        get => brush != null ? brush.mode : AdvancedTileBrush.BrushMode.Single;
        set 
        { 
            if (brush != null) 
                brush.mode = value; 
        }
    }
    
    /// <summary>
    /// 笔刷大小
    /// </summary>
    public Vector2Int BrushSize
    {
        get => brush != null ? brush.size : Vector2Int.one;
        set 
        { 
            if (brush != null) 
                brush.size = value; 
        }
    }
    
    /// <summary>
    /// 笔刷Tile
    /// </summary>
    public TileBase BrushTile
    {
        get => brush != null ? brush.tile : null;
        set 
        { 
            if (brush != null) 
                brush.tile = value; 
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPos = eventData.position;
        
        
        // 立即发送TilemapToolChanged事件
        var args = new UIEventType.EventArgs
        {
            EventType = UIEventType.Event.TilemapToolChanged,
            ItemType = _itemType,
            PrefabPathId = _prefabPathId,
            PointerData = eventData,
            Brush = brush
        };
        UIManager.Instance.TriggerEvent(UIEventType.Event.TilemapToolChanged, args, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 转发拖动事件到UIManager
        var args = new UIEventType.EventArgs
        {
            EventType = UIEventType.Event.TilemapToolDragging,
            ItemType = _itemType,
            PointerData = eventData,
            PrefabPathId = _prefabPathId
        };
        UIManager.Instance.TriggerEvent(UIEventType.Event.TilemapToolDragging, args,true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 发送TilemapToolChangedEnd事件
        var endArgs = new UIEventType.EventArgs
        {
            EventType = UIEventType.Event.TilemapToolChangedEnd,
            ItemType = _itemType,
            PrefabPathId = _prefabPathId
        };
        UIManager.Instance.TriggerEvent(UIEventType.Event.TilemapToolChangedEnd, endArgs,true);
    }

    private void Awake()
    {
        // 移除原有的点击事件处理
        var button = GetComponent<Button>();
        if (button != null)
        {
            Destroy(button);
        }
        
        // 确保有Collider用于接收拖动事件
        if (GetComponent<Collider2D>() == null && GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}