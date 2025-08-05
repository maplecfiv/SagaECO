using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_QUEST_SELECT : Packet
    {
        public CSMG_QUEST_SELECT()
        {
            offset = 2;
        }

        public uint QuestID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_QUEST_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnQuestSelect(this);
        }
    }
}