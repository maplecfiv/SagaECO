using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_OUT : Packet
    {
        public SSMG_AAA_GROUP_OUT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x23E6;
        }

        public byte Reason
        {
            set => PutByte(value, 2);
        }
    }
}