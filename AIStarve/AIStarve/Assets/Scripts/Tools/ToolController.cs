using UnityEngine;
using SceneManagement;

namespace Tools
{
    public class ToolController : MonoBehaviour
    {
        #region Fields

        private ToolBase _currentTool;
        private int _currentToolId = -1;
        private bool _isWorking = false;
        private bool _isMoving = false;
        private Direction _characterDirection = Direction.Down;
        private SceneDataBase _sceneDB;

        #endregion

        #region Properties

        public bool HasTool => _currentTool != null;
        public int CurrentToolId => _currentToolId;
        public bool IsWorking => _isWorking;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            if (GameSceneManager.Instance != null)
            {
                _sceneDB = GameSceneManager.Instance.SceneDB;
            }
            
            if (_sceneDB == null)
            {
                Debug.LogWarning("ToolController: SceneDB not found in Start");
            }
        }

        private void Update()
        {
            if (_currentTool != null && _isWorking)
            {
                _currentTool.OnWorking(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (_currentTool != null)
            {
                _currentTool.OnUpdate(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            ClearTool();
        }

        #endregion

        #region Public Methods

        public void SetTool(int prefabId)
        {
            if (prefabId == _currentToolId && _currentTool != null)
            {
                return;
            }

            ClearTool();

            if (prefabId <= 0)
            {
                _currentToolId = -1;
                return;
            }

            if (_sceneDB == null)
            {
                if (GameSceneManager.Instance != null)
                {
                    _sceneDB = GameSceneManager.Instance.SceneDB;
                }
                
                if (_sceneDB == null)
                {
                    Debug.LogError("ToolController: SceneDB is null, cannot create tool");
                    return;
                }
            }

            // 检查是否是 SeedTool (type == 4)
            if (ToolFactory.IsSeedTool(prefabId, _sceneDB))
            {
                _currentTool = ToolFactory.CreateSeedTool(prefabId, transform, _sceneDB);
                
                if (_currentTool != null)
                {
                    _currentToolId = prefabId;
                    _currentTool.SetDirection(_characterDirection);
                    
                    if (!_isMoving)
                    {
                        _currentTool.OnShow();
                    }
                    
                    Debug.Log($"ToolController: SeedTool set to ID {prefabId}");
                }
                else
                {
                    _currentToolId = -1;
                }
                return;
            }

            if (!ToolFactory.IsTool(prefabId))
            {
                Debug.Log($"ToolController: Prefab ID {prefabId} is not a tool type");
                _currentToolId = -1;
                return;
            }

            _currentTool = ToolFactory.CreateTool(prefabId, transform, _sceneDB);
            
            if (_currentTool != null)
            {
                _currentToolId = prefabId;
                _currentTool.SetDirection(_characterDirection);
                
                if (!_isMoving)
                {
                    _currentTool.OnShow();
                }
                
                Debug.Log($"ToolController: Tool set to ID {prefabId}");
            }
            else
            {
                _currentToolId = -1;
            }
        }

        public void ClearTool()
        {
            if (_currentTool != null)
            {
                _currentTool.DestroyTool();
                _currentTool = null;
            }
            _currentToolId = -1;
            _isWorking = false;
            
            Debug.Log("ToolController: Tool cleared");
        }

        public void StartWorking()
        {
            if (_currentTool == null)
            {
                Debug.LogWarning("ToolController: Cannot start working - no tool equipped");
                return;
            }

            _isWorking = true;
            Debug.Log("ToolController: Started working");
        }

        public void StopWorking()
        {
            if (_currentTool != null && _isWorking)
            {
                _isWorking = false;
                _currentTool.OnStopWorking();
                Debug.Log("ToolController: Stopped working");
            }
        }

        public void SetMoving(bool isMoving)
        {
            if (_isMoving == isMoving) return;
            
            _isMoving = isMoving;

            if (_currentTool != null)
            {
                if (_isMoving)
                {
                    _currentTool.OnHide();
                }
                else if (!_isWorking)
                {
                    _currentTool.OnShow();
                }
            }
        }

        public void SetDirection(Direction direction)
        {
            if (_characterDirection == direction) return;
            
            _characterDirection = direction;

            if (_currentTool != null)
            {
                _currentTool.SetDirection(direction);
            }
        }

        public void SetSortingOrder(int order)
        {
            if (_currentTool != null)
            {
                _currentTool.SetSortingOrder(order);
            }
        }

        #endregion
    }
}