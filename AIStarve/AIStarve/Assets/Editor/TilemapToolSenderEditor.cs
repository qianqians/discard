using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapToolSender))]
public class TilemapToolSenderEditor : Editor
{
    private SerializedProperty _itemType;
    private SerializedProperty _brush;
    private SerializedProperty _tilemap;
    private SerializedProperty _prefabPathId;
    private void OnEnable()
    {
        if (serializedObject == null)
        {
            return;
        }
        _itemType = serializedObject.FindProperty("_itemType");
        _brush = serializedObject.FindProperty("brush");
        _tilemap = serializedObject.FindProperty("tilemap");
        _prefabPathId = serializedObject.FindProperty("_prefabPathId");
    }
    
    public override void OnInspectorGUI()
    {
      
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_itemType);
        EditorGUILayout.PropertyField(_brush);
        EditorGUILayout.PropertyField(_tilemap);
        EditorGUILayout.PropertyField(_prefabPathId);
        // 检查_brush是否为空
        if (_brush == null)
        {
            EditorGUILayout.HelpBox("Brush property is not found!", MessageType.Error);
            return;
        }
        
        // 如果没有分配brush对象，提供创建按钮
        if (_brush.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign an AdvancedTileBrush to the brush field or create a new one.", MessageType.Warning);
            
            if (GUILayout.Button("Create New Brush"))
            {
                // 创建新的AdvancedTileBrush实例
                AdvancedTileBrush newBrush = new AdvancedTileBrush();
                
                // 保存到Assets文件夹
                string path = EditorUtility.SaveFilePanelInProject("Save Brush", "NewBrush.asset", "asset", "Save the new brush");
                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.CreateAsset(newBrush, path);
                    _brush.objectReferenceValue = newBrush;
                    AssetDatabase.SaveAssets();
                }
            }
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
            
            // 获取笔刷的属性
            SerializedObject brushObject = new SerializedObject(_brush.objectReferenceValue);
            SerializedProperty mode = brushObject.FindProperty("mode");
            SerializedProperty size = brushObject.FindProperty("size");
            SerializedProperty tile = brushObject.FindProperty("tile");
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(mode);
            EditorGUILayout.PropertyField(size);
            EditorGUILayout.PropertyField(tile);
            
            if (EditorGUI.EndChangeCheck())
            {
                brushObject.ApplyModifiedProperties();
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}