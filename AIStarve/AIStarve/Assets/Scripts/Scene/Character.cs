using UnityEngine;

namespace SceneManagement
{
    public class Character
    {
        public string CharacterId { get; private set; }
        public string PrefabPath { get; private set; }
        public GameObject GameObject { get; private set; }
        public Vector3 Position => GameObject.transform.position;

        public Character(string characterId, string prefabPath)
        {
            CharacterId = characterId;
            PrefabPath = prefabPath;
        }

        public void Initialize(Vector3 position)
        {
            Debug.Log($"Character.Initialize() called - CharacterId: {CharacterId}, PrefabPath: {PrefabPath}, Position: {position}");
            
            var prefab = Resources.Load<GameObject>(PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load character prefab at {PrefabPath}");
                return;
            }
            
            Debug.Log($"Character prefab loaded successfully: {prefab.name}");

            GameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            GameObject.name = $"Character_{CharacterId}";
            GameObject.SetActive(true);
            
            Debug.Log($"Character {CharacterId} instantiated successfully. GameObject: {GameObject.name}, Active: {GameObject.activeSelf}");
        }

        public void MoveTo(Vector2 newPosition, SceneManagement.Direction direction)
        {
            if (GameObject == null) return;
            RemoteCharacterController characterControl =  GameObject.GetComponent<RemoteCharacterController>();
            characterControl.SetMoveParameters(newPosition, direction);
            //GameObject.transform.position = newPosition;
           
        }

        public void Animate(string animationName)
        {
            // Implementation would depend on the animation system
            // This is a placeholder for actual animation logic
            Debug.Log($"Playing animation {animationName} for character {CharacterId}");
        }
    }
}