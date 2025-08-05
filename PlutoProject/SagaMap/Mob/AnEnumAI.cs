using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Mob
{
    /// <summary>
    ///     AI的模式
    /// </summary>
    public class AnAIMode
    {
        private Dictionary<uint, int> eventAttackingSkillCDOnLongRange;

        public BitMask mask;

        public AnAIMode(int value)
        {
            mask = new BitMask(value);
        }

        public AnAIMode()
        {
            mask = new BitMask(0);
        }

        /// <summary>
        ///     怪物ID
        /// </summary>
        public uint MobID { get; set; }

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
        ///     怪物在攻击时(近程)，需要使用技能时的技能列表，Key＝技能，Value＝几率
        /// </summary>
        public Dictionary<uint, int> EventAttackingOnShortRange { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        ///     怪物使用技能的CD(近程)，Key＝技能，Value＝CD
        /// </summary>
        public Dictionary<uint, int> EventAttackingSkillCDOnShortRange { get; set; }

        /// <summary>
        ///     怪物在攻击时(远程)，需要使用技能时的技能列表，Key＝技能，Value＝几率
        /// </summary>
        public Dictionary<uint, int> EventAttackingOnLongRange { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        ///     怪物使用技能的CD(远程)，Key＝技能，Value＝CD
        /// </summary>
        public Dictionary<uint, int> EventAttackingSkillCDOnLongRange
        {
            get => EventAttackingOnLongRange;
            set => EventAttackingOnLongRange = value;
        }

        /// <summary>
        ///     仇恨在多少范围内为近战（格）
        /// </summary>
        public int MaximumRange { get; set; }

        /// <summary>
        ///     仇恨在多少范围内为远战（格）
        /// </summary>
        public int MinimumRange { get; set; }

        /// <summary>
        ///     怪物在攻击时(近程)，使用技能的几率
        /// </summary>
        public int EventAttackingSkillRateOnShortRange { get; set; }

        /// <summary>
        ///     怪物在攻击时(远程)，使用技能的几率
        /// </summary>
        public int EventAttackingSkillRateOnLongRange { get; set; }
    }
}