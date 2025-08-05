using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_NAVIGATION_CANCEL : Packet
    {
        public SSMG_NPC_NAVIGATION_CANCEL()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1A2D;
            PutByte(0, 2);
        }
    }
}