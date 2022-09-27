using System;
using System.Collections.Generic;
using System.Text;

namespace http_admin
{
    public class OpLogListParam
    {
        public int page { set; get; }
        public int limit { set; get; }
        public string sort { set; get; }
        public string userName { set; get; }
    }

    public class OpLogList
    {
        public List<OpLogEntity> list;

        public int total;
        public OpLogList()
        {
            list = new List<OpLogEntity>();
        }
        public void add(OpLogEntity info)
        {
            list.Add(info);
            total = list.Count;
        }

        public static OpLogList valueOf(List<OpLogEntity> _list)
        {
            OpLogList list = new OpLogList();
            list.list = _list;
            return list;
        }
    }
}
