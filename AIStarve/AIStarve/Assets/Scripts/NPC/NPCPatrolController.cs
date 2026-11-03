using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using SceneManagement;
using NPCDialogue;

public class NPCPatrolController : MonoBehaviour
{
    private NPCDialogueController _npcCtrl;
    private Animator _animator;
    private NavMeshAgent _agent;
    private NPCPatrolConfig _config;
    private List<Vector3> _targetPositions = new List<Vector3>();
    private int _currentIndex;
    private bool _isWaiting;
    private bool _paused;
    private bool _started;
    private float _waitTimer;
    private Vector2 _fixedOffset;

    private static Dictionary<int, List<Vector2>> _areaSpreadPoints = new Dictionary<int, List<Vector2>>();
    private const float SPREAD_RADIUS = 2.5f;
    private const float MIN_SPACING = 1.2f;

    private static readonly int ANIM_WALKING = Animator.StringToHash("Walk");
    private static readonly int ANIM_DIRECTION = Animator.StringToHash("Direction");
    private const float ARRIVAL_DISTANCE = 1.5f;

    private static Vector2 GeneratePoissonOffset(int areaId)
    {
        if (!_areaSpreadPoints.ContainsKey(areaId))
            _areaSpreadPoints[areaId] = new List<Vector2>();

        var existing = _areaSpreadPoints[areaId];
        Vector2 offset;
        int attempts = 0;
        do
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float dist = Mathf.Sqrt(Random.Range(0f, 1f)) * SPREAD_RADIUS;
            offset = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);
            attempts++;
        }
        while (attempts < 50 && existing.Any(p => Vector2.Distance(p, offset) < MIN_SPACING));

        existing.Add(offset);
        return offset;
    }

    private void Awake()
    {
        _npcCtrl = GetComponent<NPCDialogueController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _fixedOffset = new Vector2(Random.Range(-1.6f, 1.6f), Random.Range(-1.6f, 1.6f));
        _agent.autoBraking = false;
    }

    public void Init(NPCPatrolConfig config)
    {
        _config = config;
        _agent.stoppingDistance = 0.5f;
        _agent.speed = config.speed;
        ResolveTargets();
    }

    private void OnDestroy()
    {
        _areaSpreadPoints.Clear();
    }

    private void ResolveTargets()
    {
        _targetPositions.Clear();
        if (_config?.triggerAreaIds == null) return;

        var textAsset = Resources.Load<TextAsset>("TriggerAreaMapping");
        if (textAsset == null) return;

        var mapping = JsonUtility.FromJson<TriggerAreaMappingData>(textAsset.text);
        if (mapping?.triggerAreas == null) return;

        var idToName = new Dictionary<int, string>();
        foreach (var area in mapping.triggerAreas)
            idToName[area.id] = area.gameObjectName;

        foreach (int id in _config.triggerAreaIds)
        {
            if (idToName.TryGetValue(id, out string goName))
            {
                var go = GameObject.Find(goName);
                if (go != null)
                    _targetPositions.Add(go.transform.position + (Vector3)GeneratePoissonOffset(id));
            }
        }
    }

    public void Pause()
    {
        _paused = true;
        _agent.isStopped = true;
        if (_animator != null)
            _animator.SetBool(ANIM_WALKING, false);
    }

    public void Resume()
    {
        _paused = false;
        _agent.isStopped = false;
    }

    public void MoveToDestination(Vector3 target)
    {
        _paused = false;
        _agent.isStopped = false;
        _isWaiting = false;
        _agent.SetDestination(target);
    }

    public bool HasArrivedAtMoveTarget()
    {
        return _npcCtrl == null || _npcCtrl.HasArrived();
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        if (_paused || _config == null || _targetPositions.Count == 0)
        {
            UpdateAnimation();
            return;
        }

        if (_npcCtrl != null && !_npcCtrl.HasArrived())
        {
            UpdateAnimation();
            return;
        }

        if (!_started)
        {
            _started = true;
            MoveToNext();
            return;
        }

        if (HasArrivedAtPatrolPoint())
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                _waitTimer = _config.waitSeconds;
                if (_targetPositions.Count > 1)
                    transform.position += new Vector3(_fixedOffset.x, _fixedOffset.y, 0);
            }

            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                MoveToNext();
            }
        }

        UpdateAnimation();
    }

    private bool HasArrivedAtPatrolPoint()
    {
        return !_agent.pathPending
            && _agent.remainingDistance <= ARRIVAL_DISTANCE
            && _agent.velocity.sqrMagnitude < 0.1f;
    }

    private void MoveToNext()
    {
        if (_targetPositions.Count == 0) return;
        _agent.SetDestination(_targetPositions[_currentIndex]);
        _currentIndex++;
        if (_currentIndex >= _targetPositions.Count)
            _currentIndex = _config.loop ? 0 : _targetPositions.Count - 1;
    }

    private void UpdateAnimation()
    {
        if (_animator == null || _paused)
        {
            if (_animator != null) _animator.SetBool(ANIM_WALKING, false);
            return;
        }

        Vector2 vel = _agent.velocity;
        bool isMoving = vel.sqrMagnitude > 0.1f;
        int dir = 0;
        if (isMoving)
        {
            if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
                dir = vel.x > 0 ? 1 : 3;
            else
                dir = vel.y > 0 ? 2 : 0;
        }

        _animator.SetBool(ANIM_WALKING, isMoving);
        if (isMoving)
            _animator.SetInteger(ANIM_DIRECTION, dir);
    }

    [System.Serializable]
    private class TriggerAreaMappingData
    {
        public List<TriggerAreaEntry> triggerAreas;
    }

    [System.Serializable]
    private class TriggerAreaEntry
    {
        public int id;
        public string gameObjectName;
        public string sceneName;
    }
}
