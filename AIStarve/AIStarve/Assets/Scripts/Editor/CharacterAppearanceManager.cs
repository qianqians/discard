using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// 角色外观管理器 - 创建和管理 Animator Override Controller
/// </summary>
public class CharacterAppearanceManager : EditorWindow
{
    #region 字段
    
    private string baseAnimatorControllerPath = "Assets/Animations/Character Animator.controller";
    private string animationsFolderPath = "Assets/Animations";
    private string outputFolderPath = "Assets/Animations/OverrideControllers";
    
    private Vector2 scrollPosition;
    private Dictionary<string, bool> characterFolders = new Dictionary<string, bool>();
    
    #endregion
    
    #region 窗口创建
    
    [MenuItem("Tools/角色外观管理器")]
    public static void ShowWindow()
    {
        CharacterAppearanceManager window = GetWindow<CharacterAppearanceManager>("角色外观管理器");
        window.minSize = new Vector2(500, 400);
        window.Show();
    }
    
    #endregion
    
    #region GUI 渲染
    
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("角色外观管理器", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 基础 Animator Controller
        EditorGUILayout.LabelField("基础设置:", EditorStyles.boldLabel);
        baseAnimatorControllerPath = ObjectField<RuntimeAnimatorController>(
            "基础 Animator Controller", baseAnimatorControllerPath);
        
        // 动画文件夹
        EditorGUILayout.LabelField("动画文件夹:", EditorStyles.boldLabel);
        animationsFolderPath = EditorGUILayout.TextField("动画文件夹路径", animationsFolderPath);
        if (GUILayout.Button("选择动画文件夹"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("选择动画文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                animationsFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        
        // 输出文件夹
        EditorGUILayout.LabelField("输出文件夹:", EditorStyles.boldLabel);
        outputFolderPath = EditorGUILayout.TextField("输出文件夹路径", outputFolderPath);
        if (GUILayout.Button("选择输出文件夹"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("选择输出文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                outputFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        
        EditorGUILayout.Space();
        
        // 扫描角色文件夹
        if (GUILayout.Button("扫描角色文件夹"))
        {
            ScanCharacterFolders();
        }
        
        EditorGUILayout.Space();
        
        // 显示角色文件夹列表
        if (characterFolders.Count > 0)
        {
            EditorGUILayout.LabelField("检测到的角色文件夹:", EditorStyles.boldLabel);
            
            // 创建键的副本以避免枚举时修改字典
            List<string> folderKeys = new List<string>(characterFolders.Keys);
            foreach (string folderPath in folderKeys)
            {
                EditorGUILayout.BeginHorizontal();
                characterFolders[folderPath] = EditorGUILayout.Toggle(characterFolders[folderPath], GUILayout.Width(20));
                EditorGUILayout.LabelField(folderPath);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            
            // 创建 Override Controller 按钮
            if (GUILayout.Button("为选中角色创建 Override Controller", GUILayout.Height(30)))
            {
                CreateOverrideControllers();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("点击\"扫描角色文件夹\"按钮来检测动画文件夹中的角色文件夹", MessageType.Info);
        }
        
        EditorGUILayout.Space();
        
        // 使用说明
        EditorGUILayout.LabelField("使用说明:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. 设置基础 Animator Controller 和动画文件夹路径\n" +
            "2. 点击\"扫描角色文件夹\"检测所有角色动画文件夹\n" +
            "3. 选择要创建 Override Controller 的角色\n" +
            "4. 点击按钮创建 Animator Override Controller\n" +
            "5. 在 CharacterController 中使用生成的 Override Controller 进行角色外观切换",
            MessageType.Info
        );
        
        EditorGUILayout.EndScrollView();
    }
    
    #endregion
    
    #region 工具方法
    
    private string ObjectField<T>(string label, string assetPath) where T : Object
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(150));
        
        T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        T newObj = (T)EditorGUILayout.ObjectField(obj, typeof(T), false);
        
        if (newObj != obj)
        {
            string newPath = AssetDatabase.GetAssetPath(newObj);
            if (!string.IsNullOrEmpty(newPath))
            {
                assetPath = newPath;
            }
        }
        
        EditorGUILayout.EndHorizontal();
        return assetPath;
    }
    
    private void ScanCharacterFolders()
    {
        characterFolders.Clear();
        
        if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + animationsFolderPath))
        {
            EditorUtility.DisplayDialog("错误", "动画文件夹不存在！", "确定");
            return;
        }
        
        // 获取动画文件夹中的所有子文件夹
        string fullPath = Application.dataPath.Replace("Assets", "") + animationsFolderPath;
        string[] subDirectories = Directory.GetDirectories(fullPath);
        
        foreach (string dir in subDirectories)
        {
            string folderName = Path.GetFileName(dir);
            
            // 检查文件夹中是否有 Animation Clip
            string[] animationFiles = Directory.GetFiles(dir, "*.anim");
            if (animationFiles.Length > 0)
            {
                characterFolders[folderName] = true;
            }
        }
        
        if (characterFolders.Count == 0)
        {
            EditorUtility.DisplayDialog("信息", "没有找到包含 Animation Clip 的角色文件夹", "确定");
        }
        else
        {
            EditorUtility.DisplayDialog("完成", $"找到了 {characterFolders.Count} 个角色文件夹", "确定");
        }
    }
    
    private void CreateOverrideControllers()
    {
        if (string.IsNullOrEmpty(baseAnimatorControllerPath))
        {
            EditorUtility.DisplayDialog("错误", "请设置基础 Animator Controller！", "确定");
            return;
        }
        
        RuntimeAnimatorController baseController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(baseAnimatorControllerPath);
        if (baseController == null)
        {
            EditorUtility.DisplayDialog("错误", "基础 Animator Controller 不存在！", "确定");
            return;
        }
        
        // 确保输出文件夹存在
        if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + outputFolderPath))
        {
            Directory.CreateDirectory(Application.dataPath.Replace("Assets", "") + outputFolderPath);
        }
        
        List<string> selectedCharacters = characterFolders
            .Where(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToList();
        
        if (selectedCharacters.Count == 0)
        {
            EditorUtility.DisplayDialog("错误", "请至少选择一个角色文件夹！", "确定");
            return;
        }
        
        try
        {
            int successCount = 0;
            
            foreach (string characterName in selectedCharacters)
            {
                EditorUtility.DisplayProgressBar("创建 Override Controller", 
                    $"正在处理: {characterName}", 
                    (float)successCount / selectedCharacters.Count);
                
                if (CreateSingleOverrideController(characterName, baseController))
                {
                    successCount++;
                }
            }
            
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("完成", 
                $"成功创建了 {successCount}/{selectedCharacters.Count} 个 Animator Override Controller\n" +
                $"输出文件夹: {outputFolderPath}", 
                "确定");
        }
        catch (System.Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("错误", $"创建过程中发生错误: {e.Message}", "确定");
            Debug.LogError($"创建 Override Controller 错误: {e}");
        }
    }
    
    private bool CreateSingleOverrideController(string characterName, RuntimeAnimatorController baseController)
    {
        string characterFolder = Path.Combine(animationsFolderPath, characterName);
        
        if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + characterFolder))
        {
            Debug.LogError($"角色文件夹不存在: {characterFolder}");
            return false;
        }
        
        // 获取角色文件夹中的所有 Animation Clip
        string[] animationGuids = AssetDatabase.FindAssets("t:AnimationClip", new[] { characterFolder });
        if (animationGuids.Length == 0)
        {
            Debug.LogWarning($"角色文件夹中没有找到 Animation Clip: {characterFolder}");
            return false;
        }
        
        // 创建 Animator Override Controller
        AnimatorOverrideController overrideController = new AnimatorOverrideController(baseController);
        
        // 映射 Animation Clip
        Dictionary<string, AnimationClip> clipMapping = new Dictionary<string, AnimationClip>();
        
        foreach (string guid in animationGuids)
        {
            string clipPath = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            
            if (clip != null)
            {
                string clipName = Path.GetFileNameWithoutExtension(clipPath);
                clipMapping[clipName] = clip;
            }
        }
        
        // 应用映射到 Override Controller
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);
        
        for (int i = 0; i < overrides.Count; i++)
        {
            var pair = overrides[i];
            string originalClipName = pair.Key?.name;
            
            if (!string.IsNullOrEmpty(originalClipName) && clipMapping.ContainsKey(originalClipName))
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(pair.Key, clipMapping[originalClipName]);
            }
        }
        
        overrideController.ApplyOverrides(overrides);
        
        // 保存 Override Controller
        string outputPath = Path.Combine(outputFolderPath, $"{characterName} Override.controller");
        AssetDatabase.CreateAsset(overrideController, outputPath);
        
        Debug.Log($"创建 Animator Override Controller: {outputPath}");
        return true;
    }
    
    #endregion
}