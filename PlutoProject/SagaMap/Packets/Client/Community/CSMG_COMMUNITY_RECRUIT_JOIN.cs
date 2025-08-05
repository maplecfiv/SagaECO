using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_RECRUIT_JOIN : Packet
    {
        public CSMG_COMMUNITY_RECRUIT_JOIN()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_COMMUNITY_RECRUIT_JOIN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRecruitJoin(this);
        }
    }
}