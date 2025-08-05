using SagaLib;

namespace SagaMap.Packets.Server.KnightWar
{
    public class SSMG_KNIGHTWAR_TOTAL_SCORE : Packet
    {
        public SSMG_KNIGHTWAR_TOTAL_SCORE()
        {
            data = new byte[42];
            offset = 2;
            ID = 0x1B5C;
            PutUInt(0xffffffff, 22);
            PutUInt(0xffffffff, 26);
            PutUInt(0xffffffff, 30);
            PutUInt(0xffffffff, 34);
            PutUInt(2, 38);
        }

        public int Second
        {
            set => PutInt(value, 2);
        }

        public int EastPoint
        {
            set => PutInt(value, 6);
        }

        public int WestPoint
        {
            set => PutInt(value, 10);
        }

        public int SouthPoint
        {
            set => PutInt(value, 14);
        }

        public int NorthPoint
        {
            set => PutInt(value, 18);
        }
    }
}