using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumEffect : MonoBehaviour {
    public Image stateImage = null;
    public Image numImageOne = null;
    public Image numImageTwo = null;
    public Image numImageThree = null;
    // Use this for initialization
    void Start () {
        //numImageOne.gameObject.SetActive(false);
        //numImageTwo.gameObject.SetActive(false);
        //numImageThree.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetNumShowHaveState(int num)
    {
        string str = "result_fan_num_";
        int num0;
        int num1;
        int num2;

        numImageOne.gameObject.SetActive(false);
        numImageTwo.gameObject.SetActive(false);
        numImageThree.gameObject.SetActive(false);
        if (num>0)
        {
            stateImage.overrideSprite = Resources.Load("Number/add", typeof(Sprite)) as Sprite;
        }
        else
        {
            stateImage.overrideSprite = Resources.Load("Number/subtract", typeof(Sprite)) as Sprite;
        }
        num = Mathf.Abs(num);
        if (num<10)
        {
           numImageOne.gameObject.SetActive(true);
           numImageOne.overrideSprite = Resources.Load("Number/"+ str+num.ToString(), typeof(Sprite)) as Sprite;
        }
        else if (num<100)
        {
            num0 = num / 10;
            num1 = num % 10;
            numImageOne.gameObject.SetActive(true);
            numImageTwo.gameObject.SetActive(true);
            numImageOne.overrideSprite = Resources.Load("Number/" + str + num0.ToString(), typeof(Sprite)) as Sprite;
            numImageTwo.overrideSprite = Resources.Load("Number/" + str + num1.ToString(), typeof(Sprite)) as Sprite;
        }
        else
        {
            numImageOne.gameObject.SetActive(true);
            numImageTwo.gameObject.SetActive(true);
            numImageThree.gameObject.SetActive(true);
            num0 = num / 100;
            num1 = (num % 100)/10;
            num2 = (num % 100) % 10;
            numImageOne.overrideSprite = Resources.Load("Number/" + str + num0.ToString(), typeof(Sprite)) as Sprite;
            numImageTwo.overrideSprite = Resources.Load("Number/" + str + num1.ToString(), typeof(Sprite)) as Sprite;
            numImageThree.overrideSprite = Resources.Load("Number/" + str + num2.ToString(), typeof(Sprite)) as Sprite;
        }

        
    }
}
