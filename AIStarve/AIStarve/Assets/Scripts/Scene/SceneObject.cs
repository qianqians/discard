using UnityEngine;
using UnityEngine.Tilemaps;
namespace SceneManagement
{
public class SceneObject
    {
        public string ObjectId { get; private set; }
        public string PrefabPath { get; private set; }
        public GameObject GameObject { get; private set; }
        public Vector3 Position => GameObject.transform.position;
        public TilemapItemType typeItem  {get; private set; }
        public string tilemap { get; private set; }
        public Vector3 _position;

        private static int _cachedMaxCellY = -1;

    public SceneObject(string objectId, string prefabPath, TilemapItemType itemType, string tilemapArg)
        {
            ObjectId = objectId;
            PrefabPath = prefabPath;
            typeItem = itemType;
            tilemap = tilemapArg;
            _position = Vector3.zero;
        }

        public void Initialize(Vector3 position, bool bFirstCreate)
        {
            _position = position;
            if (typeItem == TilemapItemType.GroundTilemapItem)
            {
                // Load tile asset from Resources
                TileBase tile = Resources.Load<TileBase>(PrefabPath);
                if (tile == null)
                {
                    Debug.LogError($"Tile not found at path: {PrefabPath}");
                    return;
                }

                // Find ground tilemap in scene
                GameObject tilemaTarget = GameObject.Find(tilemap);
                if (tilemaTarget == null)
                {
                    Debug.LogError("Ground Tilemap not found in scene");
                    return;
                }
                
                Tilemap groundTilemap = tilemaTarget.GetComponent<Tilemap>();
                if (groundTilemap == null)
                {
                    Debug.LogError("Ground Tilemap object has no Tilemap component");
                    return;
                }

                // Convert world position to cell position
                Vector3Int cellPosition = groundTilemap.WorldToCell(position);

                // Create brush and paint tile
                var brush = ScriptableObject.CreateInstance<AdvancedTileBrush>();
                brush.tile = tile;
                brush.Paint(groundTilemap, cellPosition);
                groundTilemap.RefreshAllTiles();
                
                // Clean up brush resources
                Object.Destroy(brush);
            }
else
            {
                var prefab = Resources.Load<GameObject>(PrefabPath);
                if (prefab == null)
                {
                    Debug.LogError($"Failed to load object prefab at {PrefabPath}");
                    return;
                }

                Vector3 alignedPosition = GetAlignedPosition(position, bFirstCreate);
                
                GameObject = Object.Instantiate(prefab, alignedPosition, Quaternion.identity);
                GameObject.name = $"Object_{ObjectId}";
                
                // 只在第一次创建时调整位置，使物体底部对齐到格子中心
                if (bFirstCreate)
                {
                    SpriteRenderer spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer == null)
                    {
                        spriteRenderer = GameObject.GetComponentInChildren<SpriteRenderer>();
                    }
                    if (spriteRenderer != null && spriteRenderer.sprite != null)
                    {
                        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
                        float scaleY = GameObject.transform.lossyScale.y;
                        float halfHeight = spriteHeight * scaleY / 2f;
                        // 再减少一半，使物体底部对齐到格子中心
                        float quarterHeight = halfHeight / 2f;
                        GameObject.transform.position = new Vector3(
                            alignedPosition.x,
                            alignedPosition.y + quarterHeight,
                            alignedPosition.z
                        );
                        Debug.Log($"SceneObject: aligned position adjusted for bottom alignment, spriteHeight: {spriteHeight}, scaleY: {scaleY}, quarterHeight: {quarterHeight}");
                    }
                }

                _position = GameObject.transform.position;
                SetSortingOrderForGameObject(GameObject);
                GameObject.SetActive(true);
            }
        }

        private Vector3 GetAlignedPosition(Vector3 worldPosition, bool bFirstCreate)
        {
            if( !bFirstCreate )//不需要做位置矫正
            {
                return worldPosition;
            }
            // 如果tilemap为空，直接返回原始位置
            if (string.IsNullOrEmpty(tilemap))
                return worldPosition;
            
            // 查找Tilemap对象
            GameObject tilemapObject = GameObject.Find(tilemap);
            if (tilemapObject == null)
            {
                Debug.LogWarning($"Tilemap '{tilemap}' not found, using original position");
                return worldPosition;
            }
            
            Tilemap tilemapComponent = tilemapObject.GetComponent<Tilemap>();
            if (tilemapComponent == null)
            {
                Debug.LogWarning($"Tilemap '{tilemap}' has no Tilemap component, using original position");
                return worldPosition;
            }
            
            // 转换到Cell坐标
            Vector3Int cellPosition = tilemapComponent.WorldToCell(worldPosition);
            Vector3 cellCenter = tilemapComponent.GetCellCenterWorld(cellPosition);

            return cellCenter;
        }

        public void SetPosition(Vector3 newPosition)
        {
            if (GameObject == null) return;
            
            GameObject.transform.position = newPosition;
            _position = newPosition;
            GameSceneManager.Instance.TriggerEvent(SceneEvent.ObjectPlaced, new SceneEventArgs {
                ObjectId = ObjectId,
                Position = newPosition
            });
        }
        
        /// <summary>
        /// 获取SpriteRenderer的底部世界坐标（考虑精灵高度）
        /// </summary>
        private Vector3 GetBottomWorldPosition(SpriteRenderer renderer)
        {
            try
            {
                if (renderer == null) return Vector3.zero;
                if (renderer.transform == null) return Vector3.zero;
                Vector3 bottomPosition = renderer.transform.position;
                // 检查bounds是否有效
                if (renderer.bounds.size.y > 0)
                {
                    bottomPosition.y -= renderer.bounds.size.y / 2f;
                }
                return bottomPosition;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"SceneObject.GetBottomWorldPosition error for renderer: {renderer?.name} - {ex.Message}");
                return Vector3.zero;
            }
        }
        
/// <summary>
        /// 为GameObject设置排序顺序（基于其底部位置）
        /// 规则：使用Ground Tilemap，Y坐标越小，sortingOrder值越大
        /// </summary>
        private void SetSortingOrderForGameObject(GameObject gameObject)
        {
            if (gameObject == null) return;
            
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer == null) return;
            }
            
            // 查找Ground Tilemap
            GameObject groundTilemapObj = GameObject.Find("Ground Tilemap");
            if (groundTilemapObj == null)
            {
                Debug.LogWarning("SetSortingOrderForGameObject: Ground Tilemap not found");
                return;
            }
            
            Tilemap groundTilemap = groundTilemapObj.GetComponent<Tilemap>();
            if (groundTilemap == null)
            {
                Debug.LogWarning("SetSortingOrderForGameObject: Ground Tilemap object has no Tilemap component");
                return;
            }
            
            // 计算底部位置和单元格
            Vector3 bottomPosition = GetBottomWorldPosition(spriteRenderer);
            Vector3Int cellPos = groundTilemap.WorldToCell(bottomPosition);
            
            // 独立计算 maxCellY（带缓存）
            int maxCellY = CalculateMaxCellY(groundTilemap);
            
            // 计算排序顺序：Y越小值越大
            int sortingOrder = maxCellY - cellPos.y;
            // 根据对象类型调整排序顺序
            int finalOrder = sortingOrder;
            if (typeItem == TilemapItemType.ObjectsTilemapGrowUpItem)
            {
                // Vegetable 需要 -1 的 order in Layer
                finalOrder = sortingOrder - 1;
                Debug.Log($"SceneObject.SetSortingOrderForGameObject: {gameObject.name} -> cellY: {cellPos.y}, base order: {sortingOrder}, vegetable order: {finalOrder}");
            }
            else
            {
                Debug.Log($"SceneObject.SetSortingOrderForGameObject: {gameObject.name} -> cellY: {cellPos.y}, order: {finalOrder}");
            }
            spriteRenderer.sortingOrder = finalOrder;
        }
        
        /// <summary>
        /// 计算场景中最大的 Y 单元格值（带缓存）
        /// </summary>
        private int CalculateMaxCellY(Tilemap tilemap)
        {
            if (_cachedMaxCellY > 0) return _cachedMaxCellY;
            
            SpriteRenderer[] allRenderers = Object.FindObjectsOfType<SpriteRenderer>();
            int maxY = int.MinValue;
            
            foreach (SpriteRenderer renderer in allRenderers)
            {
                if (renderer == null) continue;
                
                Vector3 bottomPosition = GetBottomWorldPosition(renderer);
                Vector3Int cellPos = tilemap.WorldToCell(bottomPosition);
                if (cellPos.y > maxY) maxY = cellPos.y;
            }
            
            _cachedMaxCellY = maxY == int.MinValue ? 100 : maxY;
            Debug.Log($"SceneObject.CalculateMaxCellY: maxCellY = {_cachedMaxCellY}");
            return _cachedMaxCellY;
        }
    }
}