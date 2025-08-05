using SagaLib;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_RECRUIT_DELETE : Packet
    {
        public SSMG_COMMUNITY_RECRUIT_DELETE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1B95;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}