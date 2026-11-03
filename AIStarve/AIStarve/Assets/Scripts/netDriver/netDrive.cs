using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Abelkhan;
using UnityEngine;

public class netDriver : MonoBehaviour
{
    public static string ServerHubName;
    public static scene_hub_info SceneHubInfo;

    public static string UserGuid;

    private static Client.Client _client;

    private static Abelkhan.login_caller _login_caller;
    public static Abelkhan.login_caller LoginCaller
    {
        get
        {
            return _login_caller;
        }
    }

    private static Abelkhan.bag_service_caller _bag_service_caller;
    public static Abelkhan.bag_service_caller BagCaller
    {
        get
        {
            return _bag_service_caller;
        }
    }

    private static Abelkhan.scene_mgr_caller _scene_mgr_caller;
    public static Abelkhan.scene_mgr_caller SceneMgrCaller
    {
        get
        {
            return _scene_mgr_caller;
        }
    }

    private static Abelkhan.scene_caller _scene_caller;
    public static Abelkhan.scene_caller SceneCaller
    {
        get
        {
            return _scene_caller;
        }
    }

    private static Abelkhan.scene_client_module _scene_client_Module;
    public static Abelkhan.scene_client_module SceneNtfModule
    {
        get
        {
            return _scene_client_Module;
        }
    }

    public static Task<Abelkhan.hub_info> GetHubInfo(string hubType)
    {
        var _t = new TaskCompletionSource<Abelkhan.hub_info>();

        Debug.Log("GetHubInfo GetHubInfo begin!");
        _client.get_hub_info(hubType, (_info) =>
        {
            Debug.Log("GetHubInfo GetHubInfo callback _info begin!");
            ServerHubName = _info.hub_name;
            Debug.Log("GetHubInfo GetHubInfo callback _info end!");
            _t.SetResult(_info);
        });

        return _t.Task;
    }

    [DllImport("__Internal")]
    private static extern void ReportSceneLoadedToJS();

    void Start()
    {
        Debug.Log("Start begin!");
        DontDestroyOnLoad(this);
        Debug.Log("DontDestroyOnLoad done!");

        _client = new Client.Client();
        _login_caller = new Abelkhan.login_caller(_client);
        _bag_service_caller = new Abelkhan.bag_service_caller(_client);
        _scene_caller = new Abelkhan.scene_caller(_client);
        _scene_mgr_caller = new scene_mgr_caller(_client);

        _scene_client_Module = new Abelkhan.scene_client_module(_client);
        netSceneModule.Init();

        _client.onGateConnect += () => {
            Debug.Log("ReportSceneLoadedToJS begin!");
#if UNITY_WEBGL && !UNITY_EDITOR
            ReportSceneLoadedToJS();
#endif
            Debug.Log("ReportSceneLoadedToJS end!");
        };
        _client.onGateConnectFaild += () => {
            Debug.Log("connect gate faild!");
        };
        _client.onHubConnect += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} sucessed!", hub_name));
        };
        _client.onHubConnectFaild += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} faild!", hub_name));
        };
        _client.connect_gate("wss://aistarve.ucat.games:3001", 3000);

        Debug.Log("Start done!");
    }

    // Update is called once per frame
    void Update()
    {
#if IS_TEST
        return;
#endif
        _client.poll();
    }
}
