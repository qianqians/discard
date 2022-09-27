using System.Collections;
using TinyFrameWork;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
public class PictureModel
{
    public enum loadType
    {
        noLoad,
        loading,
        loadComplete
    }

    public string url;   //存放下载地址  
    public Texture2D texture2D;  //存放下载后的图片  
    Texture2D defaultTexture2D;  //存放下载不成功的默认图片  
    public loadType isDone = loadType.noLoad;   //标志一个图片是否已经下载完成  
    public Action<Texture2D, int> callBack;
    public int parameter;
    //public List<Action<Texture2D, int>> changePicHandlerList = new List<Action<Texture2D, int>>();
}

/// <summary>
/// 
/// </summary>
public class LoadingPicModel
{
    public int parameter;
    public string url;
    public Action<Texture2D, int> callBack;
}

public class HttpUtil : MonoBehaviour {
    private List<PictureModel> picModelList;
 //   private Hashtable _allBackFunHashMap;
  //  private Hashtable _texturePool;

    // private bool loadingFlag;
    private List<LoadingPicModel> _loadingList;//如果一个图片还没加载完成，又有请求的话就缓存在这里
    void Start () {
      //  _allBackFunHashMap = new Hashtable();
      //  _texturePool = new Hashtable();
        picModelList = new List<PictureModel>();
        _loadingList = new List<LoadingPicModel>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (loadingFlag)
        //{

        //}
	}

    //下载图片   
    public void DownloadPicture(string res, Action<Texture2D,int> action, int param=-1)
    {
        PictureModel model;
        bool flag = false;
        string picName;
        if (res != "")
        {
            picName = Regex.Unescape(res);
            for (int i = 0; i < picModelList.Count; i++)
            {
                model = picModelList[i];
                if (model.url == picName)
                {
                    flag = true;
                    if (model.isDone == PictureModel.loadType.loadComplete)
                    {
                        model.parameter = param;
                        action(model.texture2D, model.parameter);
                    }
                    if (model.isDone == PictureModel.loadType.loading)
                    {
                        LoadingPicModel loadMod = new LoadingPicModel();
                        loadMod.url = res;
                        loadMod.callBack = action;
                        loadMod.parameter = param;
                        _loadingList.Add(loadMod);
                    }
                    break;
                }
            }
            if (!flag)
            {
                model = new PictureModel();
                model.url = picName;
                model.callBack = action;
                model.parameter = param;
                picModelList.Add(model);
            }
            LoadingNext();
        }
        else
        {
            Debug.Log("picName为空");
        }
    }

    private void LoadingNext()
    {
        if (CheckIsHavePicLoading())
        {
            PictureModel model;
            for (int i = 0; i < picModelList.Count; i++)
            {
                model = picModelList[i];
                if (model.isDone == PictureModel.loadType.noLoad)
                {
                    model.isDone = PictureModel.loadType.loading;
                    StartCoroutine(GETTexture(model));
                    break;
                }
            }         
        }
    }

    private bool CheckIsHavePicLoading()
    {
        PictureModel model;
        bool flag = false;
        bool isHavePicNoLoad = false;
        for (int i = 0; i < picModelList.Count; i++)
        {
            model = picModelList[i];
            if (model.isDone == PictureModel.loadType.loading)
            {
                return false;
            }

            if (model.isDone == PictureModel.loadType.noLoad)
            {
                isHavePicNoLoad = true;
            }          
        }
        if (isHavePicNoLoad)
        {
            return true;
        }
        return flag;
    }
  
    IEnumerator GETTexture(PictureModel model)
    {
        // WWW wwwTexture = new WWW(model.url);
        UnityWebRequest wwwTexture = UnityWebRequest.Get(model.url);

        //yield return wwwTexture;
        yield return wwwTexture.Send();

        model.isDone = PictureModel.loadType.loadComplete;
        LoadingPicModel loadMod;
        if (wwwTexture.error != null)
        {
            //GET请求失败  
            //Debug.Log("wwwerror :" + picURL);
            //if (picURL =="")
            //{
            //    Debug.Log("wwwerror : url为空");
            //}
            //EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, picURL+"错误");
        }
        else
        {
            Texture2D tex = new Texture2D(256, 256);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadImage(wwwTexture.downloadHandler.data);

            model.callBack(tex, model.parameter);
        //    byte[] results = wwwTexture.downloadHandler.data;
            model.texture2D = tex;
            LoadingNext();
            for (int i = _loadingList.Count -1; i >=0; i--)
            {
                loadMod = _loadingList[i];
                if (loadMod.url == model.url)
                {
                    loadMod.callBack(tex, loadMod.parameter);
                }
                _loadingList.RemoveAt(i);
            }
        }
    }
}
