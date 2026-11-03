using Abelkhan;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class netLogin
{
    public static async Task<List<scene_hub_info>> Login(string account)
    {
        Debug.Log("Login Login begin!");

        var _t = new TaskCompletionSource<List<scene_hub_info>>();

        netDriver.LoginCaller.get_hub(netDriver.ServerHubName).player_login_no_token(account).callBack((userGuid, list) =>
        {
            Debug.LogFormat($"player_login_no_token success userGuid:{userGuid}, list:{list.Count/*JsonConvert.SerializeObject(list)*/}");
            netDriver.UserGuid = userGuid;
            _t.SetResult(list);
        }, (err) =>
        {
            Debug.LogErrorFormat("player_login_no_token err:{0}", err);
            _t.SetResult(new List<scene_hub_info>());
        }).timeout(3000, () =>
        {
            Debug.LogErrorFormat("player_login_no_token timeout");
            _t.SetResult(new List<scene_hub_info>());
        });

        return await _t.Task;
    }

    public static IEnumerator RunAsyncEnumerable<T>(IAsyncEnumerable<T> source)
    {
        var enumerator = source.GetAsyncEnumerator();

        while (true)
        {
            var moveNextTask = enumerator.MoveNextAsync().AsTask();

            while (!moveNextTask.IsCompleted)
                yield return null;

            if (!moveNextTask.Result)
                break;
        }
    }


    public static async IAsyncEnumerable<bool> CombinedUserInfo(string jwtoken, Action<string, string> callback)
    {
        Debug.Log("CombinedUserInfo GetHubInfo begin!");
        var info = await netDriver.GetHubInfo("server");
        Debug.Log("CombinedUserInfo GetHubInfo info.IsCompleted!");

        Debug.LogFormat("CombinedUserInfo begin combined_user_info jwtoken:{0}", jwtoken);
        netDriver.LoginCaller.get_hub(netDriver.ServerHubName).combined_user_info(jwtoken).callBack((id, name) =>
        {
            Debug.LogFormat("CombinedUserInfo begin combined_user_info id:{0} name:{1}", id, name);
            callback(id, name);
        }, (err) =>
        {
            Debug.LogFormat("CombinedUserInfo err:{0}", err);
        }).timeout(30000, () =>
        {
            Debug.LogFormat("CombinedUserInfo timeout!");
        });

        yield return true;
    }
}
