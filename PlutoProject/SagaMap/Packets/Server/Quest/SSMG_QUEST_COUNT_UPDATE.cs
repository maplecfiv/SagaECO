using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_QUEST_COUNT_UPDATE : Packet
    {
        public SSMG_QUEST_COUNT_UPDATE()
        {
            data = new byte[15];
            offset = 2;
            ID = 0x1973;
            PutByte(3, 2);
        }

        public int Count1
        {
            set => PutInt(value, 3);
        }

        public int Count2
        {
            set => PutInt(value, 7);
        }

        public int Count3
        {
            set => PutInt(value, 11);
        }
    }
}