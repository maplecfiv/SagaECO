using SagaLib;

namespace SagaMap.Packets.Server.Quest
{
    public class SSMG_QUEST_POINT : Packet
    {
        public SSMG_QUEST_POINT()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x196E;
        }

        public ushort QuestPoint
        {
            set => PutUShort(value, 2);
        }

        public uint ResetTime
        {
            set => PutUInt(value, 4);
        }
    }
}