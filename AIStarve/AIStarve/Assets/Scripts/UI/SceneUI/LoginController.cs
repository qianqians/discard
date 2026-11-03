using Abelkhan;
using Newtonsoft.Json.Linq;
using SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public Text titleText;
    public GameObject nameGObject;
    public GameObject passwordGObject;
    public Button okButton;

    private TMP_InputField nameInput = null;
    private TMP_InputField passwordInput = null;

    void Start()
    {
        if (titleText != null)
            titleText.text = "Login";
        
        if (nameGObject != null)
            nameInput = nameGObject.GetComponentInChildren<TMP_InputField>();
        
        if (passwordGObject != null)
            passwordInput = passwordGObject.GetComponentInChildren<TMP_InputField>();
    }

    [DllImport("__Internal")]
    private static extern void NtfPlayerGuidToJS(string self_id);

    public async Task OnLoginButtonClicked(string id, string name)
    {
        Debug.LogFormat("OnLoginButtonClicked Login begin! id:{0}, name:{1}", id, name);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Field");
        // 隐藏登录面板
        if (CubeRotate.Instance != null && CubeRotate.Instance.panelLogin != null)
        {
            CubeRotate.Instance.panelLogin.SetActive(false);
        }
#if UNITY_WEBGL && !UNITY_EDITOR
        NtfPlayerGuidToJS(id);
#endif

        //string nameInputString = nameInput.text.ToString();
        var sceneList = await netLogin.Login(id);//new List<int>(); 
        Debug.LogFormat($"OnLoginButtonClicked Login Result! sceneListWait:{sceneList.Count}");
        Debug.Log("OnLoginButtonClicked Login end!");

        if (sceneList.Count > 0)
        {
            Debug.Log("OnLoginButtonClicked IntoScene begin!");
            await netSceneMgr.IntoScene(name, sceneList[0]);
            Debug.Log("OnLoginButtonClicked IntoScene end!");

        }
        else
        {
            Debug.Log("OnLoginButtonClicked CreateScene begin!");
            await netSceneMgr.CreateScene(name, "Field");
            Debug.Log("OnLoginButtonClicked CreateScene end!");
        }
    }

    public async IAsyncEnumerable<bool> OnLoginWithoutWeb(string name)
    {
        Debug.Log("CombinedUserInfo GetHubInfo begin!");
        await netDriver.GetHubInfo("server");
        Debug.Log("CombinedUserInfo GetHubInfo info.IsCompleted!");
        
        await OnLoginButtonClicked(name, name);

        yield return true;
    }


    public  void OnUIButtonClicked(string name)
    {
        StartCoroutine(netLogin.RunAsyncEnumerable(OnLoginWithoutWeb(name)));
    }
}