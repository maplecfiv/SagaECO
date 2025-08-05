using System.Collections.Generic;

namespace SagaDB.Synthese
{
    /// <summary>
    ///     合成信息
    /// </summary>
    public class SyntheseInfo
    {
        /// <summary>
        ///     合成ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     合成使用的技能
        /// </summary>
        public ushort SkillID { get; set; }

        /// <summary>
        ///     合成使用技能的等级
        /// </summary>
        public byte SkillLv { get; set; }

        /// <summary>
        ///     合成所需资金
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        ///     合成所需工具
        /// </summary>
        public uint RequiredTool { get; set; }

        /// <summary>
        ///     合成所需材料
        /// </summary>
        public List<ItemElement> Materials { get; } = new List<ItemElement>();

        /// <summary>
        ///     合成产物
        /// </summary>
        public List<ItemElement> Products { get; } = new List<ItemElement>();
    }

    /// <summary>
    ///     合成物品信息
    /// </summary>
    public class ItemElement
    {
        public int Exp;

        /// <summary>
        ///     物品ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     物品个数
        /// </summary>
        public ushort Count { get; set; }

        /// <summary>
        ///     几率
        /// </summary>
        public int Rate { get; set; }
    }
}