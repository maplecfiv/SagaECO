using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Partner
{
    public enum AIFlag
    {
        Normal,
        Active = 0x1,
        NoAttack = 0x2,
        NoMove = 0x4,
        RunAway = 0x8,
        HelpSameType = 0x10,
        HateHeal = 0x20,
        HateMagic = 0x40,
        Symbol = 0x80,
        SymbolTrash = 0x100
    }

    /// <summary>
    ///     AI的模式
    /// </summary>
    public class AIMode
    {
        //新增结束


        public BitMask mask;

        public AIMode(int value)
        {
            mask = new BitMask(value);
        }

        public AIMode()
        {
            mask = new BitMask(0);
        }

        public bool isAnAI { set; get; }
        public Dictionary<uint, SkillList> AnAI_SkillAssemblage { get; } = new Dictionary<uint, SkillList>();


        public bool isNewAI { set; get; }

        public int Distance { set; get; } = 20;

        public int ShortCD { set; get; }

        public int LongCD { set; get; }

        public Dictionary<uint, SkilInfo> SkillOfShort { get; } = new Dictionary<uint, SkilInfo>();

        public Dictionary<uint, SkilInfo> SkillOfLong { get; } = new Dictionary<uint, SkilInfo>();

        /// <summary>
        ///     怪物ID
        /// </summary>
        public uint PartnerID { get; set; }

        /// <summary>
        ///     怪物的AI模式
        /// </summary>
        public int AI
        {
            get => mask.Value;
            set => mask.Value = value;
        }

        /// <summary>
        ///     是否主动
        /// </summary>
        public bool Active => mask.Test(AIFlag.Active);

        /// <summary>
        ///     是否不会攻击
        /// </summary>
        public bool NoAttack => mask.Test(AIFlag.NoAttack);

        /// <summary>
        ///     是否无法移动
        /// </summary>
        public bool NoMove => mask.Test(AIFlag.NoMove);

        /// <summary>
        ///     是否看见玩家会逃跑
        /// </summary>
        public bool RunAway => mask.Test(AIFlag.RunAway);

        /// <summary>
        ///     是否帮助同类型怪物
        /// </summary>
        public bool HelpSameType => mask.Test(AIFlag.HelpSameType);

        /// <summary>
        ///     是否仇恨治愈魔法
        /// </summary>
        public bool HateHeal => mask.Test(AIFlag.HateHeal);

        /// <summary>
        ///     是否仇恨吟唱魔法
        /// </summary>
        public bool HateMagic => mask.Test(AIFlag.HateMagic);

        /// <summary>
        ///     是否是象征
        /// </summary>
        public bool Symbol => mask.Test(AIFlag.Symbol);

        /// <summary>
        ///     是否是象征残骸
        /// </summary>
        public bool SymbolTrash => mask.Test(AIFlag.SymbolTrash);


        /// <summary>
        ///     怪物在攻击时，需要使用技能时的技能列表，Key＝技能，Value＝几率
        /// </summary>
        public Dictionary<uint, int> EventAttacking { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        ///     怪物在攻击时，使用技能的几率
        /// </summary>
        public int EventAttackingSkillRate { get; set; }

        /// <summary>
        ///     怪物的主人在战斗中时，需要使用技能时的技能列表，Key＝技能，Value＝几率
        /// </summary>
        public Dictionary<uint, int> EventMasterCombat { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        ///     怪物的主人在战斗中时，使用技能的几率
        /// </summary>
        public int EventMasterCombatSkillRate { get; set; }

        //新增AI部分 by:An
        public class SkillsInfo
        {
            public uint SkillID { set; get; }
            public int Delay { set; get; }
        }

        public class SkillList
        {
            public int MaxHP { set; get; }
            public int MinHP { set; get; }
            public int Rate { set; get; }
            public Dictionary<uint, SkillsInfo> AnAI_SkillList { get; } = new Dictionary<uint, SkillsInfo>();
        }

        //新增结束
        //新增AI部分 by:TT
        public class SkilInfo
        {
            public int Rate { set; get; }
            public int CD { set; get; }
            public int MaxHP { set; get; }
            public int MinHP { set; get; }
        }
    }
}