using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TriggerAreaMappingTool
{
    private static string ConfigPath => Path.Combine(Application.dataPath, "Resources/TriggerAreaMapping.json");

    [MenuItem("Tools/扫描 TargetArea 并更新配置")]
    public static void ScanAndUpdateMapping()
    {
        GameObject targetAreaParent = GameObject.Find("TargetArea");
        if (targetAreaParent == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中未找到名为 \"TargetArea\" 的 GameObject", "确定");
            return;
        }

        int childCount = targetAreaParent.transform.childCount;
        if (childCount == 0)
        {
            EditorUtility.DisplayDialog("提示", "TargetArea 下没有任何子对象", "确定");
            return;
        }

        var existingData = LoadMapping();
        int nextId = 1;
        if (existingData.triggerAreas.Count > 0)
        {
            foreach (var area in existingData.triggerAreas)
                if (area.id >= nextId) nextId = area.id + 1;
        }

        var nameToExisting = new Dictionary<string, TriggerAreaEntry>();
        foreach (var area in existingData.triggerAreas)
            nameToExisting[area.gameObjectName] = area;

        var newAreas = new List<TriggerAreaEntry>();
        string activeSceneName = "PlayHouseOne.unity";

        for (int i = 0; i < childCount; i++)
        {
            string goName = targetAreaParent.transform.GetChild(i).name;

            if (nameToExisting.TryGetValue(goName, out var existing))
            {
                if (string.IsNullOrEmpty(existing.description))
                {
                    existing.description = "这是一个人的家里，家里除了自己住以外还有自己买的npc";
                }
                newAreas.Add(existing);
            }
            else
            {
                string defaultDesc = goName switch
                {
                    "TargetArea_Store" => "这是一个商店，可以购买家具以及一些家居类的东西",
                    _ => "这是一个人的家里，家里除了自己住以外还有自己买的npc"
                };

                newAreas.Add(new TriggerAreaEntry
                {
                    id = nextId++,
                    gameObjectName = goName,
                    sceneName = activeSceneName,
                    description = defaultDesc
                });
            }
        }

        var data = new TriggerAreaMappingData { triggerAreas = newAreas };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(ConfigPath, json);

        EditorUtility.DisplayDialog("完成",
            $"扫描完成！已将 {childCount} 个 TargetArea 子对象写入配置。\n路径: {ConfigPath}",
            "确定");

        Debug.Log($"TriggerAreaMappingTool: 更新完成，共 {newAreas.Count} 个区域");
    }

    private static TriggerAreaMappingData LoadMapping()
    {
        if (!File.Exists(ConfigPath))
            return new TriggerAreaMappingData();

        string json = File.ReadAllText(ConfigPath);
        return JsonUtility.FromJson<TriggerAreaMappingData>(json) ?? new TriggerAreaMappingData();
    }

    [System.Serializable]
    private class TriggerAreaMappingData
    {
        public List<TriggerAreaEntry> triggerAreas = new List<TriggerAreaEntry>();
    }

    [System.Serializable]
    private class TriggerAreaEntry
    {
        public int id;
        public string gameObjectName;
        public string sceneName;
        public string description;
    }
}
