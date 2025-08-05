using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SS_MODE : Packet
    {
        public SSMG_NPC_SS_MODE()
        {
            data = new byte[18];
            offset = 2;
            ID = 0x0606;
        }

        public ushort Toggle
        {
            set => PutUShort(value, 4);
        }

        public ushort UI
        {
            set => PutUShort(value, 8);
        }

        public ushort X
        {
            set => PutUShort(value, 12);
        }

        public ushort Y
        {
            set => PutUShort(value, 16);
        }

        public byte unknown
        {
            set => PutByte(value, 18);
        }
    }
}