using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
public class EmojiPoolManager
{
    public List<GameObject> movieClipList;
   private Hashtable movieClipContainer;
    public void Init()
    {
        //movieClipList = new List<GameObject>();
        movieClipContainer = new Hashtable();
    }

    public GameObject GetGameObjectByRes(string str)
    {
        GameObject temp;
        string res;
        if (movieClipContainer.ContainsKey(str))
        {
            temp = movieClipContainer[str] as GameObject;
        }
        else
        {    
            res = "UIPrefab/"+str;
            GameObject prefab = (GameObject)Resources.Load(res);
            movieClipContainer[str] = prefab;
            temp = prefab;
        }
        return temp;
    }
}

