using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_HIDE : Packet
    {
        public SSMG_NPC_HIDE()
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