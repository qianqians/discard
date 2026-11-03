using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using NavMeshPlus.Extensions;
using System.Linq;

namespace NPCDialogue
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AgentOverride2d))]
    public class NPCDialogueController : MonoBehaviour
    {
        [SerializeField] public string npcId;

        private NPCDialogueManager _manager;
        private Animator _animator;
        private NavMeshAgent _agent;
        private SpriteRenderer _spriteRenderer;
        private Tilemap _groundTilemap;
        private int _maxCellY;
        private Vector2 _targetPosition;
        private SceneManagement.Direction _moveDirection;

        private static readonly int ANIM_WALKING = Animator.StringToHash("Walk");
        private static readonly int ANIM_DIRECTION = Animator.StringToHash("Direction");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _targetPosition = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var groundObj = GameObject.Find("Ground Tilemap");
            if (groundObj != null)
                _groundTilemap = groundObj.GetComponent<Tilemap>();
        }

        private void Start()
        {
            _manager = NPCDialogueManager.Instance;
            if (!string.IsNullOrEmpty(npcId))
                _manager.RegisterNPCController(this);
            CalculateMaxCellY();
        }

        private void OnDestroy()
        {
            if (_manager != null && !string.IsNullOrEmpty(npcId))
                _manager.UnregisterNPCController(npcId);
        }

        public void OnInteract()
        {
            if (_manager == null || string.IsNullOrEmpty(npcId))
                return;

            var dialogues = _manager.GetDialoguesByTrigger(npcId, TriggerType.interact);
            if (dialogues.Count == 0) return;

            _manager.TriggerDialogue(npcId, dialogues.First().dialogueId);
        }

        public void MoveToPosition(Vector3 target)
        {
            SetMoveParameters(target, SceneManagement.Direction.None);
        }

        public void SetMoveParameters(Vector2 position, SceneManagement.Direction direction)
        {
            _targetPosition = position;
            _moveDirection = direction;
            _agent.SetDestination(position);
        }

        public bool HasArrived()
        {
            return !_agent.pathPending
                && _agent.remainingDistance <= _agent.stoppingDistance
                && (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f);
        }

        private int _currentAnimDir;
        private int _faceOverrideDir = -1;

        private Vector3 GetBottomWorldPosition(SpriteRenderer renderer)
        {
            if (renderer == null || renderer.sprite == null) return transform.position;
            Bounds bounds = renderer.sprite.bounds;
            return renderer.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, 0));
        }

        private void CalculateMaxCellY()
        {
            if (_groundTilemap == null) return;
            var allRenderers = Object.FindObjectsOfType<SpriteRenderer>();
            int maxY = int.MinValue;
            foreach (var renderer in allRenderers)
            {
                if (renderer == null || renderer == _spriteRenderer) continue;
                Vector3 bottomPos = GetBottomWorldPosition(renderer);
                Vector3Int cellPos = _groundTilemap.WorldToCell(bottomPos);
                if (cellPos.y > maxY) maxY = cellPos.y;
            }
            if (maxY == int.MinValue && _spriteRenderer != null)
            {
                Vector3 bottomPos = GetBottomWorldPosition(_spriteRenderer);
                Vector3Int cellPos = _groundTilemap.WorldToCell(bottomPos);
                maxY = cellPos.y;
            }
            _maxCellY = maxY;
        }

        private void UpdateSortingOrder()
        {
            if (_spriteRenderer == null || _groundTilemap == null) return;
            Vector3 bottomPos = GetBottomWorldPosition(_spriteRenderer);
            Vector3Int cellPos = _groundTilemap.WorldToCell(bottomPos);
            int sortingOrder = _maxCellY - cellPos.y;
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        private void Update()
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            transform.position = pos;
            UpdateSortingOrder();

            if (!_agent.pathPending && _agent.remainingDistance > _agent.stoppingDistance)
            {
                Vector2 vel = _agent.velocity;
                bool isMoving = vel.sqrMagnitude > 0.1f;
                if (isMoving)
                {
                    SetAnimDir(AngleToAnimatorInt(vel));
                }
                else
                {
                    _animator.SetBool(ANIM_WALKING, false);
                    if (_faceOverrideDir >= 0)
                    {
                        _currentAnimDir = _faceOverrideDir;
                        _animator.SetInteger(ANIM_DIRECTION, _faceOverrideDir);
                    }
                }
            }
            else
            {
                _animator.SetBool(ANIM_WALKING, false);
                if (_faceOverrideDir >= 0)
                {
                    _currentAnimDir = _faceOverrideDir;
                    _animator.SetInteger(ANIM_DIRECTION, _faceOverrideDir);
                }
            }
        }

        public void SetFaceDir(int dir)
        {
            _faceOverrideDir = dir;
        }

        private void SetAnimDir(int dir)
        {
            _animator.SetBool(ANIM_WALKING, true);
            _faceOverrideDir = -1;
            if (dir != _currentAnimDir)
            {
                _currentAnimDir = dir;
                _animator.SetInteger(ANIM_DIRECTION, dir);
            }
        }

        private static int AngleToAnimatorInt(Vector2 v)
        {
            if (v.sqrMagnitude < 0.0001f) return 0;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            if (angle < -135 || angle > 135) return 3;
            if (angle < -45) return 0;
            if (angle < 45) return 1;
            return 2;
        }
    }
}
