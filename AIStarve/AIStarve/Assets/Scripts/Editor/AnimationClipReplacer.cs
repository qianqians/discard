using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Animation Clip 复制和替换工具
/// 功能：复制源目录下的 Animation Clip，使用新贴图名称命名，并替换其中的 Sprite 引用
/// </summary>
public class AnimationClipReplacer : EditorWindow
{
    #region 字段
    
    private string sourceFolderPath = "Assets/Animations";
    private string targetFolderPath = "Assets/Animations";
    private Sprite originalSprite;
    private Sprite newSprite;
    private RuntimeAnimatorController baseController;
    private bool useSpriteSequence = false;
    
    private Vector2 scrollPosition;
    private bool showAdvancedOptions = false;
    
    #endregion
    
    #region 窗口创建
    
    [MenuItem("Tools/Animation Clip 复制和替换工具")]
    public static void ShowWindow()
    {
        AnimationClipReplacer window = GetWindow<AnimationClipReplacer>("Animation Clip 工具");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }
    
    #endregion
    
    #region GUI 渲染
    
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation Clip 复制和替换工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 源文件夹路径
        EditorGUILayout.LabelField("源动画文件夹:", EditorStyles.boldLabel);
        sourceFolderPath = EditorGUILayout.TextField("源文件夹路径", sourceFolderPath);
        if (GUILayout.Button("选择源文件夹"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("选择源动画文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                sourceFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        
        EditorGUILayout.Space();
        
        // 目标文件夹路径
        EditorGUILayout.LabelField("目标文件夹:", EditorStyles.boldLabel);
        targetFolderPath = EditorGUILayout.TextField("目标文件夹路径", targetFolderPath);
        if (GUILayout.Button("选择目标文件夹"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("选择目标文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                targetFolderPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
            }
        }
        
        EditorGUILayout.Space();
        
        // 贴图设置
        EditorGUILayout.LabelField("贴图设置:", EditorStyles.boldLabel);
        originalSprite = (Sprite)EditorGUILayout.ObjectField("原始贴图", originalSprite, typeof(Sprite), false);
        newSprite = (Sprite)EditorGUILayout.ObjectField("新贴图", newSprite, typeof(Sprite), false);
        
        EditorGUILayout.HelpBox("原始贴图：要替换的 Sprite 对象\n新贴图：替换后的 Sprite 对象\n注意：请确保新贴图包含完整的动画序列", MessageType.Info);
        
        EditorGUILayout.Space();
        
        // Animator Controller 设置
        EditorGUILayout.LabelField("Animator Controller 设置:", EditorStyles.boldLabel);
        baseController = (RuntimeAnimatorController)EditorGUILayout.ObjectField("基础控制器", baseController, typeof(RuntimeAnimatorController), false);
        
        EditorGUILayout.HelpBox("基础控制器：用于创建 Animator Override Controller 的基础控制器", MessageType.Info);
        
        EditorGUILayout.Space();
        
        // 高级选项
        showAdvancedOptions = EditorGUILayout.Foldout(showAdvancedOptions, "高级选项");
        if (showAdvancedOptions)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("高级选项允许更精确地控制替换过程", MessageType.Info);
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.Space();
        
        // 操作按钮
        if (GUILayout.Button("开始复制和替换", GUILayout.Height(30)))
        {
            if (ValidateInput())
            {
                ProcessAnimationClips();
            }
        }
        
        EditorGUILayout.Space();
        
        // 状态信息
        EditorGUILayout.LabelField("使用说明:", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "1. 选择源动画文件夹和目标文件夹\n" +
            "2. 选择要替换的原始贴图和新贴图\n" +
            "3. 选择基础 Animator Controller（可选）\n" +
            "4. 点击按钮开始处理\n" +
            "5. 工具会自动复制 Animation Clip 并替换其中的 Sprite 引用\n" +
            "6. 如果提供了基础控制器，会自动生成 Animator Override Controller\n" +
            "注意：请确保新贴图包含完整的动画序列，否则动画可能无法正常播放",
            MessageType.Info
        );
        
        EditorGUILayout.EndScrollView();
    }
    
    #endregion
    
    #region 输入验证
    
    private bool ValidateInput()
    {
        // 检查源文件夹是否存在
        if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + sourceFolderPath))
        {
            EditorUtility.DisplayDialog("错误", "源文件夹不存在！", "确定");
            return false;
        }
        
        // 检查目标文件夹是否存在
        if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + targetFolderPath))
        {
            EditorUtility.DisplayDialog("错误", "目标文件夹不存在！", "确定");
            return false;
        }
        
        // 检查贴图对象
        if (originalSprite == null)
        {
            EditorUtility.DisplayDialog("错误", "请选择原始贴图！", "确定");
            return false;
        }
        
        if (newSprite == null)
        {
            EditorUtility.DisplayDialog("错误", "请选择新贴图！", "确定");
            return false;
        }
        
        // 检查源文件夹中是否有 Animation Clip
        string[] animationClips = AssetDatabase.FindAssets("t:AnimationClip", new[] { sourceFolderPath });
        if (animationClips.Length == 0)
        {
            EditorUtility.DisplayDialog("警告", "源文件夹中没有找到 Animation Clip！", "确定");
            return false;
        }
        
        return true;
    }
    
    #endregion
    
    #region 处理逻辑
    
    private void ProcessAnimationClips()
    {
        try
        {
            EditorUtility.DisplayProgressBar("处理中", "正在扫描 Animation Clip...", 0f);
            
            // 获取源文件夹中的所有 Animation Clip
            string[] animationClipGuids = AssetDatabase.FindAssets("t:AnimationClip", new[] { sourceFolderPath });
            List<AnimationClip> sourceClips = new List<AnimationClip>();
            
            foreach (string guid in animationClipGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (clip != null)
                {
                    sourceClips.Add(clip);
                }
            }
            
            if (sourceClips.Count == 0)
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("完成", "没有找到可处理的 Animation Clip", "确定");
                return;
            }
            
            // 创建目标子文件夹（使用新贴图名称）
            string targetSubFolder = Path.Combine(targetFolderPath, newSprite.name);
            if (!Directory.Exists(Application.dataPath.Replace("Assets", "") + targetSubFolder))
            {
                Directory.CreateDirectory(Application.dataPath.Replace("Assets", "") + targetSubFolder);
            }
            
            // 处理每个 Animation Clip
            List<AnimationClip> processedClips = new List<AnimationClip>();
            for (int i = 0; i < sourceClips.Count; i++)
            {
                AnimationClip sourceClip = sourceClips[i];
                float progress = (float)i / sourceClips.Count;
                EditorUtility.DisplayProgressBar("处理中", $"正在处理: {sourceClip.name}", progress);
                
                AnimationClip processedClip = ProcessSingleClip(sourceClip, targetSubFolder);
                if (processedClip != null)
                {
                    processedClips.Add(processedClip);
                }
            }
            
            // 如果提供了基础控制器，生成 Animator Override Controller
            if (baseController != null && processedClips.Count > 0)
            {
                EditorUtility.DisplayProgressBar("处理中", "正在生成 Animator Override Controller...", 0.9f);
                CreateAnimatorOverrideController(processedClips, targetSubFolder);
            }
            
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            
            string completionMessage = $"成功处理了 {sourceClips.Count} 个 Animation Clip\n" +
                $"目标文件夹: {targetSubFolder}";
            
            if (baseController != null)
            {
                completionMessage += $"\n已生成 Animator Override Controller: {newSprite.name}_OverrideController";
            }
            
            EditorUtility.DisplayDialog("完成", completionMessage, "确定");
        }
        catch (System.Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("错误", $"处理过程中发生错误: {e.Message}", "确定");
            Debug.LogError($"Animation Clip 处理错误: {e}");
        }
    }
    
    private AnimationClip ProcessSingleClip(AnimationClip sourceClip, string targetFolder)
    {
        // 复制 Animation Clip
        string sourcePath = AssetDatabase.GetAssetPath(sourceClip);
        string fileName = Path.GetFileName(sourcePath);
        string targetPath = Path.Combine(targetFolder, fileName);
        
        // 复制文件
        if (!AssetDatabase.CopyAsset(sourcePath, targetPath))
        {
            Debug.LogError($"复制 Animation Clip 失败: {sourcePath} -> {targetPath}");
            return null;
        }
        
        // 加载复制的 Animation Clip
        AnimationClip targetClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(targetPath);
        if (targetClip == null)
        {
            Debug.LogError($"加载复制的 Animation Clip 失败: {targetPath}");
            return null;
        }
        
        // 替换 Sprite 引用
        ReplaceSpriteReferences(targetClip);
        
        // 保存修改
        EditorUtility.SetDirty(targetClip);
        
        return targetClip;
    }
    
    private void ReplaceSpriteReferences(AnimationClip clip)
    {
        // 创建 SpriteRenderer 的曲线绑定
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";  // 空路径表示根对象
        curveBinding.propertyName = "m_Sprite";
        
        // 获取 ObjectReferenceCurve
        ObjectReferenceKeyframe[] keyFrames = AnimationUtility.GetObjectReferenceCurve(clip, curveBinding);
        
        if (keyFrames == null)
        {
            Debug.LogWarning(clip.name + " => 无关键帧");
            return;
        }
        
        // 获取新贴图的所有 Sprite（从同一个图集中）
        List<Sprite> newSprites = GetSpritesFromSameTexture(newSprite);
        if (newSprites.Count == 0)
        {
            Debug.LogWarning($"无法从新贴图获取 Sprite 序列: {newSprite.name}");
            return;
        }
        
        // 获取原始贴图的所有 Sprite
        List<Sprite> originalSprites = GetSpritesFromSameTexture(originalSprite);
        if (originalSprites.Count == 0)
        {
            Debug.LogWarning($"无法从原始贴图获取 Sprite 序列: {originalSprite.name}");
            return;
        }
        
        bool hasReplacement = false;
        
        for (int i = 0; i < keyFrames.Length; i++)
        {
            if (keyFrames[i].value == null)
                continue;
            
            // 获取当前 Sprite 的名称
            string currentSpriteName = keyFrames[i].value.name;
            
            // 从 Sprite 名称中提取数字后缀
            int originalIndex = ExtractSpriteIndex(currentSpriteName);
            if (originalIndex < 0)
            {
                Debug.LogWarning($"无法从 Sprite 名称中提取索引: {currentSpriteName}");
                continue;
            }
            
            // 在原始 Sprite 序列中查找对应的 Sprite
            Sprite originalSpriteInSequence = originalSprites.Find(sprite =>
                ExtractSpriteIndex(sprite.name) == originalIndex);
            
            if (originalSpriteInSequence != null)
            {
                // 在新 Sprite 序列中查找相同索引的 Sprite
                Sprite newSpriteInSequence = newSprites.Find(sprite =>
                    ExtractSpriteIndex(sprite.name) == originalIndex);
                
                if (newSpriteInSequence != null)
                {
                    // 使用对应的新 Sprite 进行替换
                    keyFrames[i].value = newSpriteInSequence;
                    hasReplacement = true;
                    Debug.Log($"替换 Sprite: {currentSpriteName} -> {newSpriteInSequence.name} (索引: {originalIndex})");
                }
                else
                {
                    Debug.LogWarning($"无法找到索引为 {originalIndex} 的新 Sprite");
                }
            }
            else
            {
                Debug.LogWarning($"无法找到索引为 {originalIndex} 的原始 Sprite");
            }
        }
        
        // 应用修改后的关键帧
        if (hasReplacement)
        {
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
            EditorUtility.SetDirty(clip);
            AssetDatabase.SaveAssets();
            Debug.Log($"替换 Clip Sprite 完成: {clip.name} (替换了 {keyFrames.Length} 个关键帧)");
        }
        else
        {
            Debug.LogWarning($"未找到需要替换的 Sprite 引用: {clip.name}");
        }
    }
    
    /// <summary>
    /// 从同一个纹理中获取所有 Sprite
    /// </summary>
    private List<Sprite> GetSpritesFromSameTexture(Sprite sprite)
    {
        List<Sprite> sprites = new List<Sprite>();
        
        if (sprite == null)
            return sprites;
        
        // 获取 Sprite 的纹理路径
        string texturePath = AssetDatabase.GetAssetPath(sprite);
        if (string.IsNullOrEmpty(texturePath))
            return sprites;
        
        // 加载纹理中的所有 Sprite
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(texturePath);
        foreach (Object asset in assets)
        {
            if (asset is Sprite spriteAsset)
            {
                sprites.Add(spriteAsset);
            }
        }
        
        // 按索引排序，确保顺序一致
        sprites.Sort((a, b) =>
        {
            int indexA = ExtractSpriteIndex(a.name);
            int indexB = ExtractSpriteIndex(b.name);
            return indexA.CompareTo(indexB);
        });
        
        Debug.Log($"从纹理 {texturePath} 加载了 {sprites.Count} 个 Sprite");
        return sprites;
    }
    
    /// <summary>
    /// 从 Sprite 名称中提取数字索引
    /// 例如: "Character 1_0" -> 0, "role_grilwhiteT_1" -> 1
    /// </summary>
    private int ExtractSpriteIndex(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName))
            return -1;
        
        // 查找最后一个下划线后的数字
        int lastUnderscore = spriteName.LastIndexOf('_');
        if (lastUnderscore >= 0 && lastUnderscore < spriteName.Length - 1)
        {
            string indexStr = spriteName.Substring(lastUnderscore + 1);
            if (int.TryParse(indexStr, out int index))
            {
                return index;
            }
        }
        
        return -1;
    }
    
    
    
    #endregion
    
    #region Animator Override Controller 生成
    
    private void CreateAnimatorOverrideController(List<AnimationClip> processedClips, string targetFolder)
    {
        try
        {
            // 创建 Animator Override Controller
            AnimatorOverrideController overrideController = new AnimatorOverrideController(baseController);
            
            // 获取基础控制器中的所有动画状态
            AnimationClip[] originalClips = baseController.animationClips;
            if (originalClips == null || originalClips.Length == 0)
            {
                Debug.LogWarning("基础控制器中没有找到动画片段");
                return;
            }
            
            // 创建映射：原始动画名称 -> 新动画片段
            Dictionary<string, AnimationClip> clipMapping = new Dictionary<string, AnimationClip>();
            
            foreach (AnimationClip originalClip in originalClips)
            {
                if (originalClip == null) continue;
                
                // 查找对应的新动画片段
                AnimationClip matchingClip = processedClips.Find(clip =>
                    clip != null &&
                    Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(clip)) == originalClip.name);
                
                if (matchingClip != null)
                {
                    clipMapping[originalClip.name] = matchingClip;
                }
                else
                {
                    // 如果没有找到匹配的片段，使用原始片段
                    clipMapping[originalClip.name] = originalClip;
                }
            }
            
            // 设置覆盖控制器中的动画片段
            overrideController[""] = null; // 清除默认值
            foreach (var kvp in clipMapping)
            {
                overrideController[kvp.Key] = kvp.Value;
            }
            
            // 保存 Animator Override Controller
            string overrideControllerPath = Path.Combine(targetFolder, $"{newSprite.name}_OverrideController.overrideController");
            AssetDatabase.CreateAsset(overrideController, overrideControllerPath);
            
            Debug.Log($"已生成 Animator Override Controller: {overrideControllerPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"生成 Animator Override Controller 失败: {e.Message}");
        }
    }
    
    #endregion
}