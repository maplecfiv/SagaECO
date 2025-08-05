using System;

namespace SagaDB.KnightWar
{
    /// <summary>
    ///     骑士团演习
    /// </summary>
    public class KnightWar
    {
        public uint ID { get; set; }

        /// <summary>
        ///     演习地图ID
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     最高等级限制
        /// </summary>
        public byte MaxLV { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     持续时间（分钟）
        /// </summary>
        public int Duration { get; set; }
    }
}