using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_COMMUNITY_RECRUIT_JOIN_RES : Packet
    {
        public SSMG_COMMUNITY_RECRUIT_JOIN_RES()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1BA9;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }
    }
}