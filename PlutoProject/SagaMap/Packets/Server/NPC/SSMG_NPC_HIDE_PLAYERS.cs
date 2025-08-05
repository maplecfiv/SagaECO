using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_HIDE_PLAYERS : Packet
    {
        public SSMG_NPC_HIDE_PLAYERS()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x0613;
        }

        public uint unknown1
        {
            set => PutUInt(value, 2);
        }

        public uint unknown2
        {
            set => PutUInt(value, 6);
        }

        public byte unknown3
        {
            set => PutByte(value, 10);
        }
    }
}