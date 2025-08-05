using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SHOW : Packet
    {
        public SSMG_NPC_SHOW()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x05DF;
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }
    }
}