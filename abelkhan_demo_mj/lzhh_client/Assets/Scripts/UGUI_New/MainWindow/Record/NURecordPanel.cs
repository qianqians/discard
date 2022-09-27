using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NURecordPanel : MonoBehaviour {

    public GameObject container = null;
	// Use this for initialization
	void Start () {
        GameObject item;
        string[] keysArr = new string[] { "record0", "record1", "record2" };
        string[] imgResKeysArr = new string[] { "recordimg0", "recordimg1", "recordimg2" };
        object[] arr;
        string str;
        string res;
      //  List<AccountData> record;
       // string time;
        RecordItem code;
        GameObject prb = Resources.Load("UIPrefab/RecordGrid") as GameObject;
      //  item = GameObject.Instantiate();
        for (int i = keysArr.Length-1; i >=0 ; i--)
        {
            if (PlayerPrefs.HasKey(keysArr[i]))
            {
                str = PlayerPrefs.GetString(keysArr[i]);
                arr = WarRecordAccessTool.StoreDateforString(str);
                res = PlayerPrefs.GetString(imgResKeysArr[i]);
                item = GameObject.Instantiate(prb);
                item.transform.SetParent(container.transform, false);
                code = item.GetComponent<RecordItem>();
                code.Show(arr, res);
            }
        }   
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
