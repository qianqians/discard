using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
public class RedBagScript : MonoBehaviour {

    public Text slogan = null;
    public string red_id;
	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            //  RobRedBag
            SocketClient.Instance.RobRedBag(red_id);
        });
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetRedBagState(int state)
    {
        List<string> resArr = new List<string> {"meichaihongbao" , "chakaihongbao"};
        Image img = this.gameObject.GetComponent<Image>();
        img.overrideSprite = Resources.Load("Sprite/redBag/" + resArr[state], typeof(Sprite)) as Sprite;
    }
}
