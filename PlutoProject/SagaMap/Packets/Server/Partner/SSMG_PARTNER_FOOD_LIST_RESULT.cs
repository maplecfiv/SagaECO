using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_FOOD_LIST_RESULT : Packet
    {
        public SSMG_PARTNER_FOOD_LIST_RESULT()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x219A;
        }

        /// <summary>
        ///     Careful!ClientShownList not equals the real List!
        /// </summary>
        public byte FoodSlot
        {
            set => PutByte(value, 2);
        }

        /// <summary>
        ///     1 for in, 0 for out
        /// </summary>
        public byte MoveType
        {
            set => PutByte(value, 3);
        }

        public uint FoodItemID
        {
            set => PutUInt(value, 4);
        }
    }
}