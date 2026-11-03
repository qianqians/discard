using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System;

/// <summary>
/// AssetBundle打包工具类（支持增量更新）
/// </summary>
public class AssetBundleBuilder
{
    /// <summary>
    /// 菜单项：打包所有AssetBundles
    /// </summary>
    [MenuItem("Tools/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        var builder = new AssetBundleBuilder();
        builder.BuildAll();
    }

    /// <summary>
    /// 执行所有Prefab的AssetBundle打包
    /// </summary>
    public void BuildAll()
    {
        string outputPath = GetOutputPath();
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var prefabs = CollectAllPrefabs();
        foreach (var prefab in prefabs)
        {
            BuildPrefabBundle(prefab);
        }

        // 生成版本文件和hash表
        string version = GenerateVersion();
        File.WriteAllText(Path.Combine(outputPath, "version.txt"), version);
        
        var hashes = GenerateFileHashes(outputPath);
        File.WriteAllText(Path.Combine(outputPath, "hashes.json"), 
                         JsonUtility.ToJson(hashes));

        AssetDatabase.Refresh();
        Debug.Log($"AssetBundle打包完成，版本号：{version}");
    }

    /// <summary>
    /// 生成文件hash表
    /// </summary>
    private Dictionary<string, string> GenerateFileHashes(string path)
    {
        var hashes = new Dictionary<string, string>();
        foreach (var file in Directory.GetFiles(path).Where(f => !f.EndsWith(".meta")))
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    hashes[Path.GetFileName(file)] = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
        return hashes;
    }

    /// <summary>
    /// 生成版本号
    /// </summary>
    private string GenerateVersion()
    {
        return DateTime.Now.ToString("yyyyMMddHHmm");
    }

    /// <summary>
    /// 获取输出路径
    /// </summary>
    private string GetOutputPath()
    {
        return Path.Combine(Application.streamingAssetsPath, "AssetBundles");
    }

    /// <summary>
    /// 收集Prefabs目录下所有Prefab资源
    /// </summary>
    private GameObject[] CollectAllPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        List<GameObject> prefabs = new List<GameObject>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        }
        return prefabs.ToArray();
    }

    /// <summary>
    /// 构建单个Prefab的AssetBundle
    /// </summary>
    private void BuildPrefabBundle(GameObject prefab)
    {
        string bundleName = GetBundleName(prefab);
        UnityEngine.Object[] dependencies = AnalyzeDependencies(prefab);
        CreateAssetBundle(bundleName, dependencies);
    }

    /// <summary>
    /// 分析Prefab的所有依赖资源
    /// </summary>
    private UnityEngine.Object[] AnalyzeDependencies(GameObject prefab)
    {
        return EditorUtility.CollectDependencies(new UnityEngine.Object[] { prefab });
    }

    /// <summary>
    /// 创建AssetBundle文件
    /// </summary>
    private void CreateAssetBundle(string bundleName, UnityEngine.Object[] assets)
    {
        var buildMap = new AssetBundleBuild[] {
            new AssetBundleBuild {
                assetBundleName = bundleName,
                assetNames = assets.Select(a => AssetDatabase.GetAssetPath(a)).ToArray()
            }
        };

        BuildPipeline.BuildAssetBundles(GetOutputPath(), 
            buildMap, 
            BuildAssetBundleOptions.None, 
            EditorUserBuildSettings.activeBuildTarget);
    }

    /// <summary>
    /// 生成AssetBundle名称
    /// </summary>
    private string GetBundleName(GameObject prefab)
    {
        return $"prefab_{prefab.name.ToLower()}";
    }
}