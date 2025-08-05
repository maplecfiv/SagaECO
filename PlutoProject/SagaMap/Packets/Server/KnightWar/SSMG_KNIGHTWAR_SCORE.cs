using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_KNIGHTWAR_SCORE : Packet
    {
        public SSMG_KNIGHTWAR_SCORE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1B62;
        }

        public int Score
        {
            set => PutInt(value, 2);
        }

        public int DeathCount
        {
            set => PutInt(value, 6);
        }
    }
}