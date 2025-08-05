using System.Collections.Generic;

namespace SagaDB.Npc
{
    public enum ShopType
    {
        None,
        CP,
        ECoin,
        unknown
    }

    public class Shop
    {
        /// <summary>
        ///     Shop的ID
        /// </summary>
        public uint ID { get; set; }

        public List<uint> RelatedNPC { get; } = new List<uint>();

        /// <summary>
        ///     贩卖倍率
        /// </summary>
        public uint SellRate { get; set; }

        /// <summary>
        ///     购买倍率
        /// </summary>
        public uint BuyRate { get; set; }

        /// <summary>
        ///     购买额度
        /// </summary>
        public uint BuyLimit { get; set; }

        /// <summary>
        ///     商品
        /// </summary>
        public List<uint> Goods { get; } = new List<uint>();

        /// <summary>
        ///     商店类型
        /// </summary>
        public ShopType ShopType { get; set; }
    }
}