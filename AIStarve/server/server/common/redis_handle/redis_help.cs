
using System.Reflection.Metadata;

namespace Abelkhan
{
    public class RedisHelp
    {
        public static string BuildPlayerSceneHubKey(string userGuid)
        {
            return $"Server:PlayerSceneHub:{userGuid}";
        }

        public static string BuildTimeInfoKey()
        {
            return $"Server:GameWorld:TimeInfo";
        }

        public static string BuildLoadSceneLockKey()
        {
            return $"Server:LoadSceneLockKey";
        }

        public static string BuildSceneInfoListKey()
        {
            return $"Server:SceneInfoListKey";
        }
    }
}
