using UnityEngine;
using System;

public class TestJsonArray : MonoBehaviour
{
    [System.Serializable]
    private class PrefabEntry
    {
        public int id;
        public string[] path;
        public string iconPath;
        public string tilemap;
        public int type;
    }

    [System.Serializable]
    private class PrefabMappingData
    {
        public PrefabEntry[] prefabs;
    }

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("PrefabMapping");
        if (jsonFile != null)
        {
            var data = JsonUtility.FromJson<PrefabMappingData>(jsonFile.text);
            foreach (var prefab in data.prefabs)
            {
                Debug.Log($"Prefab ID {prefab.id}, path array length = {prefab.path?.Length ?? 0}, type = {prefab.type}");
                if (prefab.path != null)
                {
                    for (int i = 0; i < prefab.path.Length; i++)
                    {
                        Debug.Log($"  [{i}] = {prefab.path[i]}");
                    }
                }
            }
        }
    }
}