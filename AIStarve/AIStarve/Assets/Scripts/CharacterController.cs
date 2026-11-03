using UnityEngine;
using UnityEngine.Tilemaps;
using SceneManagement;
using System.Collections.Generic;
using Tools;
using NPCDialogue;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class CharacterController : MonoBehaviour
{
  #region Constants

private static readonly int ANIM_WALKING = Animator.StringToHash("Walk");
  private static readonly int ANIM_DIRECTION = Animator.StringToHash("Direction");
  private static readonly int ANIM_TOOL_USE = Animator.StringToHash("ToolUse");
  private static readonly int ANIM_CLOTHES = Animator.StringToHash("Colthes");

  #endregion


  #region Inspector

  [Min(0.1f)]
  [SerializeField]
  private float speed = 1;

  [Header("角色动画控制器")]
  [SerializeField]
  private RuntimeAnimatorController runtimeAnimatorController;

  #endregion


  #region Fields

  private Animator _animator;
  private Rigidbody2D _rigidbody;
  private Camera _mainCamera;
  private Tilemap _groundTilemap;
  private Direction directionCur = Direction.None;
  private bool bFKeyUp = false;
  // 数字键1-9检测，控制F键是否允许
  private int fKeyPerfabId = -1;
private SpriteRenderer _spriteRenderer;
  private int _maxCellY = 0;
  private static int _staticMaxCellY = 0;
  public ToolController _toolController;
private int _lastFKeyPerfabId = -1;
  private int _currentKeyIndex = -1;

  #endregion

#region Properties

  public static int MaxCellY => _staticMaxCellY;

  #endregion

  #region Public Methods

  public void SetToolByKeyIndex(int keyIndex)
  {
      if (GameSceneManager.Instance == null || GameSceneManager.Instance.SceneDB == null) return;
      
      int prefabId = GameSceneManager.Instance.SceneDB.GetPrefabIdByKey(keyIndex.ToString());
      fKeyPerfabId = prefabId;
      _currentKeyIndex = keyIndex;
      
      if (fKeyPerfabId != _lastFKeyPerfabId)
      {
          if (_toolController != null)
          {
              _toolController.SetTool(fKeyPerfabId);
              _animator.SetBool(ANIM_TOOL_USE, _toolController.HasTool);
          }
          _lastFKeyPerfabId = fKeyPerfabId;
      }
  }

  #endregion

   #region Sorting Order Methods
   
   /// <summary>
   /// 获取SpriteRenderer的底部世界坐标（考虑精灵高度）
   /// </summary>
   private Vector3 GetBottomWorldPosition(SpriteRenderer renderer)
   {
       try
       {
           if (renderer == null) return Vector3.zero;
           if (renderer.transform == null) return Vector3.zero;
           Vector3 bottomPosition = renderer.transform.position;
           // 检查bounds是否有效
           if (renderer.bounds.size.y > 0)
           {
               bottomPosition.y -= renderer.bounds.size.y / 2f;
           }
           return bottomPosition;
       }
       catch (System.Exception ex)
       {
           Debug.LogError($"CharacterController.GetBottomWorldPosition error for renderer: {renderer?.name} - {ex.Message}");
           return Vector3.zero;
       }
   }

   /// <summary>
   /// 计算场景中所有SpriteRenderer的最大Y单元格值（基于底部位置）
   /// 用于设置Order in Layer的参考基准
   /// </summary>
   private void CalculateMaxCellY()
   {
       if (_groundTilemap == null)
       {
           Debug.LogWarning("CharacterController: Ground Tilemap未找到，无法计算最大Y单元格值");
           _maxCellY = 0;
           return;
       }
       
       SpriteRenderer[] allRenderers = Object.FindObjectsOfType<SpriteRenderer>();
       int maxY = int.MinValue;
       
       foreach (SpriteRenderer renderer in allRenderers)
       {
           if (renderer == null) continue;
           // 排除自己
           if (renderer == _spriteRenderer) continue;
           
           Vector3 bottomPosition = GetBottomWorldPosition(renderer);
           Vector3Int cellPos = _groundTilemap.WorldToCell(bottomPosition);
           if (cellPos.y > maxY) maxY = cellPos.y;
       }
       
       // 如果没找到其他SpriteRenderer，使用自己的Y坐标作为参考
       if (maxY == int.MinValue && _spriteRenderer != null)
       {
           Vector3 ownBottomPosition = GetBottomWorldPosition(_spriteRenderer);
           Vector3Int ownCellPos = _groundTilemap.WorldToCell(ownBottomPosition);
           maxY = ownCellPos.y;
       }
       
        _maxCellY = maxY;
        _staticMaxCellY = maxY;
        Debug.Log($"CharacterController: 计算最大Y单元格值（基于底部位置）= {_maxCellY}");
   }
  
   /// <summary>
   /// 更新角色的Order in Layer基于当前Y位置（基于底部位置）
   /// 规则：Y坐标越小，sortingOrder值越大（从0开始）
   /// </summary>
private void UpdateSortingOrder()
    {
        if (_spriteRenderer == null || _groundTilemap == null) return;
        
        Vector3 bottomPosition = GetBottomWorldPosition(_spriteRenderer);
        Vector3Int cellPos = _groundTilemap.WorldToCell(bottomPosition);
        int sortingOrder = _maxCellY - cellPos.y;
        int characterOrder = sortingOrder - 1;
        _spriteRenderer.sortingOrder = characterOrder;
        
        // 设置工具层级为人物层级 + 1
        if (_toolController != null)
        {
            _toolController.SetSortingOrder(characterOrder);
        }
    }
    
    #endregion
    
     #region MonoBehaviour

    private void Awake()
  {
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody2D>();
    _mainCamera = Camera.main;

    int playerLayer = LayerMask.NameToLayer("Player");
    if (playerLayer >= 0)
        gameObject.layer = playerLayer;

    if (GetComponent<NpcTalkSelect>() == null)
        gameObject.AddComponent<NpcTalkSelect>();
    
    // 获取地面Tilemap引用
    GameObject groundTilemapObj = GameObject.Find("Ground Tilemap");
    if (groundTilemapObj != null)
    {
        _groundTilemap = groundTilemapObj.GetComponent<Tilemap>();
        if (_groundTilemap == null)
        {
            Debug.LogError("Ground Tilemap对象缺少Tilemap组件");
        }
    }
    else
    {
        Debug.LogError("未找到Ground Tilemap对象");
    }
    
    // 获取SpriteRenderer组件
    _spriteRenderer = GetComponent<SpriteRenderer>();
if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogWarning("CharacterController: 未找到SpriteRenderer组件，将无法设置Order in Layer");
            }
        }
        
_toolController = GetComponent<ToolController>();
        if (_toolController == null)
        {
            _toolController = gameObject.AddComponent<ToolController>();
        }
        
        UIManager.Instance.RegisterEvent(UIEventType.Event.ToolKeyPressed, OnToolKeyPressed);
  }

  private void Start()
  {
      // 设置动画控制器
      if (runtimeAnimatorController != null)
      {
          _animator.runtimeAnimatorController = runtimeAnimatorController;
      }
      
      // 计算场景中所有SpriteRenderer的最大Y单元格值
      CalculateMaxCellY();
      
      // 初始设置角色的Order in Layer
UpdateSortingOrder();
  }
  
  private void Update()
  {
      if (Input.GetKeyUp(KeyCode.F))
      {
          bFKeyUp = false;
          
if (_toolController != null && _toolController.IsWorking)
          {
              _toolController.StopWorking();
              _animator.SetBool(ANIM_TOOL_USE, _toolController.HasTool);
          }
      }
  }

  private bool TryInteractWithNPC()
  {
      Vector2 forward = Vector2.zero;
      switch (directionCur)
      {
          case Direction.Up:    forward = Vector2.up; break;
          case Direction.Down:  forward = Vector2.down; break;
          case Direction.Left:  forward = Vector2.left; break;
          case Direction.Right: forward = Vector2.right; break;
      }

      var colliders = Physics2D.OverlapCircleAll(transform.position, 1.8f);
      NPCDialogueController bestNpc = null;
      float bestScore = float.MinValue;

      for (int i = 0; i < colliders.Length; i++)
      {
          var npcCtrl = colliders[i].GetComponentInParent<NPCDialogue.NPCDialogueController>();
          if (npcCtrl == null) continue;

          Vector2 toNpc = (Vector2)npcCtrl.transform.position - (Vector2)transform.position;
          float dist = toNpc.magnitude;
          if (dist > 1.8f) continue;

          float score = 0;

          if (forward.sqrMagnitude > 0.01f)
          {
              float dot = Vector2.Dot(forward.normalized, toNpc.normalized);
              score = dot * 2f;
          }

          score -= dist * 1.5f;

          if (score > bestScore)
          {
              bestScore = score;
              bestNpc = npcCtrl;
          }
      }

      if (bestNpc != null)
      {
          bestNpc.OnInteract();
          return true;
      }
      return false;
  }

  private void FixedUpdate()
  {
      int? direction = null;
    Vector2 velocity = Vector2.zero;
    bool wasMoving = directionCur != Direction.None;

      if (Input.GetKey(KeyCode.UpArrow))
      {
          direction = 2;
          velocity = new Vector2(0, 1);
            directionCur = Direction.Up;
      }
      else if (Input.GetKey(KeyCode.DownArrow))
      {
          direction = 0;
          velocity = new Vector2(0, -1);
            directionCur = Direction.Down;
        }
      else if (Input.GetKey(KeyCode.RightArrow))
      {
          direction = 1;
          velocity = new Vector2(1, 0);
            directionCur = Direction.Right;
      }
      else if (Input.GetKey(KeyCode.LeftArrow))
      {
          direction = 3;
          velocity = new Vector2(-1, 0);
            directionCur = Direction.Left;
      }

bool isMoving = direction.HasValue;
      _rigidbody.linearVelocity = velocity * speed;
      _animator.SetBool(ANIM_WALKING, isMoving);
       


        if (_toolController != null)
      {
          _toolController.SetMoving(isMoving);
          _toolController.SetDirection(directionCur);
          
          // 跑动时 ToolUse = false，停止时根据是否有工具设置
          if (isMoving)
          {
              _animator.SetBool(ANIM_TOOL_USE, false);
              _animator.SetBool(ANIM_CLOTHES, false);
            }
          else if (_toolController.HasTool && bFKeyUp == false)
          {
              _animator.SetBool(ANIM_TOOL_USE, true);
          }
      }
      
      if (isMoving)
      {
          _animator.SetInteger(ANIM_DIRECTION, direction.Value);
          var args = new SceneEventArgs
          {
              Position = transform.position,
              DirectionArg = (Direction)direction.Value
          };
          GameSceneManager.Instance.TriggerEvent(SceneEvent.SelfMove, args, true);
      }
      
if (GameSceneManager.Instance != null && GameSceneManager.Instance.SceneDB != null)
      {
          for (int i = 1; i <= 9; i++)
          {
KeyCode keyCode = KeyCode.Alpha0 + i;
               if (Input.GetKey(keyCode))
               {
                   int prefabId = GameSceneManager.Instance.SceneDB.GetPrefabIdByKey(i.ToString());
                   fKeyPerfabId = prefabId;
                   _currentKeyIndex = i;
                   
                   if (fKeyPerfabId != _lastFKeyPerfabId)
                  {
                      if (_toolController != null)
                      {
                          _toolController.SetTool(fKeyPerfabId);
                          // 设置动画参数：有工具则 true，无工具则 false
                          _animator.SetBool(ANIM_TOOL_USE, _toolController.HasTool);
                      }
                      _lastFKeyPerfabId = fKeyPerfabId;
                  }
                  break;
              }
          }
      }
      
if (Input.GetKey(KeyCode.F) && bFKeyUp == false)
      {
            bFKeyUp = true;

            // 先检测前方是否有NPC可交互
            if (TryInteractWithNPC())
            {
                return;
            }
            
if (_toolController != null && _toolController.HasTool)
            {
                _toolController.StartWorking();
                _animator.SetBool(ANIM_TOOL_USE, false);
            }
            
                Vector3 bottomPosition = GetBottomWorldPosition(_spriteRenderer);
                Vector3Int cellPosition = _groundTilemap.WorldToCell(bottomPosition);
            
            Vector3 cellCenterPosition = _groundTilemap.GetCellCenterWorld(cellPosition);
            
            Vector3 cellSize = _groundTilemap.cellSize;
            
            var argsTemp = new SceneEventArgs
            {
                Position = cellCenterPosition,
                DirectionArg = directionCur,
                PerfabPathId = fKeyPerfabId,
                KeyIndex = _currentKeyIndex
            };
            GameSceneManager.Instance.TriggerEvent(SceneEvent.Working, argsTemp, true);
            
Debug.Log($"F键按下 - 脚下格子: {cellPosition}, 格子中心: {cellCenterPosition}, 格子大小: {cellSize}");
        }
        
        if (Input.GetKey(KeyCode.H))
        {
            _animator.SetBool(ANIM_CLOTHES, true);
        }
       
       // 更新角色的Order in Layer
       UpdateSortingOrder();

   }

  private void LateUpdate()
  {
    if (_mainCamera == null) return;
    
    Vector3 targetPos = transform.position;
    targetPos.z = _mainCamera.transform.position.z;
    
_mainCamera.transform.position = Vector3.Lerp(
      _mainCamera.transform.position,
      targetPos,
      Time.deltaTime * 5f
    );
  }

  private void OnDestroy()
  {
      UIManager.Instance?.UnregisterEvent(UIEventType.Event.ToolKeyPressed, OnToolKeyPressed);
  }

  private void OnToolKeyPressed(UIEventType.EventArgs args)
  {
      if (args.KeyIndex > 0)
      {
          SetToolByKeyIndex(args.KeyIndex);
      }
  }

  #endregion


}