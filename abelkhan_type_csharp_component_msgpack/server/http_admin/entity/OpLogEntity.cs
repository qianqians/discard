using System;
using System.Collections.Generic;
using System.Text;

namespace http_admin
{
    /// <summary>
    /// 后台操作日志
    /// </summary>
    public class OpLogEntity
    {
        public string userName { set; get; }
        public string action { set; get; }

        public string ip { set; get; }

        public string reqParams { set; get; }

        public Int64 operationTime { set; get; }

        public string respStr { set; get; }

        public static OpLogEntity ValueOf(string name, string action, string ip, string reqParams, Int64 operationTime, string respStr) {
            OpLogEntity opLogEntity = new OpLogEntity();
            opLogEntity.userName = name;
            opLogEntity.action = action;
            opLogEntity.ip = ip;
            opLogEntity.reqParams = reqParams;
            opLogEntity.operationTime = operationTime;
            opLogEntity.respStr = respStr;
            return opLogEntity;
        }
    }
}
