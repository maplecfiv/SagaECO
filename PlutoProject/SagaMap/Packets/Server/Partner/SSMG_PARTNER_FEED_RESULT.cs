using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_FEED_RESULT : Packet
    {
        //should be sent when the next feed is available.
        public SSMG_PARTNER_FEED_RESULT()
        {
            data = new byte[18];
            offset = 2;
            ID = 0x219C;
        }

        /// <summary>
        ///     0 for no message shown, 1 for message shown
        /// </summary>
        public byte MessageSwitch
        {
            set => PutByte(value, 2);
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 3);
        }

        public uint FoodItemID
        {
            set => PutUInt(value, 7);
        }

        /// <summary>
        ///     100=1.00
        /// </summary>
        public ushort ReliabilityUpRate
        {
            set => PutUShort(value, 11);
        }

        /// <summary>
        ///     seconds
        /// </summary>
        public uint NextFeedTime
        {
            set => PutUInt(value, 13);
        }

        public byte PartnerRank
        {
            set => PutByte(value, 17);
        }
    }
}