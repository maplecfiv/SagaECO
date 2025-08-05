using SagaLib;

namespace SagaMap.Packets.Server.AnotherAncientArk
{
    public class SSMG_AAA_GROUP_STATE_CHANGE : Packet
    {
        public SSMG_AAA_GROUP_STATE_CHANGE()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0;
        }
    }
}