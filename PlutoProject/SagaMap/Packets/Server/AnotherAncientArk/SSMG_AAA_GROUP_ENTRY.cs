using SagaLib;

namespace SagaMap.Packets.Server.AnotherAncientArk
{
    public class SSMG_AAA_GROUP_ENTRY : Packet
    {
        public SSMG_AAA_GROUP_ENTRY()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x23E3;
        }

        public int GroupID
        {
            set => PutInt(value, 3);
        }
    }
}