using UnityEngine;
using Abelkhan;

namespace SceneManagement
{
    public class SceneEventArgs
    {
        public SceneEvent EventType { get; set; }
        public string ObjectId { get; set; }
        public string PrefabPath { get; set; }
        public Vector2 Position { get; set; }
        public Direction DirectionArg { get; set; }
        public GameObject GameObject { get; set; }
        public CharacterType CharacterType { get; set; }
        public int PerfabPathId { get; set; }
        public string PerfabPath { get; set; }
        public int KeyIndex { get; set; } = -1;
        public TimeInfo GrowiTime { get; set; }
        /// <summary>
        /// 成熟时间（单位：分钟）
        /// </summary>
        public long RipenTime { get; set; }
        public Cultivated CultivatedData { get; set; }
    }
    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    public enum CharacterType
    {
        Self = 0,
        Friend = 1,
        Enemy = 2,
        Npc = 3,
        unknow = 4
    }
}