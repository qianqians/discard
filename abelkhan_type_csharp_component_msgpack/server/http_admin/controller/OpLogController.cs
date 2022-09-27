using abelkhan.admin;
using abelkhan.admin.helper;
using System.Collections.Generic;
using System;
using System.Linq;

namespace http_admin
{
    [Controller("/oplog")]
    public class OpLogController : ActionSupport
    {
        [AutoWired]
        private OpLogMapper opLogMapper = null;

        [Post("/list")]
        [RequirePermissions(Permission.OP_LOG)]
        public JSON GetList()
        {
            OpLogListParam listParam = getParams<OpLogListParam>();

            string condition = "{}";
            if (listParam.userName != null && !"".Equals(listParam.userName)) {
                condition = "{userName: \"" + listParam.userName + "\"}";
            }
            uint count = opLogMapper.GetObjectCountAsync(condition).GetAwaiter().GetResult();
            OpLogList list = new OpLogList();
            int limit = listParam.limit > 100 ? 100 : listParam.limit;
            int skin = (listParam.page - 1) * limit;
            List<OpLogEntity> _dataList = opLogMapper.GetOpLogEntityAsyncEx(condition, skin, limit).GetAwaiter().GetResult();
            foreach (OpLogEntity item in _dataList)
            {
                list.add(item);
            }
            list.total = (int)count;
            return Result<OpLogList>.Res(list).ToJson();
        }
    }
}
