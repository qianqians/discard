using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 出牌规则
/// </summary>
public class CardRules
{
    /// <summary>
    /// 卡牌数组排序
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static void SortCards(List<GameObject> cards, bool ascending)
    {
        cards.Sort(
            (GameObject a, GameObject b) =>
            {             
                if (!ascending)
                {
                    //先按照权重降序，再按花色升序
                    return -a.GetComponent<CardSprite>().Poker.GetCardSuit.CompareTo(b.GetComponent<CardSprite>().Poker.GetCardSuit) * 2 +
                         a.GetComponent<CardSprite>().Poker.GetCardWeight.CompareTo(b.GetComponent<CardSprite>().Poker.GetCardWeight);
                }
                else
                    //按照权重升序
                    return a.GetComponent<CardSprite>().Poker.GetCardWeight.CompareTo(b.GetComponent<CardSprite>().Poker.GetCardWeight);
            }
        );
    }

    
    /// <summary>
    /// 判断是否可以碰和杆
    ///（返回的值为2表示两个相同，3表示三个相同）
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static int PopEnable(List<GameObject> cards)
    {     
        int count = 0;
        foreach (GameObject item in cards)
        {
            if (item.GetComponent<CardSprite>().Poker.GetCardName == OrderController.Instance.CurrentPutCard.GetComponent<CardSprite>().Poker.GetCardName)
            {
                count++;
            }
        }
        return count;
    }
    /// <summary>
    /// hashtable:"name","flag"
    /// </summary>
    /// <param name="list"></param>
    /// <param name="kvp"></param>
    /// <returns></returns>
    public bool IsCanPeng(ArrayList list , Hashtable kvp)
    {
        Int64 count = 0;
        bool flag = false;
        foreach (Hashtable item in list)
        {
            if (item["name"] == kvp["name"])
            {
                count++;
            }
        }
        if (count>=2)
        {
            flag = true;
        }
        return flag;
    }

    public bool IsCanGang(ArrayList list, Hashtable kvp)
    {
        Int64 count = 0;
        bool flag = false;
        foreach (Hashtable item in list)
        {
            if (item["name"] == kvp["name"])
            {
                count++;
            }
        }
        if (count == 3)
        {
            flag = true;
        }
        return flag;
    }

   
    /// <summary>
    /// 自己摸牌的时候
    /// </summary>
    /// <param name="list"></param>
    /// <param name="kvp"></param>
    /// <returns></returns>
    public bool IsCanAnGang(ArrayList list, Hashtable kvp)
    {
        bool flag = false;
        Int64 count = 0;
        foreach (Hashtable item in list)
        {
            if (item["name"] == kvp["name"])
            {
                count++;
            }
        }
        
        if ((int)kvp["name"] == 1)
        {
            if (count == 2)
            {
                flag = true;
            }
        }
        else
        {
            if (count == 3)
            {
                flag = true;
            }
        }
        return flag;
    }

    public bool IsCanHu(ArrayList list, Hashtable kvp)
    {
        bool flag = false;
        Int64 count = 0;
        Hashtable amazing;
        //判断
        list.Add(kvp);
        foreach (Hashtable item in list)
        {
            if ((int)item["name"] == 2)
            {
                count++;
                amazing = item;
            }      
        }
        ///赖子数大于1不能胡牌
        if (count>1)
        {
            return false;
        }

        return flag;
    }

    private bool _flag;
    /// <summary>
    /// 手牌看成左、中、右三段，其中，左侧段表示“成牌区”，即：它们由坎、顺组成； 
    /// 中段表示试探区，算法要重点处理它们；
    /// RightCount是否有赖子 0没有，1有；
    /// </summary>
    /// <param name="list"></param>
    /// <param name="LeftCount"></param>
    /// <param name="MidCount"></param>
    /// <param name="RightCount"></param>
    public void Lipai(ArrayList list, int LeftCount, int MidCount, int RightCount)
    {
        Hashtable temp;
        Hashtable kvp;
        int index;
        if (LeftCount<3)
        {
            temp = list[0] as Hashtable;
            kvp = list[1] as Hashtable;
            if (kvp["name"] == temp["name"])
            {
                _flag = true;
            }
            else
            {
                if (RightCount == 1)
                {
                    if ("癞子是刚摸的牌则可以胡"== "癞子是刚摸的牌则可以胡")
                    {
                        _flag = true;
                    }
                }
            }
        }
        else
        {
            ///是否有3个相同的判断
            kvp = IsHaveThreeSame(list);
            if (kvp != null)
            {
                for (int i = list.Count-1; i >= 0; i--)
                {
                    temp = list[i] as Hashtable;
                    if (kvp["name"] == temp["name"])
                    {
                        list.RemoveAt(i);
                    }
                }
                Lipai(list, LeftCount+3, MidCount-3, RightCount);
            }

            ///是否有3个连的判断
            index = IsHaveStraight(list);
            if (index > 0)
            {
                list.RemoveAt(index);
                list.RemoveAt(index+1);
                list.RemoveAt(index+2);
                Lipai(list, LeftCount + 3, MidCount - 3, RightCount);
            }

            ///进过上面的步骤后，数组的长度大于五的话，绝对不可能胡牌
            if (list.Count>5)
            {
                _flag = false;
            }
            else if (RightCount == 0)
            {
                _flag = false;
            }
            else 
            {
                _flag = IsHaveTwoSame(list);
            }              
        }
    }

    private bool IsHaveTwoSame(ArrayList list)
    {
        bool flag = false;
        Hashtable kvp = null;
        Hashtable temp;
        Int64 count = 0;
        Int64 num1;
        Int64 num2;
        for (int i = 0; i < list.Count - 1; i++)
        {
            kvp = list[i] as Hashtable;
            temp = list[i+1] as Hashtable;
            if (kvp["name"] == temp["name"])
            {
                count++;
            }

            if (count == 2)
            {
                break;
            }          
        }

        if (count == 2)
        {
            for (int j = list.Count - 1; j > 0; j--)
            {
                temp = list[j] as Hashtable;
                if (kvp["name"] == temp["name"])
                {
                    list.RemoveAt(j);
                }
                if ((int)kvp["flag"] == 2)
                {
                    list.RemoveAt(j);
                }
            }

            //还剩最后两个
            if (list.Count == 2)
            {
                num1 = (Int64)(list[0] as Hashtable)["name"];
                num2 = (Int64)(list[1] as Hashtable)["name"];
                if ((list[0] as Hashtable)["name"] == (list[1] as Hashtable)["name"])
                {
                    flag = true;
                }
                if (Math.Abs(num1 - num2) == 1)
                {
                    flag = true;
                }
            }
            else
            {
                Debug.Log("zuihou计算出问题");
            }
        }
        else
        {
            flag = false;
        }
        return flag;
    }



    private Hashtable IsHaveThreeSame(ArrayList list)
    {
        Hashtable kvp = null;
        Hashtable temp;
        Int64 count = 0;
        for (int i = 0; i < list.Count - 2; i++)
        {
            kvp = list[i] as Hashtable;
            count = 0;
            for (int j = i + 1; j < list.Count; j++)
            {
                temp = list[j] as Hashtable;
                if (kvp["name"] == temp["name"])
                {
                    count++;
                }

                if (count >= 3)
                {
                    return kvp;
                }
            }
        }
        return kvp;
    }

    private int IsHaveStraight(ArrayList list)
    {
        Hashtable kvp = null;
        Hashtable temp;
        int num;
        int i = 0;
        for (; i < list.Count - 2; i++)
        {
            kvp = list[i] as Hashtable;
            temp = (list[i+1] as Hashtable);
            num = (int)temp["name"] - (int)kvp["name"];
            if (num != 1)
            {
                continue;
            }
            kvp = list[i+2] as Hashtable;
            num = (int)kvp["name"] - (int)temp["name"];
            if (num != 1)
            {
                continue;
            }

            return i;                  
        }
        return -1;
    }
}
