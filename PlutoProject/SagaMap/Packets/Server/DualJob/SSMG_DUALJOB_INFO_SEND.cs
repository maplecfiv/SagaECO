using SagaLib;

namespace SagaMap.Packets.Server.DualJob
{
    public class SSMG_DUALJOB_INFO_SEND : Packet
    {
        public SSMG_DUALJOB_INFO_SEND()
        {
            data = new byte[41];
            offset = 2;
            ID = 0x22D4;
        }

        public byte[] JobList
        {
            set => PutBytes(value, 2);
        }

        public byte[] JobLevel
        {
            set => PutBytes(value);
        }
    }
}