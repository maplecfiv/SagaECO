using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_FURNITURE_MOTION : Packet
    {
        public SSMG_FG_FURNITURE_MOTION()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1C08;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ushort Motion
        {
            set => PutUShort(value, 6);
        }

        public ushort EndMotion
        {
            set => PutUShort(value, 8);
        }

        public short Z
        {
            set => PutShort(value, 10);
        }

        public ushort Dir
        {
            set => PutUShort(value, 12);
        }
    }
}