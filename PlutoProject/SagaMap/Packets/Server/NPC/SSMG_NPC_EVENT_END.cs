using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_EVENT_END : Packet
    {
        public SSMG_NPC_EVENT_END()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x05DD;
        }
    }
}