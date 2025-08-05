using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SHOWPICT_LOCATION : Packet
    {
        public SSMG_NPC_SHOWPICT_LOCATION()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x05E0;
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }

        public byte X
        {
            set => PutByte(value, 6);
        }

        public byte Y
        {
            set => PutByte(value, 7);
        }

        public byte Dir
        {
            set => PutByte(value, 8);
        }
    }
}