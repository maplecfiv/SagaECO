using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_AI_SETUP_RESULT : Packet
    {
        public SSMG_PARTNER_AI_SETUP_RESULT()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x2185;
        }

        /// <summary>
        ///     0 for success, 1 for failure
        /// </summary>
        public byte Success
        {
            set => PutByte(value, 2);
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 3);
        }
    }
}