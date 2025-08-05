using System.Collections.Generic;

namespace SagaDB.DEMIC
{
    public class ShopChip
    {
        public uint ItemID { get; set; }

        public ulong EXP { get; set; }

        public ulong JEXP { get; set; }

        public string Description { get; set; }
    }

    public class ChipShopCategory
    {
        public uint ID { get; set; }

        public string Name { get; set; }

        public byte PossibleLv { get; set; }

        public Dictionary<uint, ShopChip> Items { get; } = new Dictionary<uint, ShopChip>();
    }
}