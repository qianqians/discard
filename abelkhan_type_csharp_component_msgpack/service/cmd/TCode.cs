using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace abelkhan.cmd
{
    public class TCode: HEnum
    {

        public TCode(string name, string des, int value) : base(name, des, value) { }

        public static TCode OK = new TCode("OK", "Success", 0);

        /// <summary>
        ///  30000
        /// </summary>
        public static TCode CMD_NOT_EXISTS = new TCode("CMD_NOT_EXISTS", "命令不存在", 29999);
        public static TCode CMD_DO_EXCEPTION = new TCode("CMD_DO_EXCEPTION", "命令处理异常", 30000);
    }
}
