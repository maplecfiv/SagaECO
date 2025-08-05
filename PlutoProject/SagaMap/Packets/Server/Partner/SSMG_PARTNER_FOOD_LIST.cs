using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_FOOD_LIST : Packet
    {
        private readonly byte foodlistlen = 20;

        public SSMG_PARTNER_FOOD_LIST()
        {
            data = new byte[3 + 4 * foodlistlen];
            offset = 2;
            ID = 0x2198;
            PutByte(foodlistlen, 2);
        }

        public List<uint> FoodItemList
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutUInt(value[i], 3 + i * 4);
            }
        }
    }
}