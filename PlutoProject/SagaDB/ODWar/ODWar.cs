using System.Collections.Generic;

namespace SagaDB.ODWar
{
    /// <summary>
    ///     都市攻防战
    /// </summary>
    public class ODWar
    {
        /// <summary>
        ///     攻防战地图ID
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     攻防战的时间
        /// </summary>
        public Dictionary<int, int> StartTime { get; } = new Dictionary<int, int>();

        /// <summary>
        ///     象征MobID和位置
        /// </summary>
        public Dictionary<int, Symbol> Symbols { get; } = new Dictionary<int, Symbol>();

        /// <summary>
        ///     象征残骸MobID
        /// </summary>
        public uint SymbolTrash { get; set; }

        /// <summary>
        ///     DEM Champ怪列表
        /// </summary>
        public List<uint> DEMChamp { get; } = new List<uint>();

        /// <summary>
        ///     DEM 普通怪列表
        /// </summary>
        public List<uint> DEMNormal { get; } = new List<uint>();

        /// <summary>
        ///     Boss怪物列表
        /// </summary>
        public List<uint> Boss { get; } = new List<uint>();

        /// <summary>
        ///     敌人比较强的攻击波
        /// </summary>
        public Wave WaveStrong { get; set; }

        /// <summary>
        ///     敌人比较弱的攻击波
        /// </summary>
        public Wave WaveWeak { get; set; }

        /// <summary>
        ///     攻城战是否开始
        /// </summary>
        public bool Started { get; set; }

        /// <summary>
        ///     玩家的成绩
        /// </summary>
        public Dictionary<uint, int> Score { get; } = new Dictionary<uint, int>();

        public class Symbol
        {
            public uint actorID;
            public bool broken = false;
            public int id;
            public uint mobID;
            public byte x;
            public byte y;
        }

        public class Wave
        {
            public int DEMChamp;
            public int DEMNormal;
        }
    }
}