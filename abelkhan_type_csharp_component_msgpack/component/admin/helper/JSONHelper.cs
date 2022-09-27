using Newtonsoft.Json;

namespace abelkhan.admin.helper
{
    public class JSONHelper
    {
        public static string serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T deserialize<T>(string str) {
           return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
