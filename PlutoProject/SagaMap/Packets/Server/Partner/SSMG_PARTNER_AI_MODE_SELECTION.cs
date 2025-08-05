using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_AI_MODE_SELECTION : Packet
    {
        public SSMG_PARTNER_AI_MODE_SELECTION()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x2181;
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     start at 0
        /// </summary>
        public byte AIMode
        {
            set => PutByte(value, 6);
        }

        public byte unknown0
        {
            set => PutByte(value, 7);
        }
    }
}