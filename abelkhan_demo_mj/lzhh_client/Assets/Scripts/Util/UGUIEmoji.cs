using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUIEmoji : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick(GameObject obj)
    {
        obj.GetComponent<UGUISpriteAnimation>().Rewind();
        obj.GetComponent<UGUISpriteAnimation>().Play();
    }
}
