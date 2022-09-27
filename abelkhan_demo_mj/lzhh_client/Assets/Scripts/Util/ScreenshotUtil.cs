using System.Collections;
using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class ScreenshotUtil : MonoBehaviour {


    private string url = "/onMobileSavedScreen.png";
    private string ScreenShotpath;
    // Use this for initialization
    void Start () {
        ScreenShotpath = Application.persistentDataPath + "/ImageCache/";
    }

    public void BeginShot(Action callback)
    {
        StartCoroutine(UploadPNG(callback));
    }

    //开启一个携程进行截屏
    IEnumerator UploadPNG(Action callback)
    {
        //等待帧结束
        yield return new WaitForEndOfFrame();
        //获取到屏幕的宽高，进行全屏截图
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        //将图片转成byte数据
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);
        File.WriteAllBytes(ScreenShotpath + url, bytes);

        callback();
        //利用post方法将图片数据进行上传
        //WWWForm form = new WWWForm();
        //form.AddField("frameCount", Time.frameCount.ToString());
        //form.AddBinaryData("fileUpload", bytes);
        //WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
        //yield return w;
        //if (w.error != null)
        //    print(w.error);
        //else
        //    print("Finished Uploading Screenshot");
    }


    public void GetText(Action<Texture2D> callback)
    {
        StartCoroutine(GetPic(callback));
    }
    public IEnumerator GetPic(Action<Texture2D> callback)
    {
       // Texture2D tex;
        WWW wwwTexture = new WWW(ScreenShotpath+url);
        yield return wwwTexture;
        //tex = www.texture;
        if (wwwTexture.error != null)
        {
            Debug.Log("ScreenShotpatherro");
        }
        else
        {
            callback(wwwTexture.texture);
        }
    }
    // Update is called once per frame
    void Update ()
    {
		
	 }
   
}
