using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SHOWPICT_VIEW : Packet
    {
        public SSMG_NPC_SHOWPICT_VIEW()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x05E8;
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }

        public uint PictID
        {
            set => PutUInt(value, 6);
        }
    }
}