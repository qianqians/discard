using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;
using SceneManagement;

public class DropPrefabObject : DropObject
{
    private string _prefabPath; // Store prefab path for later use
    private int PerfabPathId;
    public void StartDrag(string prefabPath, int PerfabPathIdArg)
    {
        _prefabPath = prefabPath; // Save prefab path
        PerfabPathId = PerfabPathIdArg;
        if (draggedObject != null)
        {
            Destroy(draggedObject);
        }

        var prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab != null)
        {
            draggedObject = Instantiate(prefab);
            draggedObject.transform.position = Vector3.zero;
            draggedObject.SetActive(true);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            Vector3 worldPosition = draggedObject.transform.position;
            
            // 对齐到 FarmingTilemap 格子中心
            GameObject farmingTilemapObj = GameObject.Find("FarmingTilemap");
            if (farmingTilemapObj != null)
            {
                Tilemap farmingTilemap = farmingTilemapObj.GetComponent<Tilemap>();
                if (farmingTilemap != null)
                {
                    Vector3Int cellPosition = farmingTilemap.WorldToCell(worldPosition);
                    worldPosition = farmingTilemap.GetCellCenterWorld(cellPosition);
                    Debug.Log($"Aligned position from {draggedObject.transform.position} to {worldPosition} (cell: {cellPosition})");
                }
            }
            
            GameSceneManager.Instance.TriggerEvent(SceneEvent.SendCreateObject, new SceneEventArgs {
                Position = worldPosition,
                PerfabPathId = PerfabPathId
            },true);
        }

        // Destroy the temporary dragged object
        if (draggedObject != null)
        {
            Destroy(draggedObject);
            draggedObject = null;
        }

        isDragging = false;
    }
}