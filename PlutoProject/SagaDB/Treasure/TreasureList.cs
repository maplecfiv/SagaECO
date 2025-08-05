using System.Collections.Generic;

namespace SagaDB.Treasure
{
    /// <summary>
    ///     宝物物品
    /// </summary>
    public class TreasureItem
    {
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
    ///     宝物列表
    /// </summary>
    public class TreasureList
    {
        /// <summary>
        ///     列表组名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     宝物列表
        /// </summary>
        public List<TreasureItem> Items { get; } = new List<TreasureItem>();

        /// <summary>
        ///     宝物几率总和
        /// </summary>
        public int TotalRate { get; set; }

        public override string ToString()
        {
            return string.Format("{0},Items:{1},TotalRate:{2}", Name, Items.Count, TotalRate);
        }
    }
}