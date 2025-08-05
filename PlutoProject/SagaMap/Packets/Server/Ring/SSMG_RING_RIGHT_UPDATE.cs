using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_RING_RIGHT_UPDATE : Packet
    {
        public SSMG_RING_RIGHT_UPDATE()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x1AD7;
        }

        public uint Unknown
        {
            set => PutUInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public int Right
        {
            set => PutInt(value, 10);
        }
    }
}