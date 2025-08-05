using SagaDB.Quests;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_QUEST_STATUS_UPDATE : Packet
    {
        public SSMG_QUEST_STATUS_UPDATE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x1987;
        }

        public QuestStatus Status
        {
            set => PutByte((byte)value, 2);
        }
    }
}