using System;
using System.ComponentModel;
using System.Reflection;

namespace abelkhan.admin.helper
{
    public class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memberInfos = type.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                DescriptionAttribute[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];   //获取描述特性  
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description;
                }
            }
            return en.ToString();
        }

        public static int getEnumValueByName(Type source, string name) {
            return (int)Enum.Parse(source, name);
        }
    }
}
