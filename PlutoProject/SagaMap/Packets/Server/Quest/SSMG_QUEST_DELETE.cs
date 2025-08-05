using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_QUEST_DELETE : Packet
    {
        public SSMG_QUEST_DELETE()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x198C;
        }
    }
}