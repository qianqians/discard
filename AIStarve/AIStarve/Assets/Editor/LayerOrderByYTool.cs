using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;

 /// <summary>
 /// 根据Y坐标设置SpriteRenderer的Order in Layer的工具
 /// 规则：使用Ground Tilemap转换坐标，单元格Y坐标越小，sortingOrder值越大（从0开始）
 /// </summary>
 public class LayerOrderByYTool
 {
     /// <summary>
     /// 获取SpriteRenderer的底部世界坐标（考虑精灵高度）
     /// </summary>
     private static Vector3 GetBottomWorldPosition(SpriteRenderer renderer)
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
             Debug.LogError($"GetBottomWorldPosition error for renderer: {renderer?.name} - {ex.Message}");
             return Vector3.zero;
         }
     }

     [MenuItem("Tools/Set Order in Layer by Y Position")]
    public static void SetOrderInLayerByYPosition()
    {
        // 显示确认对话框
        if (!EditorUtility.DisplayDialog("Set Order in Layer by Y Position",
            "此工具将遍历场景中所有SpriteRenderer，根据其在Ground Tilemap中的Y坐标设置Order in Layer。\n\n" +
            "规则：单元格Y坐标越小，sortingOrder值越大（从0开始）。\n\n" +
            "继续执行吗？", "执行", "取消"))
        {
            Debug.Log("操作已取消");
            return;
        }

        // 查找Ground Tilemap
        GameObject groundTilemapObj = GameObject.Find("Ground Tilemap");
        if (groundTilemapObj == null)
        {
            EditorUtility.DisplayDialog("错误", "未找到Ground Tilemap对象", "确定");
            Debug.LogError("LayerOrderByYTool: Ground Tilemap not found in scene");
            return;
        }

        Tilemap groundTilemap = groundTilemapObj.GetComponent<Tilemap>();
        if (groundTilemap == null)
        {
            EditorUtility.DisplayDialog("错误", "Ground Tilemap对象缺少Tilemap组件", "确定");
            Debug.LogError("LayerOrderByYTool: Ground Tilemap object has no Tilemap component");
            return;
        }

        Debug.Log($"LayerOrderByYTool: 找到Ground Tilemap - {groundTilemap.name}");

        // 获取场景中所有SpriteRenderer
        SpriteRenderer[] allRenderers = Object.FindObjectsOfType<SpriteRenderer>(true);
        if (allRenderers.Length == 0)
        {
            EditorUtility.DisplayDialog("提示", "场景中没有找到SpriteRenderer", "确定");
            Debug.LogWarning("LayerOrderByYTool: No SpriteRenderer found in scene");
            return;
        }

        Debug.Log($"LayerOrderByYTool: 找到 {allRenderers.Length} 个SpriteRenderer");

        // 收集所有有效的SpriteRenderer并计算最小Y坐标
        List<SpriteRenderer> validRenderers = new List<SpriteRenderer>();
        int minCellY = int.MaxValue;
        int maxCellY = int.MinValue;

        try
        {
            // 第一步：收集数据和计算范围
            EditorUtility.DisplayProgressBar("计算坐标范围", "正在分析SpriteRenderer位置...", 0.1f);
            
            foreach (SpriteRenderer renderer in allRenderers)
            {
                if (renderer == null) continue;
                
                Vector3 bottomPosition = GetBottomWorldPosition(renderer);
                Vector3Int cellPos = groundTilemap.WorldToCell(bottomPosition);
                
                validRenderers.Add(renderer);
                
                if (cellPos.y < minCellY) minCellY = cellPos.y;
                if (cellPos.y > maxCellY) maxCellY = cellPos.y;
            }

            if (validRenderers.Count == 0)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("错误", "未找到有效的SpriteRenderer", "确定");
                Debug.LogError("LayerOrderByYTool: No valid SpriteRenderer found");
                return;
            }

            Debug.Log($"LayerOrderByYTool: Y坐标范围: {minCellY} 到 {maxCellY}");
            Debug.Log($"LayerOrderByYTool: sortingOrder范围: 0 到 {maxCellY - minCellY} (Y越小值越大)");

            // 第二步：应用排序
            int processedCount = 0;
            float totalRenderers = validRenderers.Count;
            
            foreach (SpriteRenderer renderer in validRenderers)
            {
                processedCount++;
                float progress = 0.1f + (0.8f * (processedCount / totalRenderers));
                EditorUtility.DisplayProgressBar("设置Order in Layer", 
                    $"正在处理 {processedCount}/{validRenderers.Count} ({renderer.name})", 
                    progress);

                // 记录Undo操作
                Undo.RecordObject(renderer, "Set Sorting Order by Y Position");
                
                // 计算新的sortingOrder（Y越小值越大，从0开始）
                Vector3 bottomPosition = GetBottomWorldPosition(renderer);
                Vector3Int cellPos = groundTilemap.WorldToCell(bottomPosition);
                int sortingOrder = maxCellY - cellPos.y;
                
                // 应用排序
                renderer.sortingOrder = sortingOrder + 1;
                renderer.sortingOrder = sortingOrder + 1;
                
                // 可选：记录详细信息
                if (processedCount <= 10) // 只记录前10个避免日志过多
                {
                    Debug.Log($"LayerOrderByYTool: {renderer.name} -> cellY: {cellPos.y}, order: {sortingOrder}");
                }
            }

            // 第三步：完成
            EditorUtility.DisplayProgressBar("完成", "正在保存更改...", 0.95f);
            
            // 确保更改被保存
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
            EditorUtility.ClearProgressBar();

            // 显示结果
            string resultMessage = $"设置完成！\n\n" +
                                 $"处理了 {validRenderers.Count} 个SpriteRenderer\n" +
                                 $"Y坐标范围: {minCellY} 到 {maxCellY}\n" +
                                 $"sortingOrder范围: 0 到 {maxCellY - minCellY} (Y越小值越大)";
            
            EditorUtility.DisplayDialog("完成", resultMessage, "确定");
            
            Debug.Log($"LayerOrderByYTool: 成功处理 {validRenderers.Count} 个SpriteRenderer");
            Debug.Log($"LayerOrderByYTool: Y坐标范围: {minCellY} 到 {maxCellY}");
            Debug.Log($"LayerOrderByYTool: sortingOrder范围: 0 到 {maxCellY - minCellY} (Y越小值越大)");

        }
        catch (System.Exception ex)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("错误", $"执行过程中发生错误: {ex.Message}", "确定");
            Debug.LogError($"LayerOrderByYTool: Exception: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    [MenuItem("Tools/Set Order in Layer by Y Position (快速模式)")]
    public static void SetOrderInLayerByYPositionFast()
    {
        // 快速模式：不显示确认对话框，直接执行
        
        // 查找Ground Tilemap
        GameObject groundTilemapObj = GameObject.Find("Ground Tilemap");
        if (groundTilemapObj == null)
        {
            Debug.LogError("LayerOrderByYTool: Ground Tilemap not found in scene");
            return;
        }

        Tilemap groundTilemap = groundTilemapObj.GetComponent<Tilemap>();
        if (groundTilemap == null)
        {
            Debug.LogError("LayerOrderByYTool: Ground Tilemap object has no Tilemap component");
            return;
        }

        // 获取场景中所有SpriteRenderer
        SpriteRenderer[] allRenderers = Object.FindObjectsOfType<SpriteRenderer>(true);
        if (allRenderers.Length == 0)
        {
            Debug.LogWarning("LayerOrderByYTool: No SpriteRenderer found in scene");
            return;
        }

        Debug.Log($"LayerOrderByYTool: 快速模式 - 找到 {allRenderers.Length} 个SpriteRenderer");

        // 计算最小和最大Y坐标
        int minCellY = int.MaxValue;
        int maxCellY = int.MinValue;
        foreach (SpriteRenderer renderer in allRenderers)
        {
            if (renderer == null) continue;
            Vector3 bottomPosition = GetBottomWorldPosition(renderer);
            Vector3Int cellPos = groundTilemap.WorldToCell(bottomPosition);
            if (cellPos.y < minCellY) minCellY = cellPos.y;
            if (cellPos.y > maxCellY) maxCellY = cellPos.y;
        }
        int processedCount = 0;

        foreach (SpriteRenderer renderer in allRenderers)
        {
            if (renderer == null) continue;
            
            // 记录Undo操作
            Undo.RecordObject(renderer, "Set Sorting Order by Y Position (Fast)");
            
            // 计算新的sortingOrder（Y越小值越大，从0开始）
            Vector3 bottomPosition = GetBottomWorldPosition(renderer);
            Vector3Int cellPos = groundTilemap.WorldToCell(bottomPosition);
            int sortingOrder = maxCellY - cellPos.y;

            // 应用排序
            renderer.sortingOrder = sortingOrder + 1;
            
            processedCount++;
        }

        Debug.Log($"LayerOrderByYTool: 快速模式 - 完成 {processedCount} 个SpriteRenderer");
        Debug.Log($"LayerOrderByYTool: Y坐标范围: {minCellY} 到 {maxCellY}, sortingOrder范围: 0 到 {maxCellY - minCellY}");
    }
}