using UnityEngine;
using SceneManagement;

namespace Tools
{
    public abstract class ToolBase
    {
        protected GameObject _toolObject;
        protected Transform _toolTransform;
        protected Transform _ownerTransform;
        protected SpriteRenderer _spriteRenderer;
        protected SceneDataBase _sceneDB;
        protected Direction _characterDirection = Direction.Down;

        #region Properties

        public abstract int ToolId { get; }
        
        public virtual Vector3 HeadOffset => new Vector3(0f, 0.5f, 0f);
        
        public virtual float RotationSpeed => 360f;
        
        public bool IsVisible => _toolObject != null && _toolObject.activeSelf;

        #endregion

        #region Lifecycle Methods

        public virtual void Initialize(Transform ownerTransform, SceneDataBase sceneDB)
        {
            _ownerTransform = ownerTransform;
            _sceneDB = sceneDB;
        }

        public virtual bool CreateTool()
        {
            string[] paths = _sceneDB?.GetPrefabPaths(ToolId);
            if (paths == null || paths.Length == 0)
            {
                Debug.LogError($"ToolBase: No prefab paths found for tool ID {ToolId}");
                return false;
            }

            string prefabPath = paths[0];
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"ToolBase: Failed to load prefab at path: {prefabPath}");
                return false;
            }

            _toolObject = Object.Instantiate(prefab);
            _toolTransform = _toolObject.transform;
            _toolTransform.SetParent(_ownerTransform);
            
            _spriteRenderer = _toolObject.GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = _toolObject.GetComponentInChildren<SpriteRenderer>();
            }

            _toolObject.SetActive(false);
            
            Debug.Log($"ToolBase: Created tool {GetType().Name} with prefab {prefabPath}");
            return true;
        }

        public virtual void DestroyTool()
        {
            if (_toolObject != null)
            {
                Object.Destroy(_toolObject);
                _toolObject = null;
                _toolTransform = null;
                _spriteRenderer = null;
            }
        }

        public virtual void OnShow()
        {
            if (_toolObject != null)
            {
                _toolObject.SetActive(true);
                UpdateIdlePosition();
            }
        }

        public virtual void OnHide()
        {
            if (_toolObject != null)
            {
                _toolObject.SetActive(false);
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnWorking(float deltaTime)
        {
        }

        public virtual void OnStopWorking()
        {
            UpdateIdlePosition();
        }

        #endregion

        #region Public Methods

        public void SetDirection(Direction direction)
        {
            _characterDirection = direction;
            OnDirectionChanged();
        }

        public virtual void SetSortingOrder(int order)
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.sortingOrder = order;
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void UpdateIdlePosition()
        {
            if (_toolTransform == null || _ownerTransform == null) return;

            Vector3 offset = HeadOffset;
            
            switch (_characterDirection)
            {
                case Direction.Left:
                    offset.x = -0.3f;
                    break;
                case Direction.Right:
                    offset.x = 0.3f;
                    break;
                case Direction.Up:
                    offset.y = 0.6f;
                    break;
                case Direction.Down:
                    offset.y = 0.4f;
                    break;
            }

            _toolTransform.localPosition = offset;
            _toolTransform.localRotation = Quaternion.identity;
        }

        protected virtual void OnDirectionChanged()
        {
            if (IsVisible)
            {
                UpdateIdlePosition();
            }
        }

        protected float GetDirectionSign()
        {
            return _characterDirection == Direction.Left ? 1f : -1f;
        }

        #endregion
    }
}