using System;
using SagaDB.Actor;
using SagaDB.Npc;

namespace SagaDB.Quests
{
    public class QuestInfo
    {
        /// <summary>
        ///     任务ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     任务组ID
        /// </summary>
        public uint GroupID { get; set; }

        /// <summary>
        ///     任务类型
        /// </summary>
        public QuestType QuestType { get; set; }

        /// <summary>
        ///     任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     任务时间限制，分钟为单位
        /// </summary>
        public int TimeLimit { get; set; }

        /// <summary>
        ///     任务完成奖励道具
        /// </summary>
        public uint RewardItem { get; set; }

        /// <summary>
        ///     任务完成奖励道具的数量
        /// </summary>
        public byte RewardCount { get; set; }

        /// <summary>
        ///     任务需要的任务点
        /// </summary>
        public byte RequiredQuestPoint { get; set; }

        /// <summary>
        ///     任务创建的遗迹ID
        /// </summary>
        public uint DungeonID { get; set; }

        /// <summary>
        ///     任务要求最低人物等级
        /// </summary>
        public byte MinLevel { get; set; }

        /// <summary>
        ///     任务要求最低人物等级
        /// </summary>
        public byte MaxLevel { get; set; }

        /// <summary>
        ///     任务奖励经验值
        /// </summary>
        public uint EXP { get; set; }

        /// <summary>
        ///     任务奖励职业经验值
        /// </summary>
        public uint JEXP { get; set; }

        /// <summary>
        ///     任务奖励金钱
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        ///     任务奖励CP
        /// </summary>
        public uint CP { get; set; }


        /// <summary>
        ///     任务奖励声望
        /// </summary>
        public uint Fame { get; set; }

        /// <summary>
        ///     有效地图1
        /// </summary>
        public uint MapID1 { get; set; }

        /// <summary>
        ///     有效地图2
        /// </summary>
        public uint MapID2 { get; set; }

        /// <summary>
        ///     有效地图3
        /// </summary>
        public uint MapID3 { get; set; }

        /// <summary>
        ///     任务对象ID，可以是道具ID，也可以是怪物ID
        /// </summary>
        public uint ObjectID1 { get; set; }

        /// <summary>
        ///     任务对象ID2，可以是道具ID，也可以是怪物ID
        /// </summary>
        public uint ObjectID2 { get; set; }

        /// <summary>
        ///     任务对象ID3，可以是道具ID，也可以是怪物ID
        /// </summary>
        public uint ObjectID3 { get; set; }

        /// <summary>
        ///     任务要求对象个数1
        /// </summary>
        public int Count1 { get; set; }

        /// <summary>
        ///     任务要求对象个数2
        /// </summary>
        public int Count2 { get; set; }

        /// <summary>
        ///     任务要求对象个数2
        /// </summary>
        public int Count3 { get; set; }

        /// <summary>
        ///     是否可以组队进行
        /// </summary>
        public bool Party { get; set; }

        /// <summary>
        ///     搬运任务委托NPC
        /// </summary>
        public uint NPCSource { get; set; }

        /// <summary>
        ///     搬运任务目标NPC
        /// </summary>
        public uint NPCDestination { get; set; }

        /// <summary>
        ///     任务完成计数器
        /// </summary>
        public string QuestCounterName { get; set; }

        /// <summary>
        ///     任务要求的职业类别
        /// </summary>
        public JobType JobType { get; set; } = JobType.NOVICE;

        /// <summary>
        ///     任务要求的职业职业
        /// </summary>
        public PC_JOB Job { get; set; } = PC_JOB.NONE;

        /// <summary>
        ///     任务要求的种族
        /// </summary>
        public PC_RACE Race { get; set; } = PC_RACE.NONE;

        /// <summary>
        ///     任务要求的性别
        /// </summary>
        public PC_GENDER Gender { get; set; } = PC_GENDER.NONE;

        public override string ToString()
        {
            return Name;
        }
    }

    public class Quest
    {
        public Quest(uint id)
        {
            Detail = QuestFactory.Instance.Items[id];
        }

        public uint ID
        {
            get => Detail.ID;
            set => Detail.ID = value;
        }

        public QuestType QuestType
        {
            get => Detail.QuestType;
            set => Detail.QuestType = value;
        }

        public string Name
        {
            get => Detail.Name;
            set => Detail.Name = value;
        }

        public QuestInfo Detail { get; }

        /// <summary>
        ///     任务状态
        /// </summary>
        public QuestStatus Status { get; set; }

        /// <summary>
        ///     任务结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        ///     目前完成数量1
        /// </summary>
        public int CurrentCount1 { get; set; }

        /// <summary>
        ///     目前完成数量2
        /// </summary>
        public int CurrentCount2 { get; set; }

        /// <summary>
        ///     目前完成数量3
        /// </summary>
        public int CurrentCount3 { get; set; }

        /// <summary>
        ///     委托任务的NPC
        /// </summary>
        public NPC NPC { get; set; }

        /// <summary>
        ///     该任务对指定玩家来说的难度
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>难度</returns>
        public QuestDifficulty Difficulty(ActorPC pc)
        {
            var diff = Detail.MinLevel - pc.Level;
            if (Math.Abs(diff) <= 3)
                return QuestDifficulty.BEST_FIT;
            if (Math.Abs(diff) > 3 && Math.Abs(diff) <= 9)
                return QuestDifficulty.NORMAL;
            if (diff > 9)
                return QuestDifficulty.TOO_HARD;
            if (diff < -9)
                return QuestDifficulty.TOO_EASY;
            return QuestDifficulty.NORMAL;
        }
    }
}