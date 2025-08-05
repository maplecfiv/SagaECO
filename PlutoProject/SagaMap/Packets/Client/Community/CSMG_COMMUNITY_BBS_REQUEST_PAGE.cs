using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_BBS_REQUEST_PAGE : Packet
    {
        public CSMG_COMMUNITY_BBS_REQUEST_PAGE()
        {
            offset = 2;
        }

        public int Page => GetInt(2);

        public override Packet New()
        {
            return new CSMG_COMMUNITY_BBS_REQUEST_PAGE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBBSRequestPage(this);
        }
    }
}