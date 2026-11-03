using UnityEngine;
using SceneManagement;

namespace Tools
{
    public class HoeTool : ToolBase
    {
        #region Constants

        public const int HOE_TOOL_ID = 19;

        #endregion

        #region Properties

        public override int ToolId => HOE_TOOL_ID;
        public override float RotationSpeed => 540f;

        #endregion

        #region Fields

        private float _currentAngle = 0f;
        private float _toolLength = 0f;
        private bool _isWorking = false;
        private GameObject _pivotObject;
        private Transform _pivotTransform;

        #endregion

        #region ToolBase Implementation

        public override void OnShow()
        {
            base.OnShow();
            
            if (_spriteRenderer != null)
            {
                _toolLength = _spriteRenderer.bounds.size.y;
                if (_toolLength <= 0f)
                {
                    _toolLength = 1f;
                }
            }
            else
            {
                _toolLength = 1f;
            }
            
            if (_pivotObject == null)
            {
                _pivotObject = new GameObject("ToolPivot");
                _pivotTransform = _pivotObject.transform;
                _pivotTransform.SetParent(_ownerTransform);
            }
            
            _currentAngle = 0f;
            _isWorking = false;
            
            ResetToolTransform();
            UpdateIdlePosition();
        }

        public override void OnWorking(float deltaTime)
        {
            if (_toolTransform == null || _pivotTransform == null) return;
            
            // 第一次进入工作状态时重置
            if (!_isWorking)
            {
                ResetToolTransform();
            }
            
            _isWorking = true;
            
            float directionSign = GetDirectionSign();
            _currentAngle += RotationSpeed * deltaTime * directionSign;
            
            UpdateSwingPosition();
        }

        public override void OnStopWorking()
        {
            _isWorking = false;
            _currentAngle = 0f;
            ResetToolTransform();
            UpdateIdlePosition();
        }

        public override void DestroyTool()
        {
            if (_pivotObject != null)
            {
                Object.Destroy(_pivotObject);
                _pivotObject = null;
                _pivotTransform = null;
            }
            base.DestroyTool();
        }

        #endregion

        #region Protected Methods

protected override void UpdateIdlePosition()
        {
            if (_toolTransform == null || _ownerTransform == null || _pivotTransform == null) return;

            // 所有方向静止时都相同：旋转90度，锄头横着，居中偏上
            Vector3 pivotPos = new Vector3(0f, 0.9f, 0f);
            float rotationZ = 90f;

            _pivotTransform.localPosition = pivotPos;
            _pivotTransform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
            
            _toolTransform.SetParent(_pivotTransform);
            _toolTransform.localPosition = Vector3.zero;  // 居中，不偏移
            _toolTransform.localRotation = Quaternion.identity;
            
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipY = false;
                _spriteRenderer.flipX = false;
            }
        }

        #endregion

        #region Private Methods

        private void ResetToolTransform()
        {
            if (_pivotTransform == null || _toolTransform == null) return;
            
            // 重置 pivot 的位置和旋转
            _pivotTransform.localPosition = Vector3.zero;
            _pivotTransform.localRotation = Quaternion.identity;
            
            // 重置工具相对于 pivot 的位置和旋转
            _toolTransform.localPosition = Vector3.zero;
            _toolTransform.localRotation = Quaternion.identity;
            
            // 重置翻转
            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = false;
            }
        }

        private void UpdateSwingPosition()
        {
            if (_pivotTransform == null) return;
            
            // 工作时使用横向偏移
            Vector3 pivotPos;
            float baseRotation;
            
            switch (_characterDirection)
            {
                case Direction.Left:
                    pivotPos = new Vector3(-0.3f + _toolLength * 0.5f, 0.5f, 0f);  // pivot在右侧（靠近人物）
                    baseRotation = 90f;  // 正Y指向左（远离人物，即外）
                    break;
                case Direction.Right:
                    pivotPos = new Vector3(0.3f - _toolLength * 0.5f, 0.5f, 0f);  // pivot在左侧（靠近人物）
                    baseRotation = -90f;  // 正Y指向右（远离人物，即外）
                    break;
                case Direction.Up:
                case Direction.Down:
                    pivotPos = new Vector3(0f, 0.6f - _toolLength * 0.5f, 0f);
                    baseRotation = 0f;
                    break;
                
                    // 向下时和静止时一样，工具朝上显示
                    pivotPos = new Vector3(0f, 0.6f - _toolLength * 0.5f, 0f);
                    baseRotation = 0f;
                    break;
                default:
                    pivotPos = new Vector3(0f, 0.5f, 0f);
                    baseRotation = -90f;
                    break;
            }
            
            _pivotTransform.localPosition = pivotPos;
            _pivotTransform.localRotation = Quaternion.Euler(0f, 0f, baseRotation + _currentAngle);
            
            // 设置工具位置 - 让工具尾部固定在 pivot 位置，锄头口朝外
            _toolTransform.localPosition = new Vector3(0f, _toolLength * 0.5f, 0f);
            
            // 工作时翻转工具，让锄头口朝外
            if (_spriteRenderer != null)
            {
                // 向左时 flipX=true 让锄头朝左（外），向右时 flipX=false 让锄头朝右（外）
                _spriteRenderer.flipX = (_characterDirection == Direction.Right);
            }
        }

        #endregion
    }
}