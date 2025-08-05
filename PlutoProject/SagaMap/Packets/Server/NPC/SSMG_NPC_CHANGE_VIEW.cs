using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_CHANGE_VIEW : Packet
    {
        public SSMG_NPC_CHANGE_VIEW()
        {
            data = new byte[10];
            offset = 2;
            ID = 0xFFFE;
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }

        public uint MobID
        {
            set => PutUInt(value, 6);
        }
    }
}