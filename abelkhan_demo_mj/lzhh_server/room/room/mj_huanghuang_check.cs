using System;
using System.Collections;
using System.Collections.Generic;

namespace room
{
    public enum HupaiState
    {
        no_hu,
        soft_hu,
        hard_hu
    }

    class mj_huanghuang_check
    {
        static public bool check_peng(ArrayList cards, Int64 laizipi, Int64 card)
        {
            if (card == laizipi)
            {
                return false;
            }

            Int64 count = 0;
            foreach (var item in cards)
            {
                if ((Int64)item == card)
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                return true;
            }

            return false;
        }

        static public bool check_gang(ArrayList cards, ArrayList peng, Int64 laizipi, Int64 card, bool zimo, bool add_card)
        {
            Int64 laizi = laizipi + 1;
            if (laizi == 10)
            {
                laizi = (int)GameCommon.mjCards.W_1;
            }
            else if (laizi == 20)
            {
                laizi = (int)GameCommon.mjCards.B_1;
            }
            else if (laizi == 30)
            {
                laizi = (int)GameCommon.mjCards.T_1;
            }
            if(laizi == card)
            {
                return false;
            }

            Int64 count = 0;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang:{0}  add_card:{1}", card.ToString(), add_card);
            foreach (var item in cards)
            {
                if ((Int64)item == card)
                {
                    count++;
                }
            }

            if (add_card)
            {
                if (count == 3 || (laizipi == card && count == 2))
                {
                    return true;
                }
            }
            else
            {
                if (peng.Contains(card) && zimo)
                {
                    return true;
                }

                if (count == 4 || (laizipi == card && count == 3))
                {
                    return true;
                }
            }

            return false;
        }

        static public HupaiState check_dian_hu(ArrayList list, Int64 laizi, Int64 pai)
        {
            HupaiState flag = HupaiState.no_hu;
            int count = 0;
            List<Int64> arr = new List<Int64>();
            foreach (var item in list)
            {
                arr.Add((Int64)item);
            }
            
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, Json.Jsonparser.pack(list));
            if (CheckIsHuByAny(arr, laizi))
            {

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "CheckIsHuByAny");
                return flag;
            }

            arr.Add(pai);
            for (int i = 0; i < arr.Count; i++)
            {
                if (laizi == arr[i])
                {
                    count++;
                }
            }
            if (count >= 2)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "count >= 2");
                return flag;
            }
            arr.Sort();
            List<Int64> shen_yu = new List<Int64>();
            List<Int64> temp;
            List<Int64> temp2;
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
                        temp2 = temp;
                        temp2.Reverse();
                        if (checkHupaiTwo(temp2))
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
                        if (checkHupaiTwo(temp))
                        {
                            flag = HupaiState.hard_hu;
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "一");
                            return flag;
                        }
                        flag = checkHupaiTwoHavelaizi(temp, laizi);
                        if (flag != HupaiState.no_hu)
                        {
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "二");
                            return flag;
                        }

                        temp2 = temp;
                        temp2.Reverse();
                        flag = checkHupaiTwoHavelaizi(temp2, laizi);
                        if (flag != HupaiState.no_hu)
                        {
                            flag = HupaiState.soft_hu;
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "三");
                            return flag;
                        }
                    }
                    else
                    {
                        i--;
                    }
                }

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", pai);
                temp = new List<Int64>(arr);
                if (checkHupaiTwo(temp))
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "四");
                    flag = HupaiState.hard_hu;
                }
                flag = checkHupaiTwoHavelaizi(temp, laizi);
                if (flag == HupaiState.no_hu)
                {
                    flag = SubstitutePerCard(arr, laizi, GameCommon.PeopleNum.FourPeople);
                }
            }
            return flag;
        }

        static public HupaiState check_hu(ArrayList list, Int64 laizi, Int64 mopai)
        {
            HupaiState flag = HupaiState.no_hu;
            int count = 0;
            List<Int64> arr = new List<Int64>();
            foreach(var item in list)
            {
                arr.Add((Int64)item);
            }

            arr.Remove(mopai);
            if (CheckIsHuByAny(arr, laizi))
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "CheckIsHuByAny");
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
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "count >= 2");
                return flag;
            }
            arr.Sort();
            List<Int64> shen_yu = new List<Int64>();
            List<Int64> temp;
            List<Int64> temp2;
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
                        temp2 = temp;
                        temp2.Reverse();
                        if (checkHupaiTwo(temp2))
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
                        if (checkHupaiTwo(temp))
                        {
                            flag = HupaiState.hard_hu;
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "一");
                            return flag;
                        }
                        flag = checkHupaiTwoHavelaizi(temp, laizi);
                        if (flag != HupaiState.no_hu)
                        {
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "二");
                            return flag;
                        }

                        temp2 = temp;
                        temp2.Reverse();
                        flag = checkHupaiTwoHavelaizi(temp2, laizi);
                        if (flag != HupaiState.no_hu)
                        {
                            flag = HupaiState.soft_hu;
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "三");
                            return flag;
                        }
                    }
                    else
                    {
                        i--;
                    }
                }

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", mopai.ToString());
                temp = new List<Int64>(arr);
                if (checkHupaiTwo(temp))
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "四");
                    flag = HupaiState.hard_hu;
                }
                flag = checkHupaiTwoHavelaizi(temp, laizi);
                if (flag == HupaiState.no_hu)
                {
                    flag = SubstitutePerCard(arr, laizi, GameCommon.PeopleNum.FourPeople);
                }
            }
            return flag;
        }

        static private HupaiState checkHupaiTwoHavelaizi(List<Int64> arr, Int64 laizi)
        {
            HupaiState type = HupaiState.no_hu;
            List<Int64> shengyu = new List<Int64>(arr);
            List<Int64> shengyuTwo;
            for (int i = shengyu.Count - 1; i >= 0; i--)
            {
                if (shengyu[i] == laizi)
                {
                    shengyu.RemoveAt(i);
                    break;
                }
            }
            shengyuTwo = new List<Int64>(shengyu);
            TakeOutThreeSame(shengyu);
            TakeOutShunzi(shengyu);
         
                     
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
                        if (CheckIsShunzi(shengyu[0], shengyu[1], laizi))
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
            if (shengyu.Count == 1)
            {
                return HupaiState.soft_hu;
            }

            shengyu = shengyuTwo;
            TakeOutShunzi(shengyu);
            TakeOutThreeSame(shengyu);
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
                        if (CheckIsShunzi(shengyu[0], shengyu[1], laizi))
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
            if (shengyu.Count == 1)
            {
                return HupaiState.soft_hu;
            }

            return type;
        }

        static private bool CheckIsShunzi(Int64 a, Int64 b, Int64 c)
        {
            List<Int64> list = new List<Int64> { a, b, c };
            list.Sort();
            if (list[2] - list[1] == 1 && list[1] - list[0] == 1)
            {
                return true;
            }
            return false;
        }

        static private bool checkHupaiTwo(List<Int64> arr)
        {
            List<Int64> shengyu = new List<Int64>(arr);

            List<Int64> shengyuTwo = new List<Int64>(arr);
            TakeOutThreeSame(shengyu);
            if (shengyu.Count == 0)
            {
                return true;
            }
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
        static private void TakeOutThreeSame(List<Int64> shengyu)
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

        static private void TakeOutShunzi(List<Int64> shengyu)
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
        static private bool CheckIsHuByAny(List<Int64> arr, Int64 laizi)
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
            if (shengyu.Count == 0)
            {
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "CheckIsHuByAny true");
                return true;
            }
            return false;
        }

        static private bool ChcekSandwich(Int64 num1, Int64 num2)
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

        static private HupaiState SubstitutePerCard(List<Int64> arr, Int64 lai, GameCommon.PeopleNum num)
        {
            HupaiState flag = HupaiState.no_hu;
            List<Int64> temp1 = new List<Int64>(arr);       
            temp1.Remove(lai);
            int circular_num;
            if (num == GameCommon.PeopleNum.FourPeople)
            {
                circular_num = (int)GameCommon.mjCards.T_9;
            }
            else
            {
                circular_num = (int)GameCommon.mjCards.B_9;
            }
            for (int j = 1; j <= circular_num; j++)
            {
                if (j % 10 != 0)
                {
                    temp1.Add(j);
                    temp1.Sort();
                    for (int i = arr.Count - 1; i > 0;)
                    {
                        List<Int64> temp = new List<Int64>(temp1);
                        if (temp1[i] == temp1[i - 1])
                        {
                            temp.RemoveAt(i);
                            temp.RemoveAt(i - 1);
                            i -= 2;
                            if (checkHupaiTwo(temp))
                            {
                                if (j == lai)
                                {
                                    flag = HupaiState.hard_hu;
                                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "五");
                                    return flag;
                                }
                                else
                                {
                                    flag = HupaiState.soft_hu;
                                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "六");
                                    return flag;
                                }
                            }
                            temp.Reverse();
                            if (checkHupaiTwo(temp))
                            {
                                if (j == lai)
                                {
                                    flag = HupaiState.hard_hu;
                                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "七");
                                    return flag;
                                }
                                else
                                {
                                    flag = HupaiState.soft_hu;
                                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "八");
                                    return flag;
                                }
                            }
                        }
                        else
                        {
                            i--;
                        }
                    }
                    temp1.Remove(j);
                }
            }
            return flag;
        }
    }
}
