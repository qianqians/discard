using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using SceneManagement;

/// <summary>
/// 使用AdvancedTileBrush在场景中绘制Tile对象的类
/// </summary>
public class DropTileBrushObject : DropObject
{
    public AdvancedTileBrush _brush; // 存储笔刷引用
    private Tilemap _targetTilemap; // 目标Tilemap
    private Vector3Int _lastPaintPosition; // 上次绘制位置，用于避免重复绘制
    private bool _isPainting = false; // 是否正在绘制
    private Dictionary<Vector3Int, TileBase> _originalTiles = new Dictionary<Vector3Int, TileBase>(); // 存储被覆盖的原始瓦片
    private int PerfabPathId = 0;
    /// <summary>
    /// 开始拖动时设置笔刷和目标Tilemap
    /// </summary>
    public void StartDrag(AdvancedTileBrush brush, Tilemap targetTilemap, int PerfabPathIdArg)
    {
        _brush = brush;
        _targetTilemap = targetTilemap;
        _isPainting = true;
        _lastPaintPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue); // 初始化为无效位置
        PerfabPathId = PerfabPathIdArg;
        // 创建一个可视化对象来显示笔刷预览（可选）
        if (draggedObject != null)
        {
            Destroy(draggedObject);
        }

        // 创建一个简单的可视化预览对象
        draggedObject = new GameObject("BrushPreview");
        var renderer = draggedObject.AddComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 0.5f); // 半透明
        draggedObject.SetActive(true);
    }

    /// <summary>
    /// 拖动过程中持续绘制
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        if (!_isPainting || _brush == null || _targetTilemap == null) return;

        base.OnDrag(eventData);

        // 获取世界坐标
        Vector3 worldPosition = GetWorldPosition(eventData.position);
        Vector3Int tilemapPosition = _targetTilemap.WorldToCell(worldPosition);

        // 避免在相同位置重复绘制
        if (tilemapPosition != _lastPaintPosition)
        {
            // 删除前一次绘制的对象：恢复原始瓦片
            if (_lastPaintPosition.x != int.MinValue)
            {
                // 恢复原始瓦片
                if (_originalTiles.TryGetValue(_lastPaintPosition, out TileBase originalTile))
                {
                    _targetTilemap.SetTile(_lastPaintPosition, originalTile);
                }
            }
            
            // 保存当前位置的原始瓦片（如果尚未保存）
            if (!_originalTiles.ContainsKey(tilemapPosition))
            {
                _originalTiles[tilemapPosition] = _targetTilemap.GetTile(tilemapPosition);
            }
            
            // 使用笔刷绘制新位置
            _brush.Paint(_targetTilemap, tilemapPosition);
            _lastPaintPosition = tilemapPosition;
            
            // 更新预览对象位置
            if (draggedObject != null)
            {
                draggedObject.transform.position = _targetTilemap.CellToWorld(tilemapPosition) + new Vector3(0.5f, 0.5f, 0); // 居中对齐
            }
        }
    }

    /// <summary>
    /// 结束拖动时处理绘制完成事件
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!_isPainting) return;

        // 发送场景变更事件到服务器
        if (_brush != null && _targetTilemap != null)
        {
            Vector3 worldPosition = draggedObject.transform.position;
            
            // 对齐到对应 Tilemap 格子中心
            if (_targetTilemap != null)
            {
                Vector3Int cellPosition = _targetTilemap.WorldToCell(worldPosition);
                worldPosition = _targetTilemap.GetCellCenterWorld(cellPosition);
                Debug.Log($"TileBrush aligned position from {draggedObject.transform.position} to {worldPosition} (cell: {cellPosition})");
            }
            
            // 这里可以添加通知服务器场景变化的逻辑
            GameSceneManager.Instance.TriggerEvent(SceneEvent.SendCreateObject, new SceneEventArgs
            {
                Position = worldPosition,
                PerfabPathId = PerfabPathId
            }, true) ;
            /*
            SceneChange change = new SceneChange();
            // 填充change对象的add_tiles字段
            netScene.ChangeScene(change);
            */
        }

        // 清理临时对象
        if (draggedObject != null)
        {
            Destroy(draggedObject);
            draggedObject = null;
        }

        _isPainting = false;
        isDragging = false;
        _brush = null;
        _targetTilemap = null;
    }
}