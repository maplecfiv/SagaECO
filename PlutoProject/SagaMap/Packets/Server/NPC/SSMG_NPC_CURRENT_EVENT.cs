using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_CURRENT_EVENT : Packet
    {
        public SSMG_NPC_CURRENT_EVENT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x05E4;
        }

        public uint EventID
        {
            set => PutUInt(value, 2);
        }
    }
}