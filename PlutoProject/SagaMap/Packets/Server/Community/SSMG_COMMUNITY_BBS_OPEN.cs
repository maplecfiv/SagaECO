using SagaLib;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_BBS_OPEN : Packet
    {
        public SSMG_COMMUNITY_BBS_OPEN()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1AF4;
        }

        public uint Gold
        {
            set => PutUInt(value, 2);
        }
    }
}