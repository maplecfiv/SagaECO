using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_RING_INFO_UPDATE : Packet
    {
        public SSMG_RING_INFO_UPDATE()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x1AD8;
        }

        public uint RingID
        {
            set => PutUInt(value, 2);
        }

        public uint Fame
        {
            set => PutUInt(value, 6);
        }

        public byte CurrentMember
        {
            set => PutByte(value, 10);
        }

        public byte MaxMember
        {
            set => PutByte(value, 11);
        }
    }
}