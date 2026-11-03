using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Abelkhan;
using SceneManagement;

/// <summary>
/// 背包物品拖动发送器
/// 用于从背包拖动物品到快捷栏
/// </summary>
public class BagItemDragSender : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public item_def itemData;
    
    private Vector2 _dragStartPos;
    private const float DragThreshold = 10f;
    private bool _isDragging = false;
    private GameObject _dragPreview;
    private Canvas _parentCanvas;
    private RectTransform _previewRect;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragStartPos = eventData.position;
        _isDragging = true;
        
        CreateDragPreview();
        
        Debug.Log($"BagItemDragSender: Begin drag, item uuid: {itemData?.uuid}, prefabId: {itemData?.perfab_id}");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        
        if (_previewRect != null && _parentCanvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                _parentCanvas.worldCamera,
                out Vector2 localPoint);
            _previewRect.localPosition = localPoint;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        _isDragging = false;
        
        if (_dragPreview != null)
        {
            Destroy(_dragPreview);
            _dragPreview = null;
        }
        
        Debug.Log($"BagItemDragSender: End drag at position: {eventData.position}");
        
        CheckDropOnShortcut(eventData);
    }
    
    private void CreateDragPreview()
    {
        if (_parentCanvas == null)
        {
            _parentCanvas = GetComponentInParent<Canvas>();
        }
        
        if (_parentCanvas == null) return;
        
        _dragPreview = new GameObject("DragPreview");
        _dragPreview.transform.SetParent(_parentCanvas.transform, false);
        _dragPreview.transform.SetAsLastSibling();
        
        Image previewImage = _dragPreview.AddComponent<Image>();
        
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager != null && itemData != null)
        {
            string iconPath;
            if (itemData.subtype == (int)SubItemType.Mature)
            {
                iconPath = sceneManager.SceneDB.GetMaturePath((int)itemData.perfab_id);
            }
            else
            {
                iconPath = sceneManager.SceneDB.GetIconPath((int)itemData.perfab_id);
            }
            
            Sprite sprite = LoadSpriteFromPath(iconPath);
            if (sprite != null)
            {
                previewImage.sprite = sprite;
            }
        }
        
        _previewRect = _dragPreview.GetComponent<RectTransform>();
        _previewRect.anchorMin = new Vector2(0.5f, 0.5f);
        _previewRect.anchorMax = new Vector2(0.5f, 0.5f);
        _previewRect.pivot = new Vector2(0.5f, 0.5f);
        
        previewImage.raycastTarget = false;
    }
    
    private Sprite LoadSpriteFromPath(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath)) return null;
        
        Sprite sprite = Resources.Load<Sprite>(iconPath);
        if (sprite != null)
        {
            return sprite;
        }
        
        GameObject prefab = Resources.Load<GameObject>(iconPath);
        if (prefab != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                return sr.sprite;
            }
        }
        
        Debug.LogWarning($"BagItemDragSender: Cannot load sprite from path: {iconPath}");
        return null;
    }
    
    private void CheckDropOnShortcut(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        foreach (var result in results)
        {
            var shortcut = result.gameObject.GetComponent<UIBottomButtonClick>();
            if (shortcut != null)
            {
                Debug.Log($"BagItemDragSender: Dropped on shortcut button, buttonId: {shortcut.buttonId}");
                
                var args = new UIEventType.EventArgs
                {
                    EventType = UIEventType.Event.SetButtomImage,
                    PrefabPathId = (int)itemData.perfab_id,
                    ItemData = itemData,
                    TargetButtonId = shortcut.buttonId
                };
                
                shortcut.OnEndDrag(args);
                break;
            }
        }
    }
}