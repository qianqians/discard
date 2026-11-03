using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

/// <summary>
/// 热更新管理器（支持增量更新）
/// </summary>
public class HotUpdateManager : MonoBehaviour
{
    private string serverURL = "http://your-server.com/assetbundles";
    private string localVersion;
    private Dictionary<string, string> localHashes = new Dictionary<string, string>();

    void Start()
    {
        StartCoroutine(CheckUpdate());
    }

    /// <summary>
    /// 检查并执行更新
    /// </summary>
    public IEnumerator CheckUpdate()
    {
        // 加载本地版本信息
        LoadLocalVersion();

        // 获取服务器版本信息
        yield return GetServerVersion();

        // 比较版本并下载更新
        yield return DownloadUpdates();
    }

    /// <summary>
    /// 加载本地版本信息
    /// </summary>
    private void LoadLocalVersion()
    {
        string versionPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles/version.txt");
        if (File.Exists(versionPath))
        {
            localVersion = File.ReadAllText(versionPath);
        }

        string hashesPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles/hashes.json");
        if (File.Exists(hashesPath))
        {
            localHashes = JsonUtility.FromJson<Dictionary<string, string>>(File.ReadAllText(hashesPath));
        }
    }

    /// <summary>
    /// 获取服务器版本信息
    /// </summary>
    private IEnumerator GetServerVersion()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{serverURL}/version.txt"))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string serverVersion = request.downloadHandler.text;
                if (serverVersion != localVersion)
                {
                    Debug.Log($"发现新版本：{serverVersion}");
                }
                else
                {
                    Debug.Log("当前已是最新版本");
                }
            }
        }
    }

    /// <summary>
    /// 下载更新文件
    /// </summary>
    private IEnumerator DownloadUpdates()
    {
        // 获取服务器hash表
        Dictionary<string, string> serverHashes;
        using (UnityWebRequest request = UnityWebRequest.Get($"{serverURL}/hashes.json"))
        {
            yield return request.SendWebRequest();
            serverHashes = JsonUtility.FromJson<Dictionary<string, string>>(request.downloadHandler.text);
        }

        // 比较差异文件
        var filesToUpdate = new List<string>();
        foreach (var entry in serverHashes)
        {
            if (!localHashes.ContainsKey(entry.Key) || localHashes[entry.Key] != entry.Value)
            {
                filesToUpdate.Add(entry.Key);
            }
        }

        // 下载差异文件
        foreach (var file in filesToUpdate)
        {
            string url = $"{serverURL}/{file}";
            string savePath = Path.Combine(Application.persistentDataPath, "AssetBundles", file);
            
            Debug.Log($"正在下载：{file}");
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, request.downloadHandler.data);
                    Debug.Log($"下载完成：{file}");
                }
            }
        }

        if (filesToUpdate.Count > 0)
        {
            Debug.Log("热更新完成");
        }
    }
}