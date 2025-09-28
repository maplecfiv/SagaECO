using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_CASTLE_APPEAR : Packet
    {
        public SSMG_FF_CASTLE_APPEAR()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x2044;
        }

        public uint ActorID
        {
            set => PutUInt(1, 2);
        }

        public uint UnknownID
        {
            set => PutUInt(0, 6);
        }

        public ushort X
        {
            set => PutUShort(value, 10);
        }

        public ushort Z
        {
            set => PutUShort(value, 12);
        }

        public ushort Yaxis
        {
            set => PutUShort(value, 14);
        }
    }
}