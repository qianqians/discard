using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;

class CardUnit //需要在资源池里面管理的单元
{
    public mjCards CardType;
    public GameObject[]CardSameType;//保存同一种牌的数量
    List<int> HasUseNums;    
    public void initial(GameObject Card, mjCards parmCardType, int num)
    {
        CardType = parmCardType;
        HasUseNums = new List<int>();
        CardSameType = new GameObject[num];
        if (num <= 0)
        {
            return;
        }
        for( int i = 0; i < num; i++ )
        {
            CardSameType[i] = GameObject.Instantiate(Card);
          //  SetLaiziState(CardSameType[i],false);
            CardData cardData = CardSameType[i].GetComponent<CardData>();
            cardData.CardType = parmCardType;
        }
    }

    private void SetLaiziState(GameObject parmGameObject, bool flag)
    {
        string tabName = "lai";
        parmGameObject.transform.Find(tabName).gameObject.SetActive(flag);
    }

    public GameObject GetCard()
    {
        GameObject mj;
        for( int i = 0; i < CardSameType.Length; i++)
        {
            if (!HasUseNums.Contains(i))
            {
                HasUseNums.Add(i);
                CardSameType[i].SetActive(true); 
                 mj = CardSameType[i];
             //   SetLaiziState(mj, true);
                return mj;
            }
        }
        return null;
    }

    public void ReleaseCard(GameObject GameObjectParm)
    {
        for (int i = CardSameType.Length-1; i >=0; i--)
        {
            if (CardSameType[i] == GameObjectParm)
            {
                GameObjectParm.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                GameObjectParm.transform.rotation = Quaternion.identity;
                CardData cardData = GameObjectParm.GetComponent<CardData>();
                cardData.Reset();
                GameObjectParm.SetActive(false);
                GameObjectParm.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color = TableController.Instance.mjBackColor;
                // SetLaiziState(GameObjectParm, false);
                if (HasUseNums.Contains(i))
                {
                    HasUseNums.Remove(i);
                    return;
                }
            }
        }
    }

    public void ReleaseCard(int index)
    {
        if (HasUseNums.Contains(index))
        {
            HasUseNums.RemoveAt(index);
        }
    }
}

public class CardPoolManager {
   // public List<GameObject> laiziList;
    public static int CardNumForType = 4;//同一种牌的数量
    GameObject[] CardsType;
    CardUnit[] Cardunits;
    private int laiZi;
    private int laiZiPi;

    private void ChangeComponentTexture(GameObject parmGameObject, string TabName, string SourceName)
    {       
        CardData cardData = parmGameObject.GetComponent<CardData>();
        parmGameObject.transform.Find(TabName).GetComponent<MeshRenderer>().materials[0].mainTexture = Resources.Load(SourceName) as Texture2D; 
      //  parmGameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = Resources.Load(SourceName) as Texture2D;
    }

    private void LoadFaxian(GameObject parmGameObject, string TabName,mjCards mj)
    {
        string res;
        int num;
        if (mj != mjCards.Nodefine)
        {
            num = (int)mj;
            res = "InGame/Texture/" + num.ToString() + "n";
            parmGameObject.transform.Find(TabName).GetComponent<MeshRenderer>().materials[0].SetTexture("_BumpMap", Resources.Load(res) as Texture2D);
            parmGameObject.transform.Find(TabName).GetComponent<MeshRenderer>().materials[0].EnableKeyword("_NORMALMAP");
                //SetTexture("_BumpMap", Resources.Load(res) as Texture2D);
        }
    }
    public void SetLai(int laizi)
    {
        laiZi = laizi;
        laiZiPi = laiZi-1;     
        if (laiZiPi % 10 == 0)
        {
            laiZiPi = laiZi + 8;
        }
    }

    public void LoadAllCard()//先加载所有类型的牌
    {
        CardsType = new GameObject[(int)mjCards.tiao_Num];
        Cardunits = new CardUnit[(int)mjCards.tiao_Num];
        for (int i = 0; i < (int)mjCards.tiao_Num; i++)
        {

            CardsType[i] = (GameObject)GameObject.Instantiate(Resources.Load("InGame/Card"));
       //     LoadFaxian(CardsType[i], "front", (mjCards)i);
            switch ((mjCards)i)
            {
                case mjCards.wan_1:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_1");
                    break;
                case mjCards.wan_2:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_2");
                    break;
                case mjCards.wan_3:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_3");
                    break;
                case mjCards.wan_4:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_4");
                    break;
                case mjCards.wan_5:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_5");
                    break;
                case mjCards.wan_6:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_6");
                    break;
                case mjCards.wan_7:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_7");
                    break;
                case mjCards.wan_8:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_8");
                    break;
                case mjCards.wan_9:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/wan_9");
                    break;
                case mjCards.tong_1:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_1");
                    break;
                case mjCards.tong_2:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_2");
                    break;
                case mjCards.tong_3:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_3");
                    break;
                case mjCards.tong_4:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_4");
                    break;
                case mjCards.tong_5:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_5");
                    break;
                case mjCards.tong_6:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_6");
                    break;
                case mjCards.tong_7:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_7");
                    break;
                case mjCards.tong_8:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_8");
                    break;
                case mjCards.tong_9:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tong_9");
                    break;
                case mjCards.tiao_1:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_1");
                    break;
                case mjCards.tiao_2:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_2");
                    break;
                case mjCards.tiao_3:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_3");
                    break;
                case mjCards.tiao_4:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_4");
                    break;
                case mjCards.tiao_5:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_5");
                    break;
                case mjCards.tiao_6:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_6");
                    break;
                case mjCards.tiao_7:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_7");
                    break;
                case mjCards.tiao_8:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_8");
                    break;
                case mjCards.tiao_9:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/tiao_9");
                    break;
                default:
                    ChangeComponentTexture(CardsType[i], "front", "InGame/Texture/w_1");
                    break;
            }         
        }
        TableController.Instance.mjBackColor = CardsType[(int)mjCards.tiao_1].transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color;
        // parmGameObject.GetComponent<MeshRenderer>().materials[0].mainTexture
        //  TableController.Instance.mjBackColor = CardsType[(int)mjCards.tiao_1].gameObject.GetComponent<Renderer>().materials[0].color;
        InstanceLoadAllCard();
    }
    private void InstanceLoadAllCard()//创建所有牌
    {
        for (int i = 0; i < (int)mjCards.tiao_Num; i++)
        {
            int initionalNum = 0;
            if( (mjCards)i == mjCards.Nodefine)
            {
                initionalNum = 108;
            }
            else
            {
                initionalNum = 4;
            }
            CardUnit cardUnit = new CardUnit();
            cardUnit.initial(CardsType[i], (mjCards)i, initionalNum);
            Cardunits[i] = cardUnit;
        }
    }

    public void SetMjBackImg(string index)
    {
        CardUnit tempCardArr;
        GameObject[] mjObjectArr;
        GameObject mj;
        Texture2D texture;
        string res = "InGame/Texture/back_Box001_lambert1_AlbedoTransparency" + index;
        texture = Resources.Load(res) as Texture2D;
        for (int i = 0; i < Cardunits.Length; i++)
        {
            tempCardArr = Cardunits[i];
            mjObjectArr = tempCardArr.CardSameType;
            for (int j = 0; j < mjObjectArr.Length; j++)
            {
                mj = mjObjectArr[j];
                mj.transform.Find("back").GetComponent<MeshRenderer>().materials[0].mainTexture = texture;
               //  mj.GetComponent<MeshRenderer>().materials[0].mainTexture = texture;
            }
        }
    }

    public GameObject GetGameObjectByType(mjCards parmCardType)
    {
        GameObject mj;
        string res;
        if ((int)parmCardType > Cardunits.Length)
        {
            return null;
        }
        //if (parmCardType == mjCards.tiao_4)
        //{
        //    Debug.Log("");
        //}
        mj = Cardunits[(int)parmCardType].GetCard();
        if (parmCardType != mjCards.Nodefine)
        {
            if ((int)parmCardType == laiZi)
            {
                res = "InGame/Texture/lai";
                SetLaiziState(mj, true, res, RenderingMode.Transparent);
            }
            else if ((int)parmCardType == laiZiPi)
            {
                res = "InGame/Texture/laiziPi";
                SetLaiziState(mj, true, res, RenderingMode.Transparent);
            }
            else
            {
                SetLaiziState(mj, false, "", RenderingMode.Transparent);
            }
        }
        else
        {
             SetLaiziState(mj, false, "", RenderingMode.Transparent);            
        }               
        return mj;
    }

    public void ReleaseByGameObject(mjCards parmCardType, GameObject gameObjectParm )
    {
        Cardunits[(int)parmCardType].ReleaseCard(gameObjectParm);       
    }

    public void SetLaiShow(mjCards laizi)
    {
        CardUnit cardUnit;
        cardUnit = Cardunits[(int)laizi];
        GameObject[] CardSameType = cardUnit.CardSameType;
        GameObject mj;
        for (int i = 0; i < CardSameType.Length; i++)
        {
            mj = CardSameType[i];
           // SetLaiziState(mj,true);
        }
    }

    private void SetLaiziState(GameObject parmGameObject,bool flag,string SourceName, RenderingMode renderingMode)
    {
        string tabName = "lai";
        GameObject lai;
        GameObject qian;
        if (parmGameObject != null)
        {
            lai = parmGameObject.transform.Find(tabName).gameObject;
            qian = parmGameObject.transform.Find("front").gameObject;
            Color changeColor = new Color();
            float a = 14f;
            float b = 143f;
            float c = 6f;
            Color.RGBToHSV(changeColor, out a, out b,out c);
          
            lai.SetActive(flag);
            if (SourceName != "")
            {
                if (SourceName == "InGame/Texture/lai")
                {
                    qian.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else
                {
                    qian.GetComponent<Renderer>().material.color = Color.white;
                }
                lai.GetComponent<MeshRenderer>().materials[0].mainTexture = Resources.Load(SourceName) as Texture2D;
            }
            else
            {
                qian.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            Debug.Log("m麻将有空吗"+ SourceName);
        }      
    }

    public Color32 parseHexStr2Byte(string hexStr)
    {
        byte a = byte.Parse(hexStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hexStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte c = byte.Parse(hexStr.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Color mm = new Color();
        //mm = Color.HSVToRGB(a, b, c);
        return new Color32(a,b,c,225);
    }

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
