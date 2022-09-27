using abelkhan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using abelkhan.admin.helper;


namespace http_admin
{

    public class RoleMapper : BaseMapper
    {
        public override string CollectionName()
        {
            return "admin_role";
        }
        public RoleMapper() { 
        }

        public string generateUid() {
            return System.Guid.NewGuid().ToString("N");
        }

        public Task<List<RoleEntity>> GetRoleEntityAsync(string query_json)
        {
            return GetEntityAsync<RoleEntity>(query_json);
        }
    }
}
