using System;
using System.Collections.Generic;
using GameCommon;
using System.Text;

/// <summary>
/// 服务器用的
/// </summary>
public class HuPai
{
    public static HupaiState IsHupai(List<Int64> list, Int64 laizi, Int64 mopai)
    {
        HupaiState flag = HupaiState.no_hu;
        int count = 0;
        List<Int64> arr = new List<Int64>(list);
        if (CheckIsHuByAny(list, laizi))
        {
            return flag;
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
            return flag;
        }
        arr.Sort();
        List<Int64> shen_yu = new List<Int64>();
        List<Int64> temp;
        if (count == 0)
        {
            for (int i = arr.Count - 1; i > 0;)
            {
                temp = new List<Int64>(arr);
                if (arr[i] == arr[i - 1])
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    if (checkHupaiTwo(temp))
                    {
                        flag = HupaiState.hard_hu;
                        return flag;
                    }
                }
                else
                {
                    i--;
                }
            }
        }
        else
        {
            temp = new List<Int64>(arr);
            for (int i = arr.Count - 1; i > 0;)
            {
                temp = new List<Int64>(arr);
                if (arr[i] == arr[i - 1])
                {
                    temp.RemoveAt(i);
                    temp.RemoveAt(i - 1);
                    i -= 2;
                    flag = checkHupaiTwoHavelaizi(temp, laizi);
                    if (flag != HupaiState.no_hu)
                    {
                        return flag;
                    }                                                 
                }
                else
                {
                    i--;
                }
            }

            //赖子必须被配成对子——！
            if (mopai == laizi)
            {
                return checkHupaiTwoHavelaizi(temp, laizi);
            }
            else
            {
                return flag;
            }
        }
        return flag;
    }

    private static HupaiState checkHupaiTwoHavelaizi(List<Int64> arr, Int64 laizi)
    {
        HupaiState type = HupaiState.no_hu;
        List<Int64> shengyu = new List<Int64>(arr);
        shengyu.Remove(laizi);
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            if (shengyu[i] == arr[i - 1] && shengyu[i] == shengyu[i - 2])
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

        bool flag;
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            flag = false;
            if (shengyu[i] == shengyu[i - 1] + 1)
            {
                for (int k = 0; k < i - 1; k++)
                {
                    if (shengyu[i - 1] == shengyu[k] + 1)
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

        if (shengyu.Count == 2)
        {
            if (shengyu[1] == shengyu[0])
            {
                return HupaiState.soft_hu;
            }
            else
            {
                if (Math.Abs(shengyu[1] - shengyu[0]) == 1 || ChcekSandwich(shengyu[1], shengyu[0]))
                {
                    if (CheckIsShunzi(shengyu[0],shengyu[1],laizi))
                    {
                        return HupaiState.hard_hu;
                    }
                    else
                    {
                        return HupaiState.soft_hu;
                    }
                }
            }      
        }
        return type;
    }

    private static bool CheckIsShunzi(Int64 a,Int64 b,Int64 c)
    {
        List<Int64> list = new List<Int64> { a, b, c };
        list.Sort();
        if (list[2]- list[1] == list[1]- list[0])
        {
            return true;
        }
        return false;
    }
  
    private static bool checkHupaiTwo(List<Int64> arr)
    {
        List<Int64> shengyu = new List<Int64>(arr);
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            if (shengyu[i] == shengyu[i - 1] && shengyu[i] == shengyu[i - 2])
            {
                // shengyu.
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
        if (shengyu.Count == 0)
        {
            return true;
        }
        bool flag;
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            flag = false;
            if (shengyu[i] == shengyu[i - 1] + 1)
            {
                for (int k = 0; k < i - 1; k++)
                {
                    if (shengyu[i - 1] == shengyu[k] + 1)
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
        if (shengyu.Count == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检测是不是单一个赖子做将，那么摸任何牌都可以胡牌，是不允许的
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="laizi"></param>
    /// <returns></returns>
    private static bool CheckIsHuByAny(List<Int64> arr, Int64 laizi)
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
        bool flag;
        for (int i = shengyu.Count - 1; i >= 2;)
        {
            flag = false;
            if (shengyu[i] == shengyu[i - 1] + 1)
            {
                for (int k = 0; k < i - 1; k++)
                {
                    if (shengyu[i - 1] == shengyu[k] + 1)
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
        if (shengyu.Count == 0)
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
}

