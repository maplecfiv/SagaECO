using SagaLib;

namespace SagaMap.Packets.Server.AnotherAncientArk
{
    public class SSMG_AAA_GROUP_SELECT : Packet
    {
        public SSMG_AAA_GROUP_SELECT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0;
        }
    }
}