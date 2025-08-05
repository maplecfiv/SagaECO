using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_KNIGHTWAR_STATUS : Packet
    {
        public SSMG_KNIGHTWAR_STATUS()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x1B5D;
        }

        public int EastPoint
        {
            set => PutInt(value, 2);
        }

        public int WestPoint
        {
            set => PutInt(value, 6);
        }

        public int SouthPoint
        {
            set => PutInt(value, 10);
        }

        public int NorthPoint
        {
            set => PutInt(value, 14);
        }
    }
}