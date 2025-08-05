using System.Collections.Generic;

namespace SagaDB.ECOShop
{
    public class ShopItem
    {
        public string comment;
        public uint points;
        public int rental;
    }

    public class ShopCategory
    {
        public uint ID { get; set; }

        public string Name { get; set; }

        public Dictionary<uint, ShopItem> Items { get; } = new Dictionary<uint, ShopItem>();
    }
}