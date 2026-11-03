using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class TilemapObjectExporter
{
    [System.Serializable]
    public class ObjectPosition
    {
        public string name;
        public Vector3 position;
        public string prefabPath;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    [MenuItem("Tools/Export Tilemap Objects")]
    public static void ExportObjects()
    {
        // Find Grid object
        GameObject grid = GameObject.Find("Grid");
        if (grid == null)
        {
            Debug.LogError("Grid object not found");
            return;
        }

        // Find Objects Tilemap under Grid
        Transform objectsTilemap = grid.transform.Find("Objects Tilemap");
        if (objectsTilemap == null)
        {
            Debug.LogError("Objects Tilemap not found under Grid");
            return;
        }

        // Create export directory
        string exportDir = "Assets/Exports";
        if (!Directory.Exists(exportDir))
        {
            Directory.CreateDirectory(exportDir);
        }

        // Process objects
        var objectList = new List<ObjectPosition>();
        
        foreach (Transform child in objectsTilemap)
        {
            string objectName;
            string prefabPath = "";
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
            
            if (prefab != null)
            {
                objectName = Regex.Replace(child.name, @"\[.*?\]", "").Trim();
                prefabPath = AssetDatabase.GetAssetPath(prefab);
            }
            else
            {
                // Remove brackets and content from name
                objectName = Regex.Replace(child.name, @"\[.*?\]", "").Trim();
            }
            
            objectList.Add(new ObjectPosition {
                name = objectName,
                position = child.position,
                prefabPath = prefabPath
            });
        }
        
        // Convert to JSON
        var wrapper = new Wrapper<ObjectPosition> { items = objectList.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);
        
        // Save to file
        string path = Path.Combine(exportDir, "objectPositions.json");
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        
        Debug.Log($"Exported {objectsTilemap.childCount} objects to {path}");
    }
}