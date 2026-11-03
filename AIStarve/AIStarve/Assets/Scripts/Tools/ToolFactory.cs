using System.Collections.Generic;
using UnityEngine;
using SceneManagement;

namespace Tools
{
    public static class ToolFactory
    {
        private static readonly Dictionary<int, System.Type> _toolTypeMap = new Dictionary<int, System.Type>
        {
            { HoeTool.HOE_TOOL_ID, typeof(HoeTool) }
        };

        public static ToolBase CreateTool(int toolId, Transform ownerTransform, SceneDataBase sceneDB)
        {
            if (!_toolTypeMap.TryGetValue(toolId, out System.Type toolType))
            {
                Debug.LogWarning($"ToolFactory: No tool type registered for ID {toolId}");
                return null;
            }

            ToolBase tool = System.Activator.CreateInstance(toolType) as ToolBase;
            if (tool == null)
            {
                Debug.LogError($"ToolFactory: Failed to create instance of {toolType.Name}");
                return null;
            }

            tool.Initialize(ownerTransform, sceneDB);

            if (!tool.CreateTool())
            {
                Debug.LogError($"ToolFactory: Failed to create tool GameObject for {toolType.Name}");
                return null;
            }

            Debug.Log($"ToolFactory: Created tool {toolType.Name} for ID {toolId}");
            return tool;
        }

        public static bool IsTool(int prefabId)
        {
            return _toolTypeMap.ContainsKey(prefabId);
        }

        public static bool IsSeedTool(int prefabId, SceneDataBase sceneDB)
        {
            if (sceneDB == null) return false;
            int type = sceneDB.GetPrefabType(prefabId);
            return type == SeedTool.SEED_TOOL_TYPE;
        }

        public static ToolBase CreateSeedTool(int prefabId, Transform ownerTransform, SceneDataBase sceneDB)
        {
            SeedTool tool = new SeedTool();
            tool.Initialize(ownerTransform, sceneDB);
            tool.SetSeedPrefabId(prefabId);

            if (!tool.CreateTool())
            {
                Debug.LogError($"ToolFactory: Failed to create SeedTool for prefab ID {prefabId}");
                return null;
            }

            Debug.Log($"ToolFactory: Created SeedTool for prefab ID {prefabId}");
            return tool;
        }

        public static void RegisterToolType(int toolId, System.Type toolType)
        {
            if (!typeof(ToolBase).IsAssignableFrom(toolType))
            {
                Debug.LogError($"ToolFactory: Type {toolType.Name} does not inherit from ToolBase");
                return;
            }

            if (_toolTypeMap.ContainsKey(toolId))
            {
                Debug.LogWarning($"ToolFactory: Overwriting existing tool type for ID {toolId}");
            }

            _toolTypeMap[toolId] = toolType;
            Debug.Log($"ToolFactory: Registered tool type {toolType.Name} for ID {toolId}");
        }
    }
}