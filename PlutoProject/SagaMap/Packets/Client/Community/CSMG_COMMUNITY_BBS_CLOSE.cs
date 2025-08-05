using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_BBS_CLOSE : Packet
    {
        public CSMG_COMMUNITY_BBS_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_COMMUNITY_BBS_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBBSClose(this);
        }
    }
}