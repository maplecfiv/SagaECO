using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaDB.LevelLimit
{
    public class LevelLimit : Singleton<LevelLimit>
    {
        /// <summary>
        ///     是否正在等待标记
        /// </summary>
        public byte IsLock { get; set; }

        /// <summary>
        ///     初次设定的等级上限
        /// </summary>
        public uint FirstLevelLimit { get; set; }

        /// <summary>
        ///     当前的等级上限
        /// </summary>
        public uint NowLevelLimit { get; set; }

        /// <summary>
        ///     下次的等级上限
        /// </summary>
        public uint NextLevelLimit { get; set; }

        /// <summary>
        ///     上次的等级上限
        /// </summary>
        public uint LastTimeLevelLimit { get; set; }

        /// <summary>
        ///     设置下次的等级上限增幅度
        /// </summary>
        public uint SetNextUpLevelLimit { get; set; }

        /// <summary>
        ///     设置下次达成上限后等待的天数
        /// </summary>
        public uint SetNextUpDays { get; set; }

        /// <summary>
        ///     当前完成的时间
        /// </summary>
        public DateTime ReachTime { get; set; }

        /// <summary>
        ///     下次开始新上限的时间
        /// </summary>
        public DateTime NextTime { get; set; }

        /// <summary>
        ///     第一个达成的玩家CharID
        /// </summary>
        public uint FirstPlayer { get; set; }

        /// <summary>
        ///     第二个达成的玩家CharID
        /// </summary>
        public uint SecondPlayer { get; set; }

        /// <summary>
        ///     第三个达成的玩家CharID
        /// </summary>
        public uint Thirdlayer { get; set; }

        /// <summary>
        ///     第四个达成的玩家CharID
        /// </summary>
        public uint FourthPlayer { get; set; }

        /// <summary>
        ///     第五个达成的玩家CharID
        /// </summary>
        public uint FifthPlayer { get; set; }

        /// <summary>
        ///     达成上限的玩家列表
        /// </summary>
        public List<ActorPC> FinishPlayers { get; set; } = new List<ActorPC>();
    }
}