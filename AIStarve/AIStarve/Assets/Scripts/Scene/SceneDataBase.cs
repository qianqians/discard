using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SceneManagement
{
    public struct KeyboardMappingInfo
    {
        public string key;
        public int prefabId;
        public string itemGuid;
        public int amount;
    }

    public class SceneDataBase
    {
private Dictionary<int, (string path, string iconPath, string maturePath, int type, string tilemap, int time)> _prefabPathMap = new Dictionary<int, (string, string, string, int, string, int)>();
        private Dictionary<int, string[]> _prefabPathsMap = new Dictionary<int, string[]>();
        private Dictionary<string, int> _keyboardMapping = new Dictionary<string, int>();
        private Dictionary<string, KeyboardMappingEntry> _keyboardMappingData = new Dictionary<string, KeyboardMappingEntry>();
        private Dictionary<string, string> _triggerAreaToSceneMap = new Dictionary<string, string>();

        public SceneDataBase()
        {
            LoadPrefabMapping();
            LoadKeyboardMapping();
            LoadTriggerAreaMapping();
        }

        private void LoadPrefabMapping()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("PrefabMapping");
            if (jsonFile != null)
            {
                // 使用扩展类解析 JSON，以支持数组路径
                var data = JsonUtility.FromJson<PrefabMappingDataEx>(jsonFile.text);
                foreach (var prefab in data.prefabs)
                {
                    string singlePath = prefab.path;
                    string[] pathArray = prefab.pathArray;
int type = prefab.type;
string iconPath = prefab.iconPath;
                    string maturePath = prefab.maturePath;
                    string tilemap = prefab.tilemap;
                    int time = prefab.time;
                    
                    Debug.Log($"Prefab ID {prefab.id}: path='{singlePath}', pathArray={(pathArray != null ? string.Join(",", pathArray) : "null")}, type={type}");

                    // 如果 pathArray 存在且不为空，则存储数组路径
                    if (pathArray != null && pathArray.Length > 0)
                    {
                        _prefabPathsMap[prefab.id] = pathArray;
                        // 使用第一个路径作为单一路径（用于向后兼容）
                        if (string.IsNullOrEmpty(singlePath))
                        {
                            singlePath = pathArray[0];
                        }
                        Debug.Log($"Prefab ID {prefab.id} has array path with {pathArray.Length} elements, first: {pathArray[0]}");
                    }
                    else if (!string.IsNullOrEmpty(singlePath))
                    {
                        // 单一路径
                        Debug.Log($"Prefab ID {prefab.id} has single path: {singlePath}");
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab ID {prefab.id} has empty path.");
                    }

// 存储到主映射（使用单一路径，可能为空）
                    _prefabPathMap[prefab.id] = (singlePath, iconPath, maturePath, type, tilemap, time);
                }
            }
            else
            {
                Debug.LogError("PrefabMapping.json not found in Resources folder");
            }
        }

private void LoadKeyboardMapping()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "KeyBoardMapping.json");
            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<KeyboardMappingData>(json);
                if (data != null && data.keyboardMappings != null)
                {
                    foreach (var mapping in data.keyboardMappings)
                    {
                        _keyboardMapping[mapping.key] = mapping.prefabId;
                        _keyboardMappingData[mapping.key] = mapping;
                    }
                    Debug.Log($"Loaded keyboard mapping from persistentDataPath: {filePath}");
                }
            }
            else
            {
                TextAsset jsonFile = Resources.Load<TextAsset>("KeyBoardMapping");
                if (jsonFile != null)
                {
                    var data = JsonUtility.FromJson<KeyboardMappingData>(jsonFile.text);
                    if (data.keyboardMappings != null)
                    {
                        foreach (var mapping in data.keyboardMappings)
                        {
                            _keyboardMapping[mapping.key] = mapping.prefabId;
                            _keyboardMappingData[mapping.key] = mapping;
                        }
                    }
                    SaveKeyboardMappingToFile();
                    Debug.Log($"Initialized keyboard mapping from Resources and saved to: {filePath}");
                }
                else
                {
                    Debug.LogWarning("KeyBoardMapping.json not found in Resources folder, keyboard shortcuts will not be available.");
                }
            }
        }

        private void LoadTriggerAreaMapping()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("TriggerAreaMapping");
            if (jsonFile != null)
            {
                var data = JsonUtility.FromJson<TriggerAreaMappingData>(jsonFile.text);
                if (data.triggerAreas != null)
                {
                    foreach (var area in data.triggerAreas)
                    {
                        _triggerAreaToSceneMap[area.gameObjectName] = area.sceneName;
                        Debug.Log($"Trigger area mapping loaded: {area.gameObjectName} -> {area.sceneName}");
                    }
                }
            }
            else
            {
                Debug.LogError("TriggerAreaMapping.json not found in Resources folder");
            }
        }

public int GetPrefabIdByKey(string key)
        {
            if (_keyboardMapping.TryGetValue(key, out int prefabId))
            {
                return prefabId;
            }
            return -1;
        }

        public KeyboardMappingInfo? GetKeyboardMapping(string key)
        {
            if (_keyboardMappingData.TryGetValue(key, out var entry))
            {
                return new KeyboardMappingInfo
                {
                    key = entry.key,
                    prefabId = entry.prefabId,
                    itemGuid = entry.itemGuid,
                    amount = entry.amount
                };
            }
            return null;
        }

public void UpdateKeyboardMapping(string key, int prefabId, string itemGuid = "", int amount = 0)
        {
            if (_prefabPathMap.ContainsKey(prefabId) || prefabId == 0)
            {
                _keyboardMapping[key] = prefabId;
                _keyboardMappingData[key] = new KeyboardMappingEntry
                {
                    key = key,
                    prefabId = prefabId,
                    itemGuid = itemGuid,
                    amount = amount
                };
                Debug.Log($"Keyboard mapping updated: {key} -> prefabId:{prefabId}, guid:{itemGuid}, amount:{amount}");
            }
            else
            {
                Debug.LogWarning($"Prefab ID {prefabId} does not exist in PrefabMapping, but mapping will still be added.");
                _keyboardMapping[key] = prefabId;
                _keyboardMappingData[key] = new KeyboardMappingEntry
                {
                    key = key,
                    prefabId = prefabId,
                    itemGuid = itemGuid,
                    amount = amount
                };
            }
            SaveKeyboardMappingToFile();
        }

        public void DecreaseKeyAmount(string key)
        {
            if (_keyboardMappingData.TryGetValue(key, out var entry))
            {
                if (entry.amount > 0)
                {
                    entry.amount--;
                    _keyboardMappingData[key] = entry;
                    SaveKeyboardMappingToFile();
                    Debug.Log($"Decreased amount for key {key}: {entry.amount}");
                    
                    if (int.TryParse(key, out int keyIndex))
                    {
                        UIManager.Instance?.TriggerEvent(UIEventType.Event.KeyboardMappingUpdated, 
                            new UIEventType.EventArgs { KeyIndex = keyIndex }, true);
                    }
                }
            }
        }

private void SaveKeyboardMappingToFile()
        {
            var data = new KeyboardMappingData
            {
                keyboardMappings = new List<KeyboardMappingEntry>()
            };
            foreach (var kvp in _keyboardMappingData)
            {
                data.keyboardMappings.Add(kvp.Value);
            }
            string json = JsonUtility.ToJson(data, true);
            string filePath = Path.Combine(Application.persistentDataPath, "KeyBoardMapping.json");
            File.WriteAllText(filePath, json);
            Debug.Log($"Keyboard mapping saved to: {filePath}");
        }

        public string GetPrefabPath(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.path;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return null;
        }

        public string[] GetPrefabPaths(int id)
        {
            if (_prefabPathsMap.TryGetValue(id, out var paths))
            {
                return paths;
            }
            // 如果没有数组路径，则返回包含单一路径的数组（如果存在）
            if (_prefabPathMap.TryGetValue(id, out var prefabData) && !string.IsNullOrEmpty(prefabData.path))
            {
                return new string[] { prefabData.path };
            }
            return null;
        }

public string GetIconPath(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.iconPath;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return null;
        }

        public string GetMaturePath(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.maturePath;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return null;
        }

        public int GetPrefabType(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.type;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return -1;
        }

        public string GetTilemapTarget(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.tilemap;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return null;
        }

        /// <summary>
        /// 获取prefab的成熟时间（单位：分钟）
        /// </summary>
        /// <param name="id">prefab ID</param>
        /// <returns>成熟时间（分钟），如果未找到返回0</returns>
        public int GetPrefabTime(int id)
        {
            if (_prefabPathMap.TryGetValue(id, out var prefabData))
            {
                return prefabData.time;
            }
            Debug.LogError($"Prefab with ID {id} not found");
            return 0;
        }
        
        [System.Serializable]
        private class PrefabMappingData
        {
            public List<PrefabEntry> prefabs;
        }

        [System.Serializable]
        private class PrefabEntry
        {
            public int id;
            public string path;
            public string iconPath;
            public string tilemap;
            public int type;
        }

[System.Serializable]
        private class PrefabEntryEx
        {
            public int id;
            public string path;
            public string[] pathArray;
            public string iconPath;
            public string maturePath;
            public string tilemap;
            public int type;
            public int time;
        }

        [System.Serializable]
        private class PrefabMappingDataEx
        {
            public List<PrefabEntryEx> prefabs;
        }

        [System.Serializable]
        private class KeyboardMappingData
        {
            public List<KeyboardMappingEntry> keyboardMappings;
        }

[System.Serializable]
        private class KeyboardMappingEntry
        {
            public string key;
            public int prefabId;
            public string itemGuid;
            public int amount;
        }

        [System.Serializable]
        private class TriggerAreaEntry
        {
            public int id;
            public string gameObjectName;
            public string sceneName;
        }

        [System.Serializable]
        private class TriggerAreaMappingData
        {
            public List<TriggerAreaEntry> triggerAreas;
        }

        public string GetSceneNameByGameObjectName(string gameObjectName)
        {
            if (_triggerAreaToSceneMap.TryGetValue(gameObjectName, out string sceneName))
            {
                return sceneName;
            }
            Debug.LogError($"Trigger area mapping not found for GameObject: {gameObjectName}");
            return null;
        }
    }
}