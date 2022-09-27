using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine;
using System;
public class InteracitveItem : MonoBehaviour {

    public InteractivePrompt attrs;
    public Int64 cardID =0;
    public Action callback;
	// Use this for initialization
	void Start () {
		this.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (TableController.Instance.isCanPeng && attrs == InteractivePrompt.guo)
            {
                TableController.Instance.canNotPengMjList.Add(TableController.Instance.currentCardID);
                //if (canNotPengMjList)
                //{

                //}
            }
            SocketClient.Instance.SendInteractive(attrs.ToString(),cardID);
            TableController.Instance.interPanelisShow = false;
            callback();
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Destroy()
    {
        callback = null;
    }
}
