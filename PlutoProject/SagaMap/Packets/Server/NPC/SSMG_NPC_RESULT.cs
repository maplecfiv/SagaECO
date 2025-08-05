using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_EVENT_START_RESULT : Packet
    {
        public SSMG_NPC_EVENT_START_RESULT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x05E7;
            PutUInt(0, 6); //unknown
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }
    }
}