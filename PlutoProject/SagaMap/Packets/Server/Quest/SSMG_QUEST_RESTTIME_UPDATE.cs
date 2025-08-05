using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_QUEST_RESTTIME_UPDATE : Packet
    {
        public SSMG_QUEST_RESTTIME_UPDATE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1978;
        }

        public int RestTime
        {
            set => PutInt(value, 2);
        }
    }
}