using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SceneManagement;
using Abelkhan;
using TMPro;

public class SceneObjectSelect : MonoBehaviour
{
    public const string PrefabPath = "Perfabs/UI/SceneObjectSelect";
    private const string RectPrefabPath = "Perfabs/UI/RectForSceneObjectSelect";
    
    private RectTransform _rectTransform = null;
    private Transform _contentTransform;
    private List<GameObject> _itemRects = new List<GameObject>();

private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        Debug.Log($"SceneObjectSelect.Start: gameObject.activeSelf: {gameObject.activeSelf}, rectTransform: {_rectTransform != null}");
        
        if (_rectTransform != null)
        {
            Debug.Log($"SceneObjectSelect.Start: position: {_rectTransform.position}, anchoredPosition: {_rectTransform.anchoredPosition}, sizeDelta: {_rectTransform.sizeDelta}");
            Debug.Log($"SceneObjectSelect.Start: anchorMin: {_rectTransform.anchorMin}, anchorMax: {_rectTransform.anchorMax}, pivot: {_rectTransform.pivot}");
        }
        
        var parent = transform.parent;
        if (parent != null)
        {
            var parentRect = parent.GetComponent<RectTransform>();
            Debug.Log($"SceneObjectSelect.Start: parent: {parent.name}, parentRect: {parentRect?.rect}");
        }
        
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            Debug.Log($"SceneObjectSelect.Start: CanvasGroup alpha: {canvasGroup.alpha}, blocksRaycasts: {canvasGroup.blocksRaycasts}");
        }
        
        var image = GetComponent<Image>();
        if (image != null)
        {
            Debug.Log($"SceneObjectSelect.Start: Root Image color: {image.color}, enabled: {image.enabled}");
        }
        
        _contentTransform = transform.Find("Panel/Scroll View/Viewport/Content");
        if (_contentTransform == null)
        {
            Debug.LogWarning("SceneObjectSelect: Content transform not found at Panel/Scroll View/Viewport/Content");
        }
        else
        {
            Debug.Log($"SceneObjectSelect: Found Content node: {_contentTransform.name}");
        }
    }

    // 面板显示时调用（由外部调用）
public void ShowPanel()
    {
        Debug.Log("SceneObjectSelect.ShowPanel() called");
        
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            Debug.Log($"SceneObjectSelect.ShowPanel: Initialized _rectTransform: {_rectTransform != null}");
        }
        
        gameObject.SetActive(true);
        
        if (_contentTransform == null)
        {
            _contentTransform = transform.Find("Panel/Scroll View/Viewport/Content");
            Debug.Log($"SceneObjectSelect.ShowPanel: Content transform: {_contentTransform?.name ?? "null"}");
        }
        
        _lastUpdateTime = 0;
        _lastWindowSize = Vector2.zero;
        UpdatePanelPosition();
        
        
        LoadBagItems();
    }
    
  
    
    // 面板隐藏时调用（由外部调用）
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        UpdatePanelPosition();
    }

    private async void LoadBagItems()
    {
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null)
        {
            Debug.LogWarning("SceneObjectSelect: GameSceneManager is null");
            return;
        }

        var bag = await netScene.GetBag();
        if (bag != null && bag.items != null)
        {
            RefreshBagUI(bag);
        }
        else
        {
            Debug.LogWarning("SceneObjectSelect: Bag is null or empty");
        }
    }
    Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            var found = FindChildRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }
    private void RefreshBagUI(bag_def bag)
    {
        Debug.Log($"SceneObjectSelect.RefreshBagUI called, bag items count: {bag?.items?.Count ?? 0}");
        
        // 清除现有格子
        foreach (var item in _itemRects)
        {
            if (item != null) Destroy(item);
        }
        _itemRects.Clear();

        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null)
        {
            Debug.LogWarning("SceneObjectSelect: GameSceneManager is null");
            return;
        }

        if (bag == null || bag.items == null || bag.items.Count == 0)
        {
            Debug.LogWarning("SceneObjectSelect: Bag is null or empty");
            return;
        }

        Debug.Log($"SceneObjectSelect: Loading {bag.items.Count} items");
        
        foreach (var item in bag.items)
        {
            Debug.Log($"SceneObjectSelect: Processing item - prefab_id: {item.perfab_id}, amount: {item.amount}");
            
            // 实例化预制体
            var prefab = Resources.Load<GameObject>(RectPrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"SceneObjectSelect: Failed to load prefab from path: {RectPrefabPath}");
                continue;
            }
            
            GameObject rectObj = Instantiate(prefab);
            if (rectObj == null)
            {
                Debug.LogError($"SceneObjectSelect: Failed to instantiate prefab: {RectPrefabPath}");
                continue;
            }

            Debug.Log($"SceneObjectSelect: Instantiated RectForSceneObjectSelect prefab");
            
            // 设置父对象为 Content，保持本地 transform 值
            if (_contentTransform == null)
            {
                Debug.LogError("SceneObjectSelect: _contentTransform is null!");
                Destroy(rectObj);
                continue;
            }
            
            rectObj.transform.SetParent(_contentTransform, true);
            Debug.Log($"SceneObjectSelect: Set parent to Content, parent: {rectObj.transform.parent?.name}");

// 获取 Image 组件并设置图标
            Transform ImageObject = FindChildRecursive(rectObj.transform, "Image");
            Image iconImage = ImageObject.GetComponent<Image>();
            if (iconImage != null)
            {
                // 根据 subtype 加载不同贴图
                string iconPath;
                if (item.subtype == (int)SubItemType.Mature)
                {
                    iconPath = sceneManager.SceneDB.GetMaturePath((int)item.perfab_id);
                }
                else
                {
                    iconPath = sceneManager.SceneDB.GetIconPath((int)item.perfab_id);
                }
                
                if (!string.IsNullOrEmpty(iconPath))
                {
                    Sprite sprite = LoadSpriteFromPath(iconPath);
                    if (sprite != null)
                    {
                        iconImage.sprite = sprite;
                        Debug.Log($"SceneObjectSelect: Loaded icon for prefabId {item.perfab_id}, subtype: {item.subtype}, path: {iconPath}");
                    }
                    else
                    {
                        Debug.LogWarning($"SceneObjectSelect: Failed to load sprite for path: {iconPath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"SceneObjectSelect: No icon path found for prefabId: {item.perfab_id}");
                }
            }
else
            {
                Debug.LogWarning("SceneObjectSelect: Image component not found on RectForSceneObjectSelect");
            }

            Transform textCountTransform = FindChildRecursive(rectObj.transform, "TextCount");
            if (textCountTransform != null)
            {
                TextMeshProUGUI textCount = textCountTransform.GetComponent<TextMeshProUGUI>();
                if (textCount != null)
                {
                    textCount.text = item.amount.ToString();
                    Debug.Log($"SceneObjectSelect: Set TextCount to {item.amount}");
                }
                else
                {
                    Debug.LogWarning("SceneObjectSelect: TextCount found but Text component missing");
                }
            }
else
            {
                Debug.LogWarning("SceneObjectSelect: TextCount not found in prefab");
            }

            var dragSender = rectObj.AddComponent<BagItemDragSender>();
            dragSender.itemData = item;
            Debug.Log($"SceneObjectSelect: Added BagItemDragSender for item uuid: {item.uuid}");

            _itemRects.Add(rectObj);
            Debug.Log($"SceneObjectSelect: Added item rect to list, total: {_itemRects.Count}");
        }
        
        Debug.Log($"SceneObjectSelect: Finished loading {_itemRects.Count} items");
    }

private Sprite LoadSpriteFromPath(string iconPath)
    {
        // 1. 尝试直接加载 Sprite
        Sprite sprite = Resources.Load<Sprite>(iconPath);
        if (sprite != null)
        {
            return sprite;
        }
        
        // 2. 尝试加载 Prefab，获取其 SpriteRenderer.sprite
        GameObject prefab = Resources.Load<GameObject>(iconPath);
        if (prefab != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                return sr.sprite;
            }
        }
        
        Debug.LogWarning($"LoadSpriteFromPath: 无法加载精灵，路径: {iconPath}");
        return null;
    }

    private float _lastUpdateTime;
    private Vector2 _lastWindowSize;

private void UpdatePanelPosition()
    {
        if (_rectTransform == null)
        {
            return;
        }

        Vector2 currentWindowSize = new Vector2(Screen.width, Screen.height);
        if (currentWindowSize == _lastWindowSize && _lastUpdateTime > 0)
        {
            return;
        }

        float panelWidth = 850f;
        float margin = 100f;
        
        _rectTransform.anchorMin = new Vector2(1, 0);
        _rectTransform.anchorMax = new Vector2(1, 1);
        _rectTransform.pivot = new Vector2(1, 0.5f);
        _rectTransform.anchoredPosition = new Vector2(-margin, 0);
        _rectTransform.sizeDelta = new Vector2(panelWidth, -margin * 2);

        _lastWindowSize = currentWindowSize;
        _lastUpdateTime = Time.time;
        
        Debug.Log($"SceneObjectSelect: Panel positioned - anchorMin: {_rectTransform.anchorMin}, anchorMax: {_rectTransform.anchorMax}, anchoredPosition: {_rectTransform.anchoredPosition}, sizeDelta: {_rectTransform.sizeDelta}, screen: {currentWindowSize}");
    }


}