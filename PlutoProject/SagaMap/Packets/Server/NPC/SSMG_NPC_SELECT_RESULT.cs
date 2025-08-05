using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SELECT_RESULT : Packet
    {
        public SSMG_NPC_SELECT_RESULT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x05F8;
            PutByte(0, 2); //unknown
        }
    }
}