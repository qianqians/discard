# AIStarve

## Prefab与TypeID对应关系

prefab的typeid保存在 `AIStarve/Assets/Resources/PrefabMapping.json` 文件中。

### 当前配置：

| TypeID | Prefab路径 | 类型 | Tilemap |
|--------|------------|------|---------|
| 1 | Perfabs/LeftBottomPanel | 1 | - |
| 2 | Perfabs/SceneObjectSelect | 1 | - |
| 3 | Perfabs/Characters/OtherCharacter | 1 | - |
| 4 | Perfabs/Characters/SelfCharacter | 1 | - |
| 5 | Perfabs/Scene/Bush | 1 | - |
| 6 | Perfabs/Scene/House | 1 | - |
| 7 | Perfabs/Scene/Tree | 1 | - |
| 8 | Tiles/Tilemap Obstacles_18 | 2 | Decoration Tilemap |
| 9 | Tiles/Tileset Obstacles_3 | 2 | Decoration Tilemap |
| 10 | Tiles/Tileset Obstacles_4 | 2 | Decoration Tilemap |
| 11 | Tiles/Tileset Obstacles_5 | 2 | Decoration Tilemap |
| 12 | Tiles/Tilemap Obstacles_35 | 2 | Decoration Tilemap |
| 13 | Tiles/Tilemap Obstacles_27 | 2 | Decoration Tilemap |
| 14 | Assets/Tile Rules/LandTile | 2 | Decoration Tilemap |

### 使用说明：

- **TypeID 1**: UI预制体
- **TypeID 2**: 场景对象预制体
- 通过 `SceneDataBase` 类可以查询prefab路径、类型和tilemap信息
- 添加新的prefab时，需要在 `PrefabMapping.json` 文件中添加对应的映射关系