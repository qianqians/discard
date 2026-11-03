using UnityEngine;

/// <summary>
/// Tilemap项类型枚举
/// </summary>
public enum TilemapItemType
{
    /// <summary>
    /// 对象类型 - 放置预制体
    /// </summary>
    ObjectsTilemapItem = 1,
    
    /// <summary>
    /// 地面类型 - 绘制Tile
    /// </summary>
    GroundTilemapItem = 2,

    /// <summary>
    /// 下面UI的快捷栏
    /// </summary>
    ButtonKeyBoard = 3,
    /// <summary>
    /// 会长大的类型
    /// </summary>
    /// 
    ObjectsTilemapGrowUpItem = 4
}
public enum SubItemType
{
    Seed = 1,
    Mature = 2
}