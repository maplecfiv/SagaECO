using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_EVENT_START : Packet
    {
        public SSMG_NPC_EVENT_START()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x05DC;
        }
    }
}