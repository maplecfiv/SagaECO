using System.Collections.Generic;

namespace SagaDB.Iris
{
    /// <summary>
    ///     Iris卡片能力向量
    /// </summary>
    public class AbilityVector
    {
        /// <summary>
        ///     该能力向量拥有的具体RA能力，Key为向量等级
        /// </summary>
        public Dictionary<byte, Dictionary<ReleaseAbility, int>> ReleaseAbilities { get; } =
            new Dictionary<byte, Dictionary<ReleaseAbility, int>>();

        /// <summary>
        ///     Iris Ability ID (0-1000 原版能力 1000-2000 组队条件触发长时间面板显示能力 2000-3000 actorpc自身条件影响攻防结果计算时判定能力 3000-4000
        ///     对象条件影响攻防结果计算时判定能力 4000+待定)
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}