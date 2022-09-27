using System;
using System.Collections.Generic;
using System.Collections;

namespace abelkhan.cmd
{
    public class HEnum : IComparable<HEnum>, IEquatable<HEnum>
    {
        static int counter = -1;            //默认数值计数器
        private static Hashtable hashTable = new Hashtable();       //不重复数值集合
        protected static List<HEnum> members = new List<HEnum>();   //所有实例集合
        public string Name { get; set; }
        public int Value { get; set; }
        public string Des { get; set; }

        /// <summary>
        /// 不指定数值构造实例
        /// </summary>
        protected HEnum(string name, string des)
        {
            this.Des = des;
            this.Name = name;
            this.Value = ++counter;
            members.Add(this);
            if (!hashTable.ContainsKey(this.Value))
            {
                hashTable.Add(this.Value, this);
            }
        }

        public HEnum(): this("OK", "Success", 0) { 
        
        }

        /// <summary>
        /// 指定数值构造实例
        /// </summary>
        protected HEnum(string name, int value) : this(name, name)
        {
            this.Value = value;
            counter = value;
        }

        /// <summary>
        /// 指定数值构造实例
        /// </summary>
        protected HEnum(string name, string des, int value) : this(name, des)
        {
            this.Value = value;
            counter = value;
        }

        /// <summary>
        /// 向string转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name.ToString();
        }

        /// <summary>
        /// 显式强制从int转换
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static explicit operator HEnum(int i)
        {
            if (hashTable.ContainsKey(i))
            {
                return (HEnum)members[i];
            }
            return new HEnum(i.ToString(), i);
        }

        /// <summary>
        /// 显式强制向int转换
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static explicit operator int(HEnum e)
        {
            return e.Value;
        }

        public static void ForEach(Action<HEnum> action)
        {
            foreach (HEnum item in members)
            {
                action(item);
            }
        }

        public int CompareTo(HEnum other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(HEnum other)
        {
            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HEnum))
                return false;
            return this.Value == ((HEnum)obj).Value;
        }

        public override int GetHashCode()
        {
            HEnum std = (HEnum)hashTable[this.Value];
            if (std.Name == this.Name)
                return base.GetHashCode();
            return std.GetHashCode();
        }

        public static bool operator !=(HEnum e1, HEnum e2)
        {
            return e1.Value != e2.Value;
        }

        public static bool operator <(HEnum e1, HEnum e2)
        {
            return e1.Value < e2.Value;
        }

        public static bool operator <=(HEnum e1, HEnum e2)
        {
            return e1.Value <= e2.Value;
        }

        public static bool operator ==(HEnum e1, HEnum e2)
        {
            return e1.Value == e2.Value;
        }

        public static bool operator >(HEnum e1, HEnum e2)
        {
            return e1.Value > e2.Value;
        }

        public static bool operator >=(HEnum e1, HEnum e2)
        {
            return e1.Value >= e2.Value;
        }
    }
}
