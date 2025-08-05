using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SHOWPICT_CANCEL : Packet
    {
        public SSMG_NPC_SHOWPICT_CANCEL()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x05E1;
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }
    }
}