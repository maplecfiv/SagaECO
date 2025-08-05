using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_DESTROY : Packet
    {
        public SSMG_AAA_GROUP_DESTROY()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x23E1;
        }

        public byte Reason
        {
            set => PutByte(value, 2);
        }
    }
}