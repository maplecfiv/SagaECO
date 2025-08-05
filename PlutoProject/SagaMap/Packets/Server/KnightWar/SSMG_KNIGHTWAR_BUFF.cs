using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_KNIGHTWAR_BUFF : Packet
    {
        public SSMG_KNIGHTWAR_BUFF()
        {
            data = new byte[25];
            offset = 2;
            ID = 0x1B5E;
        }

        public byte exp
        {
            set => PutByte(value, 2);
        }

        public byte buffID
        {
            set => PutByte(value, 3);
        }

        public int time
        {
            set
            {
                PutInt(value, 5);
                PutInt(value, 10);
                PutInt(value, 15);
                PutInt(value, 20);
            }
        }
    }
}