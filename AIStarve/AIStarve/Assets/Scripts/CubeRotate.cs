using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneManagement;

public class CubeRotate : MonoBehaviour
{
    private static CubeRotate _instance;
    public static CubeRotate Instance => _instance;
    public TMP_InputField tx;
    public GameObject panelLogin;

    private void Start()
    {
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

    }
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 30, Space.World);
    }

    public void ChatMsg(string id_msg)
    {
        string[] id_msg_split = id_msg.Split('|');
    }

    public void LoginJwtoken(string _jwtoken)
    {
        Debug.LogFormat("LoginJwtoken begin jwtoken:{0}", _jwtoken);
        StartCoroutine(netLogin.RunAsyncEnumerable(netLogin.CombinedUserInfo(_jwtoken, SetTextInfo)));
    }

    public async void SetTextInfo(string id, string name)
    {
        // 查找PanelLogin对象并调用LoginController的OnLoginButtonClicked方法
        if (panelLogin != null)
        {
            LoginController loginController = panelLogin.GetComponent<LoginController>();
            if (loginController != null)
            {
                Debug.Log("已调用PanelLogin对象的LoginController.OnLoginButtonClicked() begin!");
                await loginController.OnLoginButtonClicked(id, name);
                Debug.Log("已调用PanelLogin对象的LoginController.OnLoginButtonClicked() end");
            }
            else
            {
                Debug.LogWarning("PanelLogin对象上未找到LoginController组件");
            }
        }
        else
        {
            Debug.LogWarning("未找到名为PanelLogin的GameObject");
        }
    }
    public void AddScale()
    {
        transform.localScale += Vector3.one * 0.1f;
    }
    public void SubtractScale()
    {
        transform.localScale -= Vector3.one * 0.1f;
    }

    // 从JavaScript调用获取场景信息
    public void GetSceneInfoFromJS(string uuid)
    {
        Debug.Log($"GetSceneInfoFromJS called with uuid: {uuid}");
        
        // 获取角色信息并通过JavaScript回调返回
        string characterInfo = GetCharacterInfoAsJson(uuid);
        
        // 通过Application.ExternalEval调用JavaScript回调函数返回结果
        Application.ExternalEval($"window.ReceiveSceneInfo('{characterInfo.Replace("'", "\\'")}')");
    }

    // 获取角色信息并返回JSON格式
    private string GetCharacterInfoAsJson(string uuid)
    {
        try
        {
            // 检查GameSceneManager实例是否存在
            if (GameSceneManager.Instance == null)
            {
                Debug.LogWarning("GameSceneManager instance is null");
                return "{\"error\": \"GameSceneManager not available\"}";
            }

            // 获取指定角色
            Character character = GameSceneManager.Instance.GetCharacter(uuid);
            
            if (character == null)
            {
                // 如果找不到指定角色，返回所有角色列表
                List<string> allCharacterIds = GameSceneManager.Instance.GetAllCharacterIds();
                
                // 手动构建JSON字符串，因为Unity的JsonUtility不支持匿名类型
                string json = $"{{\"requestedUuid\":\"{uuid}\",\"characterFound\":false,\"availableCharacters\":[{string.Join(",", allCharacterIds.Select(id => $"\"{id}\""))}],\"timestamp\":\"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\"}}";
                Debug.Log($"Generated character list JSON: {json}");
                return json;
            }
            else
            {
                // 返回指定角色的详细信息
                Vector3 worldPosition = character.Position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                // 获取屏幕尺寸
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
                string json = $"{{\"uuid\":\"{uuid}\",\"characterId\":\"{character.CharacterId}\",\"prefabPath\":\"{character.PrefabPath}\",\"worldPosition\":{{\"x\":{worldPosition.x},\"y\":{worldPosition.y},\"z\":{worldPosition.z}}},\"screenPosition\":{{\"x\":{screenPosition.x},\"y\":{screenPosition.y},\"z\":{screenPosition.z}}},\"screenSize\":{{\"width\":{screenWidth},\"height\":{screenHeight}}},\"timestamp\":\"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\"}}";
                Debug.Log($"Generated character info JSON: {json}");
                return json;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error getting character info: {ex.Message}");
            return $"{{\"error\": \"{ex.Message}\"}}";
        }
    }

    // 处理从JavaScript返回的场景信息
    public void HandleSceneInfo(string characterInfo)
    {
        Debug.Log($"Received character info: {characterInfo}");
        
        // 这里可以处理从GameSceneManager获取的角色信息
        // 例如：显示在UI上或进行其他处理
    }
}