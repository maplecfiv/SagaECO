using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_SEND_TALK : Packet
    {
        public SSMG_PARTNER_SEND_TALK()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x219f;
        }

        public uint PartnerID
        {
            set => PutUInt(value, 2);
        }

        public uint Parturn
        {
            set => PutUInt(value, 6);
        }
    }
}