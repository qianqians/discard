using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GameCommon;
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
    public static void SortCards(List<CardObject> list, Int64 exclude)
    {
        list.Sort((CardObject a, CardObject b) => {
            Int64 numa = (Int64)a.cardType;
            Int64 numb = (Int64)b.cardType;
            return SortFun(numa, numb, exclude);
        });  
    }

    public static void SortCardsForInt(List<Int64> list, Int64 exclude)
    {
        list.Sort((Int64 a, Int64 b) => {
            return SortFun(a, b, exclude);
        });
    }
    private static int SortFun(Int64 numa,Int64 numb,Int64 exclude)
    {
        int result;
        if (numa == exclude || numb == exclude)
        {
            if (numa == numb)
            {
                result = 0;
            }
            else
            {
                if (numa == exclude)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
        }
        else
        {
            if (numa == numb)
            {
                result = 0;
            }
            else
            {

                if (numa.CompareTo(numb) > 0)
                {
                    result = -1;
                }
                else
                {
                    result = 1;
                }
            }
        }
        return result;
    }

    public static void SortCardsByGameObject(List<GameObject> cards)
    {
        cards.Sort(
            (GameObject a, GameObject b) =>
            {
                return a.GetComponent<CardData>().CardType.CompareTo(b.GetComponent<CardData>().CardType);
            }
        );
    }   

    /// <summary>
    /// 判断是否可以碰和杆
    ///（返回的值为2表示两个相同，3表示三个相同）
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static int PopEnable(List<Int64> cards, Int64 cardID, Int64 laizipi)
    {
        int count = 0;
        foreach (Int64 item in cards)
        {
            if (item == cardID)
            {
                count++;
            }
        }
        if (cardID == laizipi && count >= 2)
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// 自己摸牌后检测自己是否有可以杠的牌，，如果有，则返回可以杠的牌
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="cardID"></param>
    /// <param name="laizi"></param>
    /// <param name="laizipi"></param>
    /// <returns></returns>
    public static int CheckBySelfAddCardIsCanGang(List<Int64> cards, Int64 cardID,Int64 laizi ,Int64 laizipi)
    {
        int count = 0;
        List<Int64> list = new List<Int64>(cards);
        Int64 tempID;
        if (cardID != 0)
        {
            list.Add(cardID);
        }         
        int length =list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            count = 0;
            tempID = list[i];
            if (tempID == laizi)
            {
                continue;
            }
            //length = 
            for(int j = i+1; j < length; j++)
            {
                if (list[j] == tempID)
                {
                    count++;
                }
            }

            if (tempID == laizipi && count == 2)
            {
                count++;
            }

            if (count == 3)
            {
                return (int)tempID;
            }
        }
        return 0;
    }

    /// <summary>
    /// 检测自己是否是硬胡
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static bool CheckIsYingHu(List<Int64> arr)
    {
        List<Int64> temp;
        arr.Sort();
        for (int i = arr.Count - 1; i > 0;)//检测赖子不做配牌的情况
        {
            temp = new List<Int64>(arr);
            if (arr[i] == arr[i - 1])
            {
                temp.RemoveAt(i);
                temp.RemoveAt(i - 1);
                i -= 2;
                if (checkHupaiTwo(temp))
                {
                    return true;
                }

                temp.Reverse();
                if (checkHupaiTwo(temp))
                {
                    return true;
                }
            }
            else
            {
                i--;
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="laizi"></param>
    /// <returns></returns>
    public static bool IsHupai(List<Int64> list, Int64 laizi, Int64 mopai,GameRule rule)
    {
        list.Sort();
        int count = 0;
        List<Int64> arr = new List<Int64>(list);
        if (CheckIsHuByAny(list,laizi))
        {
            return false;
        }
        arr.Add(mopai);
       
        for (int i = 0; i < arr.Count; i++)
        {
            if (laizi == arr[i])
            {
                count++;
            }
        }

        if (count >= 2)
        {
            return false;
        }

        if (rule == GameRule.NoLai)
        {
            if (count > 0)
            {
                return false;
            }
        }

        if (CheckIsYingHu(arr))
        {
            return true;
        }

      //  arr.Sort();           
        List<Int64> temp;
        if (rule == GameRule.HalfLai)
        {
            for (int i = arr.Count - 1; i > 0;)//检测赖子不做配牌的情况
            {
                temp = new List<Int64>(arr);
                if (arr[i] == arr[i - 1])
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    if (checkHupaiTwo(temp))
                    {
                        return true;
                    }

                    temp.Reverse();
                    if (checkHupaiTwo(temp))
                    {
                        return true;
                    }
                }
                else
                {
                    i--;
                }
            }

      
            if (count > 0 && TableController.Instance.PlayrInfo.selfPutOutLaiziNum == 0)
            {
                if (supposePutoutCard != laizi || supposePutoutCard==0)//模拟检测的时候。检测自己打出去的是赖子
                {
                    return false;
                }   
            }                    
        }

        if (count == 0)
        {
            //for (int i = arr.Count - 1; i > 0;)
            //{
            //    temp = new List<Int64>(arr);
            //    if (arr[i] == arr[i - 1])
            //    {
            //        temp.RemoveAt(i);
            //        temp.RemoveAt(i - 1);
            //        i -= 2;
            //        if (checkHupaiTwo(temp))
            //        {
            //            return true;
            //        }

            //        //temp.Reverse();
            //        //if (checkHupaiTwo(temp))
            //        //{
            //        //    return true;
            //        //}
            //    }
            //    else
            //    {
            //        i--;
            //    }
            //}
        }
        else
        {
            temp = new List<Int64>(arr);
            if (CheckHuipaiByHaveLaiziTwo(temp, laizi))
            {
                return true;
            }
            for (int i = arr.Count - 1; i > 0;)
            {
                temp = new List<Int64>(arr);
                if (arr[i] == arr[i - 1])
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    if (CheckHuipaiByHaveLaiziTwo(temp, laizi))
                    {
                        return true;
                    }
                    //if (ChenckHuipaiByHavaLaiOP(temp,laizi))
                    //{
                    //    return true;
                    //}

                    temp.Reverse();
                    if (CheckHuipaiByHaveLaiziTwo(temp, laizi))
                    {
                        return true;
                    }
                    //if (ChenckHuipaiByHavaLaiOP(temp,laizi))
                    //{
                    //    return true;
                    //}
                }
                else
                {
                    i--;
                }
            }
        }
        return false;
    }

    private static bool ChenckHuipaiByHavaLaiOneee(List<Int64> arr, Int64 laizi)
    {
        List<Int64> wangList = new List<Int64>();
        List<Int64> tongList = new List<Int64>();
        List<Int64> tiaolist = new List<Int64>();
        List<Int64> temp = new List<Int64>(arr);
        temp.Remove(laizi);

        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] < (Int64)mjCards.wan_Num)
            {
                wangList.Add(temp[i]);
            }
            else if (temp[i] < (Int64)mjCards.tong_Num)
            {
                tongList.Add(temp[i]);
            }
            else
            {
                tiaolist.Add(temp[i]);
            }
        }
        //if (!ChenckHuipaiByHavaLaiOP(wangList))
        //{

        //}
        return false;
    }

    private static bool ChenckHuipaiByHavaLaiOP(List<Int64> arr, Int64 laizi)
    {
        List<Int64> temp = new List<Int64>(arr);
        temp.Remove(laizi);
        for (int i = temp.Count-1; i >0 ; i--)
        {
            if(ChcekSandwich(temp[i], temp[i - 1]))
            {
                temp.RemoveAt(i);
                temp.RemoveAt(i-1);
                break;
            }
        }
        List<Int64> temptwo = new List<Int64>(temp);
        TakeOutShunzi(temp);
        TakeOutThreeSame(temp);
        if (temp.Count==0)
        {
            return true;
        }

        TakeOutThreeSame(temptwo);
        TakeOutShunzi(temptwo);      
        if (temptwo.Count == 0)
        {
            return true;
        }
        return false;
    }

    private static bool ChenckHuipaiByHavaLai(List<Int64> arr, Int64 laizi)
    {
        int count = 1;
        List<Int64> temp = new List<Int64>(arr);
        temp.Remove(laizi);
        for (int i = temp.Count -1; i >=0;)
        {
            if (temp.Count>2)
            {
                if (temp[i] == temp[i - 1])
                {
                    if (temp[i - 1] == temp[i - 2])
                    {
                        temp.RemoveAt(i);
                        temp.RemoveAt(i - 1);
                        temp.RemoveAt(i - 2);
                        i -= 3;
                    }
                    else
                    {
                        temp.RemoveAt(i);
                        temp.RemoveAt(i - 1);
                        i -= 2;
                        count--;
                    }
                }
                else if (Math.Abs(temp[i] - temp[i - 1]) == 1)
                {
                    if (Math.Abs(temp[i - 1] - temp[i - 2]) == 1)
                    {
                        temp.RemoveAt(i);
                        temp.RemoveAt(i - 1);
                        temp.RemoveAt(i - 2);
                        i -= 3;
                    }
                    else
                    {
                        temp.RemoveAt(i);
                        temp.RemoveAt(i - 1);
                        i -= 2;
                        count--;
                    }
                }
                else if (ChcekSandwich(temp[i], temp[i - 1]))
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    count--;
                }
                else
                {
                    return false;
                }             
            }
            else
            {
                if (Math.Abs(temp[i] - temp[i - 1]) == 1 || ChcekSandwich(temp[i], temp[i-1]) || temp[i] == temp[i - 1])
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    count--;
                }
                else
                {
                    return false;
                }
            }

            if (count <= -1)
            {
                return false;
            }
        }

        return true;
    }

    private static bool CheckHuipaiByHaveLaiziTwo(List<Int64> arr, Int64 laizi)
    {
        int count = 1;
        int tempCount;
        int tempCountTwo;
        List<Int64> wangList = new List<Int64>();
        List<Int64> tongList = new List<Int64>();
        List<Int64> tiaolist = new List<Int64>();
        List<Int64> temp = new List<Int64>(arr);
        temp.Remove(laizi);

        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i] < (Int64)mjCards.wan_Num)
            {
                wangList.Add(temp[i]);
            }
            else if (temp[i] < (Int64)mjCards.tong_Num)
            {
                tongList.Add(temp[i]);
            }
            else
            {
                tiaolist.Add(temp[i]);
            }
        }
        if (wangList.Count>0)
        {
            tempCount = CheckEvyone(wangList, mjCards.wan_Num);
            if (tempCount==2)
            {
                tempCountTwo = CheckEvyTwo(wangList, mjCards.wan_Num);
                if (tempCountTwo < tempCount)
                {
                    tempCount = tempCountTwo;
                }
            }
                 
            if (tempCount == 2)
            {
                return false;
            }
            else
            {
                count -= tempCount;
            }
        }

        if (tongList.Count>0)
        {
            tempCount = CheckEvyone(tongList, mjCards.tong_Num);
            if (tempCount == 2)
            {
                tempCountTwo = CheckEvyTwo(tongList, mjCards.tong_Num);
                if (tempCountTwo < tempCount)
                {
                    tempCount = tempCountTwo;
                }
            }

            if (tempCount == 2)
            {
                return false;
            }
            else
            {
                count -= tempCount;
            }
        }

        if (tiaolist.Count>0)
        {
            tempCount = CheckEvyone(tiaolist, mjCards.tiao_Num);

            if (tempCount == 2)
            {
                tempCountTwo = CheckEvyTwo(tiaolist, mjCards.tiao_Num);
                if (tempCountTwo < tempCount)
                {
                    tempCount = tempCountTwo;
                }
            }
            if (tempCount == 2)
            {
                return false;
            }
            else
            {
                count -= tempCount;
            }
        }
      
        if (count<0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private static int CheckEvyone(List<Int64> list,mjCards state)
    {
        int count =0;
        List<Int64> tempList = new List<Int64>(list);
        List<Int64> temp = new List<Int64>(list);
        TakeOutShunzi(temp);
        TakeOutThreeSame(temp);
        if (temp.Count > 2)
        {
            return 2;
        }

        if (temp.Count == 2)
        {
            if (temp[1] == temp[0] || Math.Abs(temp[1] - temp[0]) == 1)
            {
                count++;
            }
            else
            {
                if (ChcekSandwich(temp[1], temp[0]))
                {
                    count++;
                }
                else
                {
                    // return 2;
                    if (CheckOneMJ(tempList, state))
                    {
                        count++;
                    }
                    else
                    {
                        return 2;
                    }                
                }
            }          
        }

        if (temp.Count == 1)
        {
            count++;
        }
        return count;
    }

    private static int CheckEvyTwo(List<Int64> list, mjCards state)
    {
        int count = 0;
        List<Int64> tempList = new List<Int64>(list);
        List<Int64> temp = new List<Int64>(list);
        TakeOutThreeSame(temp);
        TakeOutShunzi(temp);
        if (temp.Count > 2)
        {
            return 2;
        }

        if (temp.Count == 2)
        {
            if (temp[1] == temp[0] || Math.Abs(temp[1] - temp[0]) == 1)
            {
                count++;
            }
            else
            {
                if (ChcekSandwich(temp[1], temp[0]))
                {
                    count++;
                }
                else
                {
                    // return 2;
                    if (CheckOneMJ(tempList, state))
                    {
                        count++;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
        }

        if (temp.Count == 1)
        {
            count++;
        }
        return count;
    }

    private static bool CheckOneMJ(List<Int64> arr, mjCards state)
    {
        if (arr.Count >7)
        {
            List<Int64> tempList;
            int start;
            int end;
            if (state == mjCards.wan_Num)
            {
                start = 1;
                end = 10;
            }
            else if (state == mjCards.tong_Num)
            {
                start = 11;
                end = 20;
            }
            else
            {
                start = 21;
                end = 30;
            }
            for (int i = start; i < end; i++)
            {
                tempList = new List<Int64>(arr);
                tempList.Add(i);
                tempList.Sort();
                if (CheckIsNeat(tempList))
                {
                    return true;
                }
                tempList.Reverse();
                if (CheckIsNeat(tempList))
                {
                    return true;
                }
            }
        }
      
        return false;
    }

    //  private 
    private  static bool CheckHuipaiByHaveLaizi(List<Int64> arr,Int64 laizi, GameRule rule)
    {
        List<Int64>  temp = new List<Int64>(arr);
        temp.Remove(laizi);
        int count;
        if (TableController.Instance.creatRoomInfo.playerNum == 4)
        {
            count = (int)mjCards.tiao_Num;
        }
        else
        {
            count = (int)mjCards.tong_Num;
        }
        for (int i = 1; i < count; i++)
        {
            if (i % 10 != 0 && i!= laizi)
            {
                temp.Add(i);
                if (CheckIsYingHu(temp))
                {
                    return true;
                }
                temp.Remove(i);
            }
        }
        return false;
    }

    //private static bool CheckHupaiTwoHavelaizi(List<Int64> arr, Int64 laizi)
    //{
    //    List<Int64> shengyu = new List<Int64>(arr);
    //    List<Int64> shengyuTwo ;
    //    for (int i = shengyu.Count - 1; i >= 0; i--)
    //    {
    //        if (shengyu[i] == laizi)
    //        {
    //            shengyu.RemoveAt(i);
    //            break;
    //        }
    //    }
    //    shengyuTwo = new List<Int64>(shengyu);
    //    TakeOutThreeSame(shengyu);
    //    TakeOutShunzi(shengyu);      
    //    if (shengyu.Count == 2)
    //    {
    //        if (shengyu[1] == shengyu[0] || Math.Abs(shengyu[1] - shengyu[0]) == 1)
    //        {
    //            return true;
    //        }
    //        if (ChcekSandwich(shengyu[1], shengyu[0]))
    //        {
    //            return true;
    //        }
    //    }
    //    if (shengyu.Count == 1)
    //    {
    //        return true;
    //    }

    //    shengyu = shengyuTwo;
    //    TakeOutShunzi(shengyu);
    //    TakeOutThreeSame(shengyu);      
    //    if (shengyu.Count == 2)
    //    {
    //        if (shengyu[1] == shengyu[0] || Math.Abs(shengyu[1] - shengyu[0]) == 1)
    //        {
    //            return true;
    //        }
    //        if (ChcekSandwich(shengyu[1], shengyu[0]))
    //        {
    //            return true;
    //        }
    //    }
    //    if (shengyu.Count == 1)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private static bool checkHupaiTwo(List<Int64> arr)
    {
        List<Int64> wangList = new List<Int64>();
        List<Int64> tongList = new List<Int64>();
        List<Int64> tiaolist = new List<Int64>();
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i]<(Int64)mjCards.wan_Num)
            {
                wangList.Add(arr[i]);
            }
            else if (arr[i] < (Int64)mjCards.tong_Num)
            {
                tongList.Add(arr[i]);
            }
            else
            {
                tiaolist.Add(arr[i]);
            }           
        }

        if (CheckIsNeat(wangList))
        {
            if (CheckIsNeat(tongList))
            {
                if (CheckIsNeat(tiaolist))
                {
                    return true;
                }
            }
        }      
        return false;
    }

    /// <summary>
    /// 检测 牌是否是圆句
    /// </summary>
    private static bool CheckIsNeat(List<Int64> arr)
    {
        List<Int64> shengyu = new List<Int64>(arr);
        List<Int64> shengyuTwo = new List<Int64>(arr);
        TakeOutThreeSame(shengyu);
        TakeOutShunzi(shengyu);
        if (shengyu.Count == 0)
        {
            return true;
        }

        TakeOutShunzi(shengyuTwo);
        TakeOutThreeSame(shengyuTwo);
        if (shengyuTwo.Count == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 剔除 拿出三个一样的
    /// </summary>
    /// <param name="shengyu"></param>
    private static void TakeOutThreeSame(List<Int64> shengyu)
    {
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            if (shengyu[i] == shengyu[i - 1] && shengyu[i] == shengyu[i - 2])
            {
                shengyu.RemoveAt(i);
                shengyu.RemoveAt(i - 1);
                shengyu.RemoveAt(i - 2);
                i -= 3;
            }
            else
            {
                i--;
            }
        }
    }

    private static void TakeOutShunzi(List<Int64> shengyu)
    {
        bool flag;
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            flag = false;
            if (Math.Abs(shengyu[i] - shengyu[i - 1]) == 1)
            {
                for (int k = 0; k < i - 1; k++)
                {
                    if (Math.Abs(shengyu[i - 1] - shengyu[k]) == 1)
                    {
                        shengyu.RemoveAt(i);
                        shengyu.RemoveAt(i - 1);
                        shengyu.RemoveAt(k);
                        i = shengyu.Count - 1;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    i--;
                }
            }
            else
            {
                i--;
            }
        }
    }

    /// <summary>
    /// 检测是不是单一个赖子做将，那么摸任何牌都可以胡牌，是不允许的
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="laizi"></param>
    /// <returns></returns>
    private static bool CheckIsHuByAny(List<Int64> arr,Int64 laizi)
    {
        List<Int64> shengyu = new List<Int64>(arr);
        for (int i = shengyu.Count - 1; i >= 0; i--)
        {
            if (shengyu[i] == laizi)
            {
                shengyu.RemoveAt(i);
                break;
            }
        }
        shengyu.Sort();
        TakeOutThreeSame(shengyu);
        TakeOutShunzi(shengyu);
        if (shengyu.Count ==0)
        {
            return true;
        }
        return false;
    }

    private static bool ChcekSandwich(Int64 num1, Int64 num2)
    {
        if (Math.Abs(num1 - num2) == 2)
        {
            if (num1 / 10 == num2 / 10)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测自己打什么牌可以听，，supposePutoutCard记录的就是当前模拟要打出去的牌
    /// </summary>
    public static Int64 supposePutoutCard;
    /// <summary>
    /// 检测打什么牌后可以听牌
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="laiz"></param>
    /// <returns></returns>
    public static List<Int64> CheckPutOutCanTingpai(List<Int64> arr,Int64 laiz, GameRule gameRule)
    {
        List<Int64> canTingCardList = new List<Int64>();
        List<Int64> canNotCardList = new List<Int64>();
        List<Int64> shengyu;
        List<Int64> tempList = new List<Int64>(arr);
       
        for (int i = tempList.Count-1; i >=0 ; i--)
        {
            shengyu = new List<Int64>(tempList);
            supposePutoutCard = tempList[i];
            shengyu.RemoveAt(i);

            if (!canNotCardList.Contains(supposePutoutCard))
            {
                if (canTingCardList.Contains(supposePutoutCard))
                {
                    canTingCardList.Add(tempList[i]);
                }
                else
                {
                    if (CheckIsTing(shengyu, laiz, gameRule).Count > 0)
                    {
                        canTingCardList.Add(tempList[i]);
                    }
                    else
                    {
                        canNotCardList.Add(tempList[i]);
                    }
                }
            }                  
        }
        supposePutoutCard = 0;
        return canTingCardList;
    }

    /// <summary>
    /// 检测是否听牌
    /// </summary>
    /// <returns></returns>
    public static List<Int64> CheckIsTing(List<Int64> list,Int64 laizi,GameRule gameRule,Int64 choseCard = 0)
    {
        int count;
        List<Int64> canHuCardList = new List<Int64>();
        if (TableController.Instance.creatRoomInfo.playerNum == 4)
        {
            count = (int)mjCards.tiao_Num;
        }
        else
        {
            count = (int)mjCards.tong_Num;
        }
        for (int i = 1; i < count; i++)
        {
            if(i%10!=0)
            {
                if (choseCard!=0)
                {
                    supposePutoutCard = choseCard;
                }
                if (IsHupai(list, laizi, i, gameRule))
                {
                    canHuCardList.Add(i);
                }
            }
        }
        return canHuCardList;
    }

    /// <summary>
    /// 检测自己无论赖子配什么都不可以胡牌
    /// </summary>
    private static void CheckIsCanNotHupai()
    {

    }
}
