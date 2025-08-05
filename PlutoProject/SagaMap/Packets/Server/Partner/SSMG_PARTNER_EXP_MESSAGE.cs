using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_EXP_MESSAGE : Packet
    {
        public SSMG_PARTNER_EXP_MESSAGE()
        {
            data = new byte[15];
            offset = 2;
            ID = 0x2196;
        }

        public long EXP
        {
            set => PutLong(value, 3);
        }

        public int Reliability
        {
            set => PutInt(value, 11);
        }
    }
}