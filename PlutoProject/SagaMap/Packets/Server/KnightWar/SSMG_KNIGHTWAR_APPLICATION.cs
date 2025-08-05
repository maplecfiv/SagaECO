using SagaLib;

namespace SagaMap.Packets.Server.KnightWar
{
    public class SSMG_KNIGHTWAR_APPLICATION : Packet
    {
        public SSMG_KNIGHTWAR_APPLICATION()
        {
            data = new byte[22];
            offset = 2;
            ID = 0x1B58;
        }

        public int Time
        {
            set => PutInt(value, 2);
        }

        public int EastCount
        {
            set => PutInt(value, 6);
        }

        public int WestCount
        {
            set => PutInt(value, 10);
        }

        public int SouthCount
        {
            set => PutInt(value, 14);
        }

        public int NorthCount
        {
            set => PutInt(value, 18);
        }
    }
}