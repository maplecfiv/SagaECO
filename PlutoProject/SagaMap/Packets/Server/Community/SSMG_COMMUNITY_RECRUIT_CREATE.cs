using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_COMMUNITY_RECRUIT_CREATE : Packet
    {
        public SSMG_COMMUNITY_RECRUIT_CREATE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1B8B;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}