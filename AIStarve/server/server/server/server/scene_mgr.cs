using Abelkhan;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class SceneMgr()
    {
        private Dictionary<string, Scene> scenes = new();
        public Dictionary<string, Scene> Scenes
        {
            get
            {
                return scenes;
            }

            private set { }
        }

        private Dictionary<string, Scene> clientScenes = new();
        public Dictionary<string, Scene> ClientScenes
        {
            get
            {
                return clientScenes;
            }

            private set { }
        }

        public Scene CreateScene(string sceneName)
        {
            var scene = Scene.CreateScene(sceneName);
            scenes.Add(scene.sceneID, scene);
            return scene;
        }

        public async Task<Scene> LoadScene(string sceneName, string sceneID)
        {
            var scene = await Scene.LoadScene(sceneName, sceneID); 
            scenes.Add(scene.sceneID, scene);
            return scene;
        }

        public Scene GetScene(string sceneID)
        {
            return scenes.GetValueOrDefault(sceneID, null);
        }

        public async Task<Scene> PlayerIntoScene(string sceneID, string userGuid, string userName, string userTableID, string userNetUUID)
        {
            Log.Log.trace("PlayerIntoScene sceneID:{0} begin!", sceneID);

            Scene scene = null;
            if (scenes.TryGetValue(sceneID, out scene))
            {
                await scene.PlayerIntoScene(userGuid, userName, userTableID, userNetUUID);
                clientScenes[userNetUUID] = scene;
            }
            Log.Log.trace("PlayerIntoScene sceneID:{0} end!", sceneID);
            return scene;
        }

        public void ClientDisconnect(string uuid)
        {
            if (clientScenes.TryGetValue(uuid, out var scene))
            {
                scene.PlayerLevelScene(uuid);
                clientScenes.Remove(uuid);
            }
        }

        public void NtfTimeInfo(TimeInfo info)
        {
            foreach (var scene in scenes.Values)
            {
                scene.NtfTimeInfo(info);
            }
        }
    }
}