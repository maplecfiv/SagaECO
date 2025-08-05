using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_COMMUNITY_RECRUIT_DELETE : Packet
    {
        public CSMG_COMMUNITY_RECRUIT_DELETE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_COMMUNITY_RECRUIT_DELETE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRecruitDelete(this);
        }
    }
}