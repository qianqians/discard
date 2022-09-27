using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
public class WarRecordAccessTool
{
    private static string signForPeople = "$*@@1Gp";
    private static string signForAttr = "@*@@1Gp";
    public static string StoreDateforString(List<AccountData> list,Int64 time, int playRule,string times)
    {
        string res ="";
        string attStr ="";
        AccountData data;

        res += time;
        res += signForPeople;
        res += playRule;
        res += signForPeople;
        res += times;
      //  res += signForPeople;
        for (int i = 0; i < list.Count; i++)
        {
            attStr = "";
            attStr += signForPeople;
            data = list[i];
            attStr += data.ID;
            attStr += signForAttr;
            attStr += data.score;
            attStr += signForAttr;
            attStr += data.wechat_name;            
            res += attStr;          
        }   
        return res;
    }

    public static object[] StoreDateforString(string str)
    {
        string attStr = "";
        AccountData data;
        List<AccountData> list = new List<AccountData>();
        object[] arr = new object[]{"","","",""};
        // Regex regexPeople = new Regex(signForPeople);//
        // Regex regexAttr = new Regex(signForAttr);//
        string temp1 = Regex.Escape("$*@@1Gp");
        string temp2 = Regex.Escape("@*@@1Gp");
        string[] peopleInfo = Regex.Split(str, temp1);
        string[] attrInfo;
        arr[0] = peopleInfo[0];
        arr[1] = peopleInfo[1];
        arr[2] = peopleInfo[2];
        for (int i = 3; i < peopleInfo.Length; i++)
        {
            attStr = peopleInfo[i];
            attrInfo = Regex.Split(attStr, temp2);
            data = new AccountData();
            data.ID = Int64.Parse(attrInfo[0]);
            data.score = Int64.Parse(attrInfo[1]);
            data.wechat_name = attrInfo[2];
            list.Add(data);
        }
        arr[3] = list;    
        return arr;
    }
}
