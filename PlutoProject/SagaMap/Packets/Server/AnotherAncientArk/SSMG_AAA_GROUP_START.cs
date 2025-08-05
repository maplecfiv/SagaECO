using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_START : Packet
    {
        public SSMG_AAA_GROUP_START()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x0;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }
    }
}