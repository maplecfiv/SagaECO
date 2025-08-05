using SagaLib;

namespace SagaMap.Packets.Server.AnotherAncientArk
{
    public class SSMG_AAA_GROUP_WHO_OUT : Packet
    {
        public SSMG_AAA_GROUP_WHO_OUT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x23E7;
        }

        public byte Position
        {
            set => PutByte(value, 2);
        }

        public byte Reason
        {
            set => PutByte(value, 3);
        }
    }
}