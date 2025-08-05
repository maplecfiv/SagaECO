using System.Collections.Generic;

namespace SagaDB.Fish
{
    public class Fish
    {
        private string name;

        /// <summary>
        ///     物品ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     取得几率
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        ///     取得数量
        /// </summary>
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Format("ItemID:{0}, Rate:{1},Count:{2}", ID, Rate, Count);
        }
    }

    /// <summary>
    ///     钓鱼列表
    /// </summary>
    public class FishList
    {
        /// <summary>
        ///     钓鱼列表
        /// </summary>
        public List<Fish> Items { get; } = new List<Fish>();

        /// <summary>
        ///     钓鱼列表几率总和
        /// </summary>
        public int TotalRate { get; set; }
    }
}