using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_MESSAGE_END : Packet
    {
        public SSMG_NPC_MESSAGE_END()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x03FA;
        }
    }
}