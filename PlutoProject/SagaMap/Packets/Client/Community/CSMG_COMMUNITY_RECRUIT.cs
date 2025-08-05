using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_COMMUNITY_RECRUIT : Packet
    {
        public CSMG_COMMUNITY_RECRUIT()
        {
            offset = 2;
        }

        public RecruitmentType Type => (RecruitmentType)GetByte(2);

        public int Page => GetInt(3);

        public override Packet New()
        {
            return new CSMG_COMMUNITY_RECRUIT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRecruit(this);
        }
    }
}