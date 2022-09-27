using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CD : MonoBehaviour{

    public Text TimeText0;
    public Text TimeText1;
    public InputField InputTime;
    public Button ButtonS;

    public Image numImage0 = null;
    public Image numImage1 = null;
    //默认时间60s
    int time = 60;
    int minute = 0;
    int second = 0;
    // Use this for initialization
    void Start()
    {
        //执行协程
        StartCoroutine(CountI());     
        ButtonS.onClick.AddListener(delegate ()
        {
            butClik();
        });
    }

    // Update is called once per frame
    void Update()
    {


    }
    /// <summary>
    /// 转换字符串
    /// </summary>
    void butClik()
    {
        string inputTime = InputTime.transform.FindChild("Text").gameObject.GetComponent<Text>().text;
        StopCoroutine(CountI());
        try
        {
            time = int.Parse(inputTime);
           
        }
        catch
        {
            try
            {         
                    string minStr = "";
                    string secStr = "";
                    int sign = 100;
                    char[] timeList = inputTime.ToCharArray();
                    for (int i = 0; i < timeList.Length; i++)
                    {
                        if (timeList[i] + "" == ":")
                        {
                            sign = i;
                            continue;
                        }
                        if (sign > i)
                        {
                            minStr += timeList[i] + "";
                        }
                        else if (sign < i)
                        {
                            secStr += timeList[i] + "";
                        }
                    }

                    minute = int.Parse(minStr);
                    second = int.Parse(secStr);
                    time = minute * 60 + second;
                    
            }
            catch
            {
                TimeText0.GetComponent<Text>().text = "格式";
                TimeText1.GetComponent<Text>().text = "错误";
            }     
        }
        if (TimeText1.GetComponent<Text>().text == "结束")
        {
            StartCoroutine(CountI());
        }     
    }

    /// <summary>
    /// 时间倒数
    /// </summary>
    /// <returns></returns>
    IEnumerator CountI()
    {
        while (time > 0)
        {
            time--;
            minute = time / 60;
            second = time - (minute * 60);
            TimeText0.GetComponent<Text>().text = minute + "";
            TimeText1.GetComponent<Text>().text = second + "";
            ShowSecondByImage(second);
            yield return new WaitForSeconds(1);
        }
        TimeOver();
    }

    private void ShowSecondByImage(int num)
    {
        int num0 = num % 10;
        int num1 = num / 10;
        numImage0.overrideSprite = Resources.Load("Number/result_fan_num_" + num0.ToString(), typeof(Sprite)) as Sprite;
        numImage1.overrideSprite = Resources.Load("Number/result_fan_num_" + num1.ToString(), typeof(Sprite)) as Sprite;
    }

    void TimeOver()
    {
        TimeText0.GetComponent<Text>().text = "时间";
        TimeText1.GetComponent<Text>().text = "结束";
    }
}

