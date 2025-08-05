using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SYNTHESE_RESULT : Packet
    {
        public SSMG_NPC_SYNTHESE_RESULT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13B8;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }
    }
}