using UnityEngine;
using SceneManagement;

namespace Tools
{
    public class SeedTool : ToolBase
    {
        #region Constants

        public const int SEED_TOOL_TYPE = 4;

        #endregion

        #region Properties

        public override int ToolId => _seedPrefabId;
        public override float RotationSpeed => 0f;

        #endregion

        #region Fields

        private GameObject _seedObject;
        private SpriteRenderer _seedRenderer;
        private int _seedPrefabId;

        #endregion

        #region Public Methods

        public void SetSeedPrefabId(int prefabId)
        {
            _seedPrefabId = prefabId;
        }

        #endregion

        #region ToolBase Implementation

        public override bool CreateTool()
        {
            if (_sceneDB == null)
            {
                Debug.LogError("SeedTool: SceneDB is null");
                return false;
            }

            string iconPath = _sceneDB.GetIconPath(_seedPrefabId);
            if (string.IsNullOrEmpty(iconPath))
            {
                Debug.LogError($"SeedTool: No iconPath found for prefab ID {_seedPrefabId}");
                return false;
            }

            Sprite seedSprite = LoadSpriteFromPath(iconPath);
            if (seedSprite == null)
            {
                Debug.LogError($"SeedTool: Failed to load sprite from path: {iconPath}");
                return false;
            }

            _seedObject = CreateSeedObject("SeedObject", seedSprite);

            if (_seedObject == null)
            {
                Debug.LogError("SeedTool: Failed to create seed object");
                return false;
            }

            _seedObject.transform.SetParent(_ownerTransform);

            _seedRenderer = _seedObject.GetComponent<SpriteRenderer>();

            _seedObject.SetActive(false);

            Debug.Log($"SeedTool: Created seed tool with icon {iconPath}");
            return true;
        }

        public override void DestroyTool()
        {
            if (_seedObject != null)
            {
                Object.Destroy(_seedObject);
                _seedObject = null;
                _seedRenderer = null;
            }
        }

        public override void OnShow()
        {
            if (_seedObject != null) _seedObject.SetActive(true);
            UpdateIdlePosition();
        }

        public override void OnHide()
        {
            if (_seedObject != null) _seedObject.SetActive(false);
        }

        public override void OnWorking(float deltaTime)
        {
        }

        public override void OnStopWorking()
        {
            UpdateIdlePosition();
        }

        public override void SetSortingOrder(int order)
        {
            if (_seedRenderer != null) _seedRenderer.sortingOrder = order+1;
        }

        #endregion

        #region Protected Methods

        protected override void UpdateIdlePosition()
        {
            if (_seedObject == null || _ownerTransform == null) return;

            float yOffset = 1.45f;
            float xOffset = 0f;  // 居中

            _seedObject.transform.localPosition = new Vector3(xOffset, yOffset, 0f);
        }

        #endregion

        #region Private Methods

        private GameObject CreateSeedObject(string name, Sprite sprite)
        {
            GameObject seedObj = new GameObject(name);
            seedObj.transform.localScale = Vector3.one;
            SpriteRenderer renderer = seedObj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Front";
            renderer.sortingOrder = 0;
            return seedObj;
        }

        private Sprite LoadSpriteFromPath(string path)
        {
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                return sprite;
            }

            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab != null)
            {
                SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    return sr.sprite;
                }
            }

            return null;
        }

        #endregion
    }
}