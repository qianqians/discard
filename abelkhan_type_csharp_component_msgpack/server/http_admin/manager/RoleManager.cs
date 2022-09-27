using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace http_admin
{
    public class RoleManager
    {
        public Dictionary<string, RoleEntity> Roles { set; get; }

        public List<RoleEntity> GetList() {
            return Roles.Values.ToList<RoleEntity>();
        }

        public RoleEntity GetEntityByKey(string key) {
            return Roles.GetValueOrDefault(key);
        }

        public void AddAll(List<RoleEntity> list) {
            Roles = list.ToDictionary(entity => entity.key, entity => entity);
        }

        public void Add(RoleEntity entity) {
            Roles.Add(entity.key, entity);
        }
    }
}
