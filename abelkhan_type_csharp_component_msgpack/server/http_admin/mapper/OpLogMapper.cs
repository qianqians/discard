using abelkhan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using abelkhan.admin.helper;


namespace http_admin
{

    public class OpLogMapper : BaseMapper
    {
        public override string CollectionName()
        {
            return "admin_operator_log";
        }
        public OpLogMapper() { 
        }

        public string generateUid() {
            return System.Guid.NewGuid().ToString("N");
        }

        public Task<List<OpLogEntity>> GetOpLogEntityAsync(string query_json)
        {
            return GetEntityAsync<OpLogEntity>(query_json);
        }

        public Task<List<OpLogEntity>> GetOpLogEntityAsyncEx(string query_json, int skip, int limit)
        {
            return GetEntityAsyncEx<OpLogEntity>(query_json, skip, limit);
        }
    }
}
