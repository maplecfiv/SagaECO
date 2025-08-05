using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_KNIGHTWAR_RESULT : Packet
    {
        public SSMG_KNIGHTWAR_RESULT()
        {
            data = new byte[73];
            offset = 2;
            ID = 0x1B6C;
            PutByte(2, 5);
            PutByte(4, 6);
            PutByte(4, 11);
            PutByte(4, 28);
            PutInt(-1, 29);
            PutInt(-1, 33);
            PutInt(-1, 37);
            PutInt(-1, 41);
        }

        public Country Rank1Country
        {
            set => PutByte((byte)value, 7);
        }

        public Country Rank2Country
        {
            set => PutByte((byte)value, 8);
        }

        public Country Rank3ountry
        {
            set => PutByte((byte)value, 9);
        }

        public Country Rank4Country
        {
            set => PutByte((byte)value, 10);
        }

        public int Rank1Point
        {
            set => PutInt(value, 12);
        }

        public int Rank2Point
        {
            set => PutInt(value, 16);
        }

        public int Rank3Point
        {
            set => PutInt(value, 20);
        }

        public int Rank4Point
        {
            set => PutInt(value, 24);
        }

        public int ExpBonus
        {
            set => PutInt(value, 45);
        }

        public int ExpPenalty
        {
            set => PutInt(value, 49);
        }

        public int ExpScoreBonus
        {
            set => PutInt(value, 53);
        }

        public int JexpBonus
        {
            set => PutInt(value, 57);
        }

        public int JexpPenalty
        {
            set => PutInt(value, 61);
        }

        public int JexpScoreBonus
        {
            set => PutInt(value, 65);
        }

        public int CPBouns
        {
            set => PutInt(value, 69);
        }
    }
}