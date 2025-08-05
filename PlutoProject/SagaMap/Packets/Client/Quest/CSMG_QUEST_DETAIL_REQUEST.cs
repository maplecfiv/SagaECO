using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_QUEST_DETAIL_REQUEST : Packet
    {
        public CSMG_QUEST_DETAIL_REQUEST()
        {
            offset = 2;
        }

        public uint QuestID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_QUEST_DETAIL_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnQuestDetailRequest(this);
        }
    }
}