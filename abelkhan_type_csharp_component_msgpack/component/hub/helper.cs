using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan
{
    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public class RandomHelper
    {
        //随机数对象
        private static Random _random = new Random();

        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int RandomInt(int minNum, int maxNum)
        {
            return _random.Next(minNum, maxNum);
        }

        /// <summary>
        /// 生成一个指定范围的0~max随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="maxNum">最大值</param>
        public static int RandomInt(int maxNum)
        {
            return RandomInt(0, maxNum);
        }
    }
}
