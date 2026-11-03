using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Abelkhan;
using System.Linq;
using NPCDialogue;

namespace SceneManagement
{
    public class GameSceneManager : MonoBehaviour
    {
        private static GameSceneManager _instance;
        public static GameSceneManager Instance => _instance;
        
        public SceneDataBase SceneDB { get; private set; }

        private TimeInfo _currentServerTime; // 当前服务器时间

        private Dictionary<string, Character> _characters = new Dictionary<string, Character>();
        private Dictionary<string, SceneObject> _objects = new Dictionary<string, SceneObject>();
        private Dictionary<string, (string[] paths, int prefabPathId, SceneObject sceneObject, TimeInfo growiTime, long ripenTime, int lastDisplayedIndex)> _growUpObjects = new Dictionary<string, (string[], int, SceneObject, TimeInfo, long, int)>();
        
        private RuleTile _landTile; // 缓存LandTile资源
        
        private HashSet<Vector3Int> _occupiedGrowUpCells = new HashSet<Vector3Int>(); // 存储已占用的GrowUp对象单元格
        private Dictionary<Vector2Int, Cultivated> _cultivatedDict = new Dictionary<Vector2Int, Cultivated>(); // 存储耕地对象，key为位置
        
        private Dictionary<SceneEvent, Action<SceneEventArgs>> _eventHandlers =
            new Dictionary<SceneEvent, Action<SceneEventArgs>>();

            
        // 新增：事件缓存队列
        private Queue<(SceneEvent, SceneEventArgs)> _eventQueue = new Queue<(SceneEvent, SceneEventArgs)>();
        private bool _isProcessingEvents = false;

        private void Awake()
        {
            Debug.Log($"GameSceneManager.Awake() called - Instance check: {_instance != null}");
            
            if (_instance != null && _instance != this)
            {
                Debug.Log($"GameSceneManager: Destroying duplicate instance");
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            Debug.Log($"GameSceneManager: Instance set successfully, SceneDB initializing...");
            SceneDB = new SceneDataBase();
            
            Debug.Log($"GameSceneManager: Registering event handlers...");
            // Register event handlers
            RegisterEvent(SceneEvent.CharacterMoved, OnCharacterMoved);
            RegisterEvent(SceneEvent.SelfMove, OnSelfMove);
            RegisterEvent(SceneEvent.SendCreateObject, OnSendCreateObject);
            RegisterEvent(SceneEvent.CreateObject, OnCreateObject);
            RegisterEvent(SceneEvent.Working, OnWorking);
            RegisterEvent(SceneEvent.SendCreateGrowUpObject, OnSendCreateGrowUpObject);
            RegisterEvent(SceneEvent.CreateGrowUpObject, OnCreateGrowUpObject);
            RegisterEvent(SceneEvent.CreateCultivatedLand,OnCreateCultivatedLand);
            TriggerEvent(SceneEvent.DBInitFinish, null, true);

            // Subscribe to scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log($"GameSceneManager: Initialization completed successfully!");
        }

        private void Start()
        {
            Debug.Log($"GameSceneManager.Start() called - Instance: {_instance != null}, SceneDB: {SceneDB != null}");
            
            // 如果当前场景是Field，设置状态为GamePlay
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            Debug.Log($"GameSceneManager.Start() - Current Scene: {currentSceneName}");
            
            if (currentSceneName == "Field")
            {
                Debug.Log($"GameSceneManager.Start() - Setting state to GamePlay for Field scene");
                GameStateMachine.Instance.ChangeState(GameStateMachine.PanelState.GamePlay);
            }
            
            // 输出当前游戏状态
            GameStateMachine.PanelState currentState = GameStateMachine.Instance.GetCurState();
            ProcessQueuedEvents();
            Debug.Log($"GameSceneManager.Start() - Current Game State: {currentState}");
                 
        }

    public void RegisterEvent(SceneEvent eventType, Action<SceneEventArgs> handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] += handler;
            }
            else
            {
                _eventHandlers[eventType] = handler;
            }
        }

        public void UnregisterEvent(SceneEvent eventType, Action<SceneEventArgs> handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] -= handler;
            }
        }

        // 修改：添加事件缓存功能
        public void TriggerEvent(SceneEvent eventType, SceneEventArgs args = null, bool immediate = false)
        {
            if (args == null) args = new SceneEventArgs();
            args.EventType = eventType;

            // 如果要求立即处理或者当前状态允许处理
            if (immediate || ShouldProcessEvent(eventType))
            {
                ProcessEvent(eventType, args);
            }
            else
            {
                // 否则加入缓存队列
                _eventQueue.Enqueue((eventType, args));
            }
        }
        
        // 新增：检查当前状态是否应该处理事件
        private bool ShouldProcessEvent(SceneEvent eventType)
        {
            // 这里可以添加特定的事件处理条件
            // 例如：某些事件只在特定游戏状态下处理
            GameStateMachine.PanelState stateTemp = GameStateMachine.Instance.GetCurState();
            //Debug.Log($"GameSceneManager.ShouldProcessEvent() - EventType: {eventType}, CurrentState: {stateTemp}");
            
            if(stateTemp == GameStateMachine.PanelState.MainMenu)
            {
                if(eventType == SceneEvent.CharacterMoved || 
                    eventType == SceneEvent.CreateObject ||
                    eventType == SceneEvent.CreateCultivatedLand ||
                    eventType == SceneEvent.CreateGrowUpObject
                    )
                {
                    //Debug.Log($"GameSceneManager.ShouldProcessEvent() - Allowing {eventType} in GamePlay state");
                    return true;
                }
            }
            //Debug.Log($"GameSceneManager.ShouldProcessEvent() - Rejecting {eventType} in {stateTemp} state");
            return false;
        }
        
        // 新增：处理单个事件
        private void ProcessEvent(SceneEvent eventType, SceneEventArgs args)
        {
            _isProcessingEvents = true;
            
            if (_eventHandlers.TryGetValue(eventType, out var handler))
            {
                handler?.Invoke(args);
            }
            
            _isProcessingEvents = false;
        }

        private void OnSelfMove(SceneEventArgs args)
        {
            // 将SelfMove事件转发为CharacterMoved事件
            Abelkhan.Pos posTemp = new Pos();
            posTemp.X = args.Position.x;
            posTemp.Y = args.Position.y;
            netScene.Move(posTemp, (Abelkhan.Direction)args.DirectionArg);
        }
        
        // 新增：处理缓存队列中的事件
        public void ProcessQueuedEvents()
        {
            if (_isProcessingEvents || _eventQueue.Count == 0)
                return;
                
            while (_eventQueue.Count > 0)
            {
                var (eventType, args) = _eventQueue.Dequeue();
                if (ShouldProcessEvent(eventType))
                {
                    ProcessEvent(eventType, args);
                }
                else
                {
                    // 如果还不能处理，重新放回队列头部
                    var remainingEvents = new List<(SceneEvent, SceneEventArgs)>();
                    remainingEvents.Add((eventType, args));
                    while (_eventQueue.Count > 0)
                    {
                        remainingEvents.Add(_eventQueue.Dequeue());
                    }
                    foreach (var e in remainingEvents)
                    {
                        _eventQueue.Enqueue(e);
                    }
                    break;
                }
            }
        }
        
        private void Update()
        {
           
            
           
        }

        // 其余原有方法保持不变...
        private void OnCharacterMoved(SceneEventArgs args)
        {
            //Debug.Log($"GameSceneManager.OnCharacterMoved() called - ObjectId: {args.ObjectId}, CharacterType: {args.CharacterType}, Position: {args.Position}");
            
            if (!_characters.ContainsKey(args.ObjectId))
            {
                //Debug.Log($"Character {args.ObjectId} not found in dictionary, creating new character...");
                string prefabPath = GetCharacterPrefabPath(args.CharacterType);
                //Debug.Log($"Prefab path for {args.CharacterType}: {prefabPath}");
                CreateCharacter(args.ObjectId, prefabPath, args.Position);
            }
            else
            {
                //Debug.Log($"Character {args.ObjectId} already exists, moving to new position...");
                if(args.CharacterType == CharacterType.Self)//不同步服务器的自己位置
                {
                    //Debug.Log($"Character {args.ObjectId} is Self type, skipping position sync");
                    return;
                }
                var character = _characters[args.ObjectId];
                character.MoveTo(args.Position, args.DirectionArg);
                //Debug.Log($"Character {args.ObjectId} moved to position: {args.Position}");
            }
        }
        
        private string GetCharacterPrefabPath(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Self:
                    return "Perfabs/Characters/SelfCharacter";
                case CharacterType.Friend:
                    return "Perfabs/Characters/OtherCharacter";
                case CharacterType.Enemy:
                    return "Perfabs/Characters/EnemyCharacter";
                default:
                    return "Perfabs/Characters/DefaultCharacter";
            }
        }
        public Character CreateCharacter(string characterId, string prefabPath, Vector3 position)
        {
            Debug.Log($"GameSceneManager.CreateCharacter() called - CharacterId: {characterId}, PrefabPath: {prefabPath}, Position: {position}");
            
            if (_characters.ContainsKey(characterId))
            {
                Debug.LogError($"Character with ID {characterId} already exists");
                return null;
            }
            

            var character = new Character(characterId, prefabPath);
            Debug.Log($"Character object created, initializing at position: {position}");
            character.Initialize(position);
            _characters[characterId] = character;
            
            Debug.Log($"Character {characterId} successfully created and added to dictionary. Total characters: {_characters.Count}");

            // TriggerEvent(SceneEvent.CharacterCreated, new SceneEventArgs {
            //     ObjectId = characterId,
            //     PrefabPath = prefabPath,
            //     Position = position,
            //     GameObject = character.GameObject
            // });

            return character;
        }
        public void OnCreateObject(SceneEventArgs args)//string objectId, int prefabPathID, Vector2 position)
        {
            Debug.Log($"GameSceneManager.OnCreateObject() called - ObjectId: {args.ObjectId}, PrefabPathId: {args.PerfabPathId}, Position: {args.Position}");
            #if IS_TEST
            #else
            if (args.ObjectId != null && _objects.ContainsKey(args.ObjectId))//ouyj test
            {
               Debug.LogError($"Object with ID {args.ObjectId} already exists");
               return ;
            }
            #endif
            var type = (TilemapItemType)SceneDB.GetPrefabType(args.PerfabPathId);
            if (type == TilemapItemType.ObjectsTilemapGrowUpItem)
            {
                Debug.Log($"Grow‑up object detected, triggering SendCreateGrowUpObject for ObjectId: {args.ObjectId}");
                TriggerEvent(SceneEvent.SendCreateGrowUpObject, args, true);
                return;
            }
            var prefabPath = SceneDB.GetPrefabPath(args.PerfabPathId);
            var tilemapPath = SceneDB.GetTilemapTarget(args.PerfabPathId);
            Debug.Log($"GameSceneManager.OnCreateObject() - PrefabPath: {prefabPath}, Type: {type}, TilemapPath: {tilemapPath}");
            
            var sceneObject = new SceneObject(args.ObjectId, prefabPath, type, tilemapPath);
            sceneObject.Initialize(args.Position, true);
            #if IS_TEST
            #else
                _objects[args.ObjectId] = sceneObject;//ouyj test
            #endif
            Debug.Log($"GameSceneManager.OnCreateObject() completed - Object created and added to dictionary");

            

            // TriggerEvent(SceneEvent.ObjectCreated, new SceneEventArgs {
            //     ObjectId = objectId,
            //     PrefabPath = prefabPath,
            //     Position = position,
            //     GameObject = sceneObject.GameObject
            // });

           
        }
public void OnSendCreateGrowUpObject(SceneEventArgs args)
        {
            string objectId = args.ObjectId;
            if (string.IsNullOrEmpty(objectId))
            {
                objectId = System.Guid.NewGuid().ToString();
            }
            Debug.Log($"GameSceneManager.OnSendCreateGrowUpObject() called - ObjectId: {objectId}, PrefabPathId: {args.PerfabPathId}, Position: {args.Position}");
            
            var tilemapPath = SceneDB.GetTilemapTarget(args.PerfabPathId);
            Tilemap tilemap = GetOrCreateTilemap(tilemapPath);
            Vector3Int cellPosition = tilemap.WorldToCell(args.Position);
            Vegetable vegetable = new Vegetable();
            // todo: vegetable.VegetableGuid 这个Guid必须传，使用种菜协议的返回值传回的 public static async Task<List<Vegetable>> grow(Vegetable vegetable, Cultivated cultivated)
            vegetable.perfabId = args.PerfabPathId;
            vegetable.topLeft = new Pos { X = cellPosition.x, Y = cellPosition.y };
            vegetable.bottomRight = new Pos { X = cellPosition.x, Y = cellPosition.y };
            vegetable.growiTime = new TimeInfo
            {
                year = DateTime.Now.Year,
                months = DateTime.Now.Month,
                day = DateTime.Now.Day,
                hour = DateTime.Now.Hour,
                minute = DateTime.Now.Minute,
                second = DateTime.Now.Second
            };
            vegetable.ripenTime = SceneDB.GetPrefabTime(args.PerfabPathId);
            vegetable.VegetableResource = "";
            vegetable.subtype =(System.Int32)SubItemType.Seed;
            vegetable.count = 1;
            // 获取 Cultivated 对象
            Vector2Int key = new Vector2Int(cellPosition.x, cellPosition.y);
            Cultivated cultivated;
            if (!_cultivatedDict.TryGetValue(key, out cultivated) || cultivated == null)
            {
                Debug.LogWarning($"Cannot grow: no cultivated land at position {key}");
                return;
            }
            Debug.Log($"Using existing cultivated at position: {key}");

            // 检查该位置是否有 LandTile
            if (!HasLandTileAtPosition(args.Position))
            {
                Debug.LogWarning($"Cannot create grow-up object: no LandTile at position {args.Position}");
                return;
            }
            
            // 检查该单元格是否已被其他GrowUp对象占用
            if (IsGrowUpCellOccupied(args.Position))
            {
                Debug.LogWarning($"Cannot create grow-up object: cell at position {args.Position} is already occupied by another grow-up object");
                return;
            }
            netScene.grow(vegetable, cultivated);

        }

        public void OnCreateGrowUpObject(SceneEventArgs args)
        {
            string objectId = args.ObjectId;
            if (string.IsNullOrEmpty(objectId))
            {
                objectId = System.Guid.NewGuid().ToString();
            }
            Debug.Log($"GameSceneManager.OnCreateGrowUpObject() called - ObjectId: {objectId}, PrefabPathId: {args.PerfabPathId}, Position: {args.Position}");
            
            // 获取路径数组
            string[] paths = SceneDB.GetPrefabPaths(args.PerfabPathId);
            if (paths == null || paths.Length == 0)
            {
                Debug.LogError($"No prefab paths found for ID {args.PerfabPathId}");
                return;
            }
            
            // 获取类型和 tilemap
            var type = (TilemapItemType)SceneDB.GetPrefabType(args.PerfabPathId);
            var tilemapPath = SceneDB.GetTilemapTarget(args.PerfabPathId);
            
            // 检查该位置是否有 LandTile
            if (!HasLandTileAtPosition(args.Position))
            {
                Debug.LogWarning($"Cannot create grow-up object: no LandTile at position {args.Position}");
                return;
            }
            
            // 检查该单元格是否已被其他GrowUp对象占用
            if (IsGrowUpCellOccupied(args.Position))
            {
                Debug.LogWarning($"Cannot create grow-up object: cell at position {args.Position} is already occupied by another grow-up object");
                return;
            }
            
            // args.Position 已经是对齐后的格子中心位置，直接使用
            Vector3 alignedPosition = args.Position;
            
            // 使用第一个路径创建初始 SceneObject
            var sceneObject = new SceneObject(objectId, paths[0], type, tilemapPath);
            sceneObject.Initialize(alignedPosition, true);
            
            // 标记单元格为已占用
            MarkGrowUpCellOccupied(args.Position);

            // 获取生长时间参数（从服务器传递）
            TimeInfo growiTime = args.GrowiTime;
            long ripenTime = args.RipenTime > 0 ? args.RipenTime : SceneDB.GetPrefabTime(args.PerfabPathId);
            
            // 如果服务器没有提供生长时间，使用当前时间作为后备（但这种情况不应该发生）
            if (growiTime == null)
            {
                Debug.LogWarning($"Grow-up object {objectId} has null growiTime, using local time as fallback");
                growiTime = new TimeInfo
                {
                    year = DateTime.Now.Year,
                    months = DateTime.Now.Month,
                    day = DateTime.Now.Day,
                    hour = DateTime.Now.Hour,
                    minute = DateTime.Now.Minute,
                    second = DateTime.Now.Second
                };
            }
            
            // 存储到成长对象字典（包含时间信息）
            _growUpObjects[objectId] = (paths, args.PerfabPathId, sceneObject, growiTime, ripenTime, 0);

            // 更新显示到正确的生长阶段
            UpdateGrowUpObjectPrefab(objectId);

            Debug.Log($"Grow-up object {objectId} created with {paths.Length} prefab paths, growiTime: {growiTime?.ToString() ?? "null"}, ripenTime: {ripenTime}min.");
        }


        private void OnSendCreateObject(SceneEventArgs args)
        {
            Debug.Log($"GameSceneManager.OnSendCreateObject() called - ObjectId: {args.ObjectId}, PrefabPathId: {args.PerfabPathId}, Position: {args.Position}");
            
            // Notify network about new object with correct types
            SceneChange change = new SceneChange();
            Building item = new Building();
            item.topLeft = new Pos();
            item.topLeft.X =  args.Position.x;
            item.topLeft.Y = args.Position.y;
            item.BuildingId = args.PerfabPathId;
            // todo
            item.BuildingResource = "";
            item.UserGuid = "";
            item.SceneID = "";
            item.BuildingGuid = args.ObjectId;

            change.add_buildings = new List<Building>();
            change.add_buildings.Add(item);
            netScene.ChangeScene(change);
            
            Debug.Log($"GameSceneManager.OnSendCreateObject() completed - Network notification sent");
        }

        /// <summary>
        /// 处理工作事件，在指定位置创建Rule Tile
        /// </summary>
        /// <param name="args">场景事件参数</param>
        public void OnWorking(SceneEventArgs args)
        {
            Debug.Log($"GameSceneManager.OnWorking() called - Position: {args.Position}, PerfabPathId: {args.PerfabPathId}");
            
            // First, check if there's a harvestable vegetable in front of the player
            TryHarvestVegetable(args.Position, args.DirectionArg);
            
            if(args.PerfabPathId == 19)
            {
                
                // 默认行为：创建Rule Tile
                // 获取或创建Tilemap
                Tilemap tilemap = GetOrCreateTilemap("FarmingTilemap");
                if (tilemap == null)
                {
                    Debug.LogError("Failed to get or create tilemap for working");
                    return;
                }
                
                // 将世界坐标转换为格子坐标
                Vector3Int cellPosition = tilemap.WorldToCell(args.Position);
                List<Cultivated> cultivateds = new List<Cultivated>();
                Cultivated temp = new Cultivated() ;
                temp.topLeft = new Pos();
                temp.bottomRight = new Pos();
                temp.topLeft.X = cellPosition.x;
                temp.topLeft.Y = cellPosition.y;
                temp.bottomRight.X = cellPosition.x;
                temp.bottomRight.Y = cellPosition.y;
                cultivateds.Add(temp);
                if (netDriver.SceneHubInfo != null)
                {
                    netScene.Cultivated(cultivateds);
                }
                else
                {
                    Debug.LogWarning("SceneHubInfo is null, skip Cultivated call");
                }
                
            }
            // 如果PerfabPathId有效，检查类型
            else if (args.PerfabPathId > 0)
            {
                var type = (TilemapItemType)SceneDB.GetPrefabType(args.PerfabPathId);
                Debug.Log($"PerfabPathId {args.PerfabPathId} type: {type}");
                if (type == TilemapItemType.ObjectsTilemapItem)
                {

                    
                    // 触发放置预制体事件
                    // Debug.Log($"Triggering SendCreateObject for ObjectsTilemapItem at {args.Position}");
                    // GameSceneManager.Instance.TriggerEvent(SceneEvent.SendCreateObject, new SceneEventArgs
                    // {
                    //     Position = args.Position,
                    //     PerfabPathId = args.PerfabPathId
                    // }, true);
                    TriggerEvent(SceneEvent.CreateObject, new SceneEventArgs
                    {
                       Position = args.Position,
                       PerfabPathId = args.PerfabPathId,
                       DirectionArg = args.DirectionArg
                    }, true);
                    return;
                }
else if( type == TilemapItemType.ObjectsTilemapGrowUpItem)
                {
                    if (args.KeyIndex > 0)
                    {
                        SceneDB.DecreaseKeyAmount(args.KeyIndex.ToString());
                    }
                    
                    Tilemap farmingTilemap = GetOrCreateTilemap("FarmingTilemap");
                    if (farmingTilemap != null)
                    {
                        Vector3Int cellPosition = farmingTilemap.WorldToCell(args.Position);
                        Vector3 alignedPosition = farmingTilemap.GetCellCenterWorld(cellPosition);
                        
                        TriggerEvent(SceneEvent.CreateObject, new SceneEventArgs
                        {
                            Position = alignedPosition,
                            PerfabPathId = args.PerfabPathId,
                            DirectionArg = args.DirectionArg,
                            ObjectId = System.Guid.NewGuid().ToString()
                        }, true);
                    }
                    else
                    {
                        Debug.LogError("FarmingTilemap not found for GrowUp object");
                    }
                }
            }

        }
        private void OnCreateCultivatedLand(SceneEventArgs args)
        {
            // 保存 Cultivated 对象
            if (args.CultivatedData != null)
            {
                Vector2Int key = new Vector2Int((int)args.Position.x, (int)args.Position.y);
                _cultivatedDict[key] = args.CultivatedData;
                Debug.Log($"Cultivated saved at position: {key}");
            }
            
            // 默认行为：创建Rule Tile
            // 获取或创建Tilemap
            Tilemap tilemap = GetOrCreateTilemap("FarmingTilemap");
            if (tilemap == null)
            {
                Debug.LogError("Failed to get or create tilemap for working");
                return;
            }
            Vector3Int cellPosition = new Vector3Int(0,0,0);
            cellPosition.x = (int)args.Position.x;
            cellPosition.y = (int)args.Position.y;
            // 创建Rule Tile并设置到指定位置
            RuleTile ruleTile = CreateWorkingRuleTile();
            if (ruleTile != null)
            {
                tilemap.SetTile(cellPosition, ruleTile);
                Debug.Log($"Rule Tile created at cell position: {cellPosition}");
            }
            else
            {
                Debug.LogError("Failed to create Rule Tile");
            }
        }
        /// <summary>
        /// 获取或创建Tilemap
        /// </summary>
        /// <param name="tilemapName">Tilemap名称</param>
        /// <returns>Tilemap对象</returns>
        public Tilemap GetOrCreateTilemap(string tilemapName)
        {
            // 查找现有的Tilemap
            GameObject tilemapObject = GameObject.Find(tilemapName);
            if (tilemapObject != null)
            {
                return tilemapObject.GetComponent<Tilemap>();
            }
            
            // 创建新的Tilemap
            GameObject newTilemapObject = new GameObject(tilemapName);
            Tilemap newTilemap = newTilemapObject.AddComponent<Tilemap>();
            newTilemapObject.AddComponent<TilemapRenderer>();
            
            // 设置Tilemap的父对象为当前场景的根对象
            newTilemapObject.transform.SetParent(transform);
            
            return newTilemap;
        }
        
        /// <summary>
        /// 创建工作用的Rule Tile
        /// </summary>
        /// <returns>Rule Tile对象</returns>
        private RuleTile CreateWorkingRuleTile()
        {
            // 从SceneDataBase获取id为14的asset路径
            string ruleTilePath = SceneDB.GetPrefabPath(14);
            if (string.IsNullOrEmpty(ruleTilePath))
            {
                Debug.LogError("Failed to get RuleTile path for ID 14");
                return null;
            }

            // 加载RuleTile资源
            RuleTile ruleTile = Resources.Load<RuleTile>(ruleTilePath);
            if (ruleTile == null)
            {
                Debug.LogError($"Failed to load RuleTile from path: {ruleTilePath}");
                return null;
            }

            Debug.Log($"Working Rule Tile loaded from: {ruleTilePath}");
            return ruleTile;
        }

        /// <summary>
        /// 获取LandTile资源（缓存）
        /// </summary>
        private RuleTile GetLandTile()
        {
            if (_landTile == null)
            {
                string landTilePath = SceneDB.GetPrefabPath(14); // ID 14 对应 LandTile
                if (string.IsNullOrEmpty(landTilePath))
                {
                    Debug.LogError("Failed to get LandTile path for ID 14");
                    return null;
                }
                _landTile = Resources.Load<RuleTile>(landTilePath);
                if (_landTile == null)
                {
                    Debug.LogError($"Failed to load LandTile from path: {landTilePath}");
                }
                else
                {
                    Debug.Log($"LandTile loaded from: {landTilePath}");
                }
            }
            return _landTile;
        }

        /// <summary>
        /// 检查指定世界坐标位置是否存在LandTile
        /// </summary>
        private bool HasLandTileAtPosition(Vector3 worldPosition)
        {
            // 只查找现有的FarmingTilemap，不创建新的
            GameObject tilemapObject = GameObject.Find("FarmingTilemap");
            if (tilemapObject == null)
            {
                Debug.LogWarning("FarmingTilemap not found in scene");
                return false;
            }
            Tilemap farmingTilemap = tilemapObject.GetComponent<Tilemap>();
            if (farmingTilemap == null)
            {
                Debug.LogWarning("FarmingTilemap GameObject has no Tilemap component");
                return false;
            }
            Vector3Int cellPos = farmingTilemap.WorldToCell(worldPosition);
            TileBase tile = farmingTilemap.GetTile(cellPos);
            if (tile == null)
            {
                return false;
            }
            RuleTile landTile = GetLandTile();
            if (landTile == null)
            {
                Debug.LogWarning("LandTile resource not loaded");
                return false;
            }
            // 比较Tile是否为LandTile（通过引用比较）
            return tile == landTile;
        }

        /// <summary>
        /// 检查指定世界坐标位置是否已被GrowUp对象占用
        /// </summary>
        private bool IsGrowUpCellOccupied(Vector3 worldPosition)
        {
            Vector3Int? cellPos = GetCellPosition(worldPosition);
            if (!cellPos.HasValue)
            {
                return false;
            }
            return _occupiedGrowUpCells.Contains(cellPos.Value);
        }

        /// <summary>
        /// 标记指定世界坐标位置的单元格为已占用
        /// </summary>
        private void MarkGrowUpCellOccupied(Vector3 worldPosition)
        {
            Vector3Int? cellPos = GetCellPosition(worldPosition);
            if (cellPos.HasValue)
            {
                _occupiedGrowUpCells.Add(cellPos.Value);
                Debug.Log($"Marked cell {cellPos.Value} as occupied for grow-up object.");
            }
        }

        /// <summary>
        /// 清除指定世界坐标位置的单元格占用标记
        /// </summary>
        private void ClearGrowUpCellOccupied(Vector3 worldPosition)
        {
            Vector3Int? cellPos = GetCellPosition(worldPosition);
            if (cellPos.HasValue)
            {
                _occupiedGrowUpCells.Remove(cellPos.Value);
                Debug.Log($"Cleared cell {cellPos.Value} occupancy for grow-up object.");
            }
        }

        /// <summary>
        /// 将世界坐标转换为FarmingTilemap的单元格坐标
        /// </summary>
        private Vector3Int? GetCellPosition(Vector3 worldPosition)
        {
            GameObject tilemapObject = GameObject.Find("FarmingTilemap");
            if (tilemapObject == null)
            {
                Debug.LogWarning("FarmingTilemap not found in scene");
                return null;
            }
            Tilemap farmingTilemap = tilemapObject.GetComponent<Tilemap>();
            if (farmingTilemap == null)
            {
                Debug.LogWarning("FarmingTilemap GameObject has no Tilemap component");
                return null;
            }
            return farmingTilemap.WorldToCell(worldPosition);
        }

        // 新增：获取角色信息的方法
        public Character GetCharacter(string characterId)
        {
            if (_characters.TryGetValue(characterId, out Character character))
            {
                return character;
            }
            return null;
        }

        // 新增：获取所有角色ID列表
        public List<string> GetAllCharacterIds()
        {
            return new List<string>(_characters.Keys);
        }

        // 更新服务器时间并刷新所有生长对象
        public void UpdateServerTime(TimeInfo serverTime)
        {
            _currentServerTime = serverTime;
            //Debug.Log($"Server time updated: {serverTime.months}/{serverTime.day} {serverTime.hour}:{serverTime.minute}:{serverTime.second}");
            
            // 更新所有生长对象的显示
            UpdateAllGrowUpObjects();
        }

        // 将TimeInfo转换为总分钟数（简化计算：每年365天，每月30天，忽略second字段）
        private long TimeInfoToMinutes(TimeInfo time)
        {
            if (time == null) return 0;
            // 计算：(((year * 365 + months * 30 + day) * 24 + hour) * 60 + minute
            long totalDays = time.year * 365L + time.months * 30L + time.day;
            long totalHours = totalDays * 24L + time.hour;
            return totalHours * 60L + time.minute;
            // 忽略second字段
        }

        // 计算生长对象的当前显示索引
        private int CalculateGrowthIndex(TimeInfo growiTime, long ripenTime, int pathCount, TimeInfo currentTime)
        {
            if (growiTime == null || currentTime == null || pathCount <= 0)
                return 0;
            
            // ripenTime现在是分
            if (ripenTime <= 0)
                return pathCount - 1;
            
            long elapsedMinutes = TimeInfoToMinutes(currentTime) - TimeInfoToMinutes(growiTime);
            if (elapsedMinutes <= 0)
                return 0;
            
            if (elapsedMinutes >= ripenTime)
                return pathCount - 1;
            
            float progress = (float)elapsedMinutes / ripenTime;
            return Mathf.Min(pathCount - 1, (int)(progress * pathCount));
        }

        // 更新单个生长对象的prefab显示
        private void UpdateGrowUpObjectPrefab(string objectId)
        {
            if (!_growUpObjects.TryGetValue(objectId, out var data))
            {
                Debug.LogWarning($"UpdateGrowUpObjectPrefab: Object {objectId} not found in dictionary");
                return;
            }

            var (paths, prefabPathId, sceneObject, growiTime, ripenTime, lastDisplayedIndex) = data;
            
            // 安全检查：路径数组必须非空
            if (paths == null || paths.Length == 0)
            {
                Debug.LogWarning($"UpdateGrowUpObjectPrefab: Object {objectId} has empty prefab paths");
                return;
            }
            
            // 计算当前应该显示的索引
            int targetIndex = 0;
            if (_currentServerTime != null && growiTime != null)
            {
                targetIndex = CalculateGrowthIndex(growiTime, ripenTime, paths.Length, _currentServerTime);
            }
            else
            {
                // 没有时间信息，显示第一个prefab
                targetIndex = 0;
            }

            // 如果索引没有变化，不需要更新
            if (targetIndex == lastDisplayedIndex)
                return;

            // 存储当前位置
            Vector3 storedPosition = sceneObject.Position;
            
            // 销毁当前的 GameObject（如果存在）
            if (sceneObject.GameObject != null)
            {
                UnityEngine.Object.Destroy(sceneObject.GameObject);
            }
            
            // 获取类型和 tilemap
            var type = (TilemapItemType)SceneDB.GetPrefabType(prefabPathId);
            var tilemapPath = SceneDB.GetTilemapTarget(prefabPathId);
            
            // 使用目标索引的路径创建新的 SceneObject
            var newSceneObject = new SceneObject(objectId, paths[targetIndex], type, tilemapPath);
            newSceneObject.Initialize(storedPosition, false);
            
            // 更新字典中的SceneObject和lastDisplayedIndex
            _growUpObjects[objectId] = (paths, prefabPathId, newSceneObject, growiTime, ripenTime, targetIndex);
            
            Debug.Log($"Grow-up object {objectId} updated to prefab index {targetIndex} ({paths[targetIndex]})");
        }

        // 更新所有生长对象的显示
        private void UpdateAllGrowUpObjects()
        {
            if (_growUpObjects.Count == 0)
                return;

            //Debug.Log($"Updating {_growUpObjects.Count} grow-up objects based on server time");
            var objectIds = new List<string>(_growUpObjects.Keys);
            foreach (var objectId in objectIds)
            {
                UpdateGrowUpObjectPrefab(objectId);
            }
        }

        #region Harvesting Helper Methods
        
        private Vector3Int DirectionToVector3Int(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Vector3Int(0, 1, 0);
                case Direction.Down:
                    return new Vector3Int(0, -1, 0);
                case Direction.Left:
                    return new Vector3Int(-1, 0, 0);
                case Direction.Right:
                    return new Vector3Int(1, 0, 0);
                default:
                    return Vector3Int.zero;
            }
        }

        private Vector3Int? GetForwardCellPosition(Vector3 worldPosition, Direction direction)
        {
            Vector3Int? baseCell = GetCellPosition(worldPosition);
            if (!baseCell.HasValue)
                return null;

            Vector3Int directionOffset = DirectionToVector3Int(direction);
            return baseCell.Value + directionOffset;
        }

        private string FindVegetableAtCell(Vector3Int cellPosition)
        {
            foreach (var kvp in _growUpObjects)
            {
                var (paths, prefabPathId, sceneObject, growiTime, ripenTime, lastDisplayedIndex) = kvp.Value;
                Vector3Int? objectCell = GetCellPosition(sceneObject.Position);
                if (objectCell.HasValue && Mathf.Abs(objectCell.Value.x - cellPosition.x) <= 1 && Mathf.Abs(objectCell.Value.y - cellPosition.y) <= 1 )
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        private bool IsVegetableRipe(TimeInfo growiTime, long ripenTime)
        {
            if (growiTime == null || ripenTime <= 0)
                return true;

            if (_currentServerTime == null)
            {
                Debug.LogWarning("No server time available, using local time for maturity check");
                long elapsedMinutes = TimeInfoToMinutes(new TimeInfo
                {
                    year = System.DateTime.Now.Year,
                    months = System.DateTime.Now.Month,
                    day = System.DateTime.Now.Day,
                    hour = System.DateTime.Now.Hour,
                    minute = System.DateTime.Now.Minute,
                    second = 0  // 忽略second字段
                }) - TimeInfoToMinutes(growiTime);
                return elapsedMinutes >= ripenTime;
            }

            long elapsedMinutesServer = TimeInfoToMinutes(_currentServerTime) - TimeInfoToMinutes(growiTime);
            return elapsedMinutesServer >= ripenTime;
        }

        private Vegetable CreateVegetableFromData(string vegetableGuid, int vegetableId, Vector3Int cellPosition)
        {
            return new Vegetable
            {
                VegetableGuid = vegetableGuid,
                perfabId = vegetableId,
                topLeft = new Pos { X = cellPosition.x, Y = cellPosition.y },
                bottomRight = new Pos { X = cellPosition.x, Y = cellPosition.y },
                VegetableResource = "",
                count = 1,
                growiTime = new TimeInfo(),
                ripenTime = 0
            };
        }

        private async void TryHarvestVegetable(Vector2 position, Direction direction)
        {
            // Get FarmingTilemap to convert world position to cell
            Tilemap farmingTilemap = GetOrCreateTilemap("FarmingTilemap");
            if (farmingTilemap == null)
            {
                Debug.LogWarning("TryHarvestVegetable: FarmingTilemap not found");
                return;
            }
            
            Vector3Int cellPosition = farmingTilemap.WorldToCell(position);
            string vegetableGuid = FindVegetableAtCell(cellPosition);
            if (string.IsNullOrEmpty(vegetableGuid))
            {
                // No vegetable at this cell
                return;
            }
            
            // Get vegetable data
            if (!_growUpObjects.TryGetValue(vegetableGuid, out var data))
            {
                Debug.LogWarning($"TryHarvestVegetable: Vegetable {vegetableGuid} not found in dictionary");
                return;
            }
            
            var (paths, prefabPathId, sceneObject, growiTime, ripenTime, lastDisplayedIndex) = data;
            
            // Check if vegetable is ripe
            if (!IsVegetableRipe(growiTime, ripenTime))
            {
                Debug.Log($"TryHarvestVegetable: Vegetable {vegetableGuid} is not ripe yet");
                return;
            }
            
            // Create vegetable object for network call using stored data
            Vegetable vegetable = new Vegetable
            {
                VegetableGuid = vegetableGuid,
                perfabId = prefabPathId,
                topLeft = new Pos { X = cellPosition.x, Y = cellPosition.y },
                bottomRight = new Pos { X = cellPosition.x, Y = cellPosition.y },
                VegetableResource = "",
                count = 1,
                growiTime = growiTime,
                ripenTime = ripenTime,
                subtype = (int)SubItemType.Mature
            };
            
            // Call server to harvest
            bool success = false;
            try
            {
                success = await netScene.harvest(vegetable);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"TryHarvestVegetable: Exception calling cut_vegetable: {ex.Message}");
            }
            
            if (!success)
            {
                Debug.LogError($"TryHarvestVegetable: Server rejected harvesting vegetable {vegetableGuid}");
                // No UI prompt as per requirements
            }
            else
            {
                Debug.Log($"TryHarvestVegetable: Harvest request sent for vegetable {vegetableGuid}");
                // Server will respond with on_remove_scene_info to actually remove the object
            }
        }

        private void RemoveGrowUpObject(string objectId)
        {
            if (!_growUpObjects.TryGetValue(objectId, out var data))
            {
                Debug.LogWarning($"RemoveGrowUpObject: Object {objectId} not found");
                return;
            }

            var (paths, prefabPathId, sceneObject, growiTime, ripenTime, lastDisplayedIndex) = data;
            
            // Destroy GameObject
            if (sceneObject.GameObject != null)
            {
                UnityEngine.Object.Destroy(sceneObject.GameObject);
            }
            
            // Clear cell occupancy
            ClearGrowUpCellOccupied(sceneObject.Position);
            
            // Remove from dictionary
            _growUpObjects.Remove(objectId);
            
            Debug.Log($"Grow-up object {objectId} removed locally");
        }

        public void RemoveGrowUpObjectByGuid(string vegetableGuid)
        {
            if (string.IsNullOrEmpty(vegetableGuid))
            {
                Debug.LogWarning("RemoveGrowUpObjectByGuid: vegetableGuid is null or empty");
                return;
            }
            
            // Check if this is a grow-up object ID
            if (_growUpObjects.ContainsKey(vegetableGuid))
            {
                RemoveGrowUpObject(vegetableGuid);
                return;
            }
            
            // If not found by direct key, search through all objects to find matching VegetableGuid
            // (In our current implementation, objectId equals VegetableGuid from server)
            // So this fallback may not be needed, but we keep it for robustness.
            Debug.LogWarning($"RemoveGrowUpObjectByGuid: No grow-up object found with GUID {vegetableGuid}");
        }
        #endregion

        // 测试方法：检查 GameSceneManager 是否正常工作
        public void TestGameSceneManager()
        {
            Debug.Log($"GameSceneManager.TestGameSceneManager() called - Instance: {_instance != null}, SceneDB: {SceneDB != null}");
            Debug.Log($"Characters count: {_characters.Count}, Objects count: {_objects.Count}");
            Debug.Log($"Event handlers registered: {_eventHandlers.Count}");
        }

        /// <summary>
        /// 清理场景数据，在场景切换时调用
        /// </summary>
        private void CleanupSceneData()
        {
            Debug.Log($"GameSceneManager.CleanupSceneData() called - Cleaning up scene data");

            // 清理_growUpObjects字典中的GameObject
            foreach (var kvp in _growUpObjects)
            {
                var (paths, prefabPathId, sceneObject, growiTime, ripenTime, lastDisplayedIndex) = kvp.Value;
                if (sceneObject != null && sceneObject.GameObject != null)
                {
                    UnityEngine.Object.Destroy(sceneObject.GameObject);
                }
            }
            _growUpObjects.Clear();

            // 清理_objects字典中的GameObject
            foreach (var kvp in _objects)
            {
                var sceneObject = kvp.Value;
                if (sceneObject != null && sceneObject.GameObject != null)
                {
                    UnityEngine.Object.Destroy(sceneObject.GameObject);
                }
            }
            _objects.Clear();

            // 清理其他数据结构
            _occupiedGrowUpCells.Clear();
            _cultivatedDict.Clear();
            _characters.Clear();

            Debug.Log($"GameSceneManager.CleanupSceneData() completed - All scene data cleared");
        }

        /// <summary>
        /// 场景加载完成事件处理
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"GameSceneManager.OnSceneLoaded() - Scene: {scene.name}, Mode: {mode}");
            CleanupSceneData();
            
            // 调用into_subscene通知服务器当前场景
            try
            {
                netScene.into_subscene(scene.name);
                Debug.Log($"GameSceneManager.OnSceneLoaded() - Called into_subscene with scene name: {scene.name}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"GameSceneManager.OnSceneLoaded() - Failed to call into_subscene: {ex.Message}");
            }

            // 在Field场景中生成NPC
            if (scene.name == "Field")
            {
                SpawnNPCs();
            }
        }

        private void SpawnNPCs()
        {
            var manager = NPCDialogueManager.Instance;
            var allNpcs = manager.GetAllNPCDefinitions();
            Debug.Log($"[SceneManager.SpawnNPCs] Spawning {allNpcs.Count} NPCs in Field scene");
            foreach (var npcDef in allNpcs)
            {
                if (_characters.ContainsKey($"npc_{npcDef.npcId}"))
                    continue;

                var prefabPath = string.IsNullOrEmpty(npcDef.prefabPath)
                    ? manager.GetGlobalSettings()?.defaultPrefabPath ?? "Perfabs/Characters/OtherCharacter"
                    : npcDef.prefabPath;

                var npcCharacter = new Character($"npc_{npcDef.npcId}", prefabPath);
                npcCharacter.Initialize(npcDef.spawnPosition);

                if (npcCharacter.GameObject != null)
                {
                    int npcLayer = LayerMask.NameToLayer("NPC");
                    if (npcLayer >= 0)
                    {
                        npcCharacter.GameObject.layer = npcLayer;
                        SetLayerRecursively(npcCharacter.GameObject, npcLayer);
                    }

                    var npcController = npcCharacter.GameObject.GetComponent<NPCDialogueController>();
                    if (npcController == null)
                    {
                        npcController = npcCharacter.GameObject.AddComponent<NPCDialogueController>();
                    }
                    npcController.npcId = npcDef.npcId;

                    var remoteCtrl = npcCharacter.GameObject.GetComponent<RemoteCharacterController>();
                    if (remoteCtrl != null)
                        remoteCtrl.enabled = false;

                    if (npcDef.patrol != null && npcDef.patrol.triggerAreaIds.Count > 0)
                    {
                        Debug.Log($"[SceneManager.SpawnNPCs] {npcDef.npcName}({npcDef.npcId}): patrol found, triggerAreaIds=[{string.Join(",", npcDef.patrol.triggerAreaIds)}]");
                        var patrol = npcCharacter.GameObject.GetComponent<NPCPatrolController>();
                        if (patrol == null)
                            patrol = npcCharacter.GameObject.AddComponent<NPCPatrolController>();
                        patrol.Init(npcDef.patrol);
                    }
                    else
                    {
                        Debug.Log($"[SceneManager.SpawnNPCs] {npcDef.npcName}({npcDef.npcId}): no patrol config (patrol={(npcDef.patrol != null ? "exists" : "null")}, triggerAreaIds.Count={(npcDef.patrol?.triggerAreaIds?.Count ?? -1)})");
                    }

                    _characters[$"npc_{npcDef.npcId}"] = npcCharacter;
                    Debug.Log($"Spawned NPC {npcDef.npcName} ({npcDef.npcId}) at {npcDef.spawnPosition}");
                }
            }
        }

        private static void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private void OnDestroy()
        {
            // 取消订阅场景加载事件
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Debug.Log("GameSceneManager.OnDestroy() - Unsubscribed from sceneLoaded event");
        }

        // 原有其他方法...
    }
}
