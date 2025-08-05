using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_RECRUIT_REQUEST_ANS : Packet
    {
        public CSMG_COMMUNITY_RECRUIT_REQUEST_ANS()
        {
            offset = 2;
        }

        public bool Accept => GetUInt(2) == 1;

        public uint CharID => GetUInt(6);

        public string CharName => Global.Unicode.GetString(GetBytes(GetByte(10), 11));

        public override Packet New()
        {
            return new CSMG_COMMUNITY_RECRUIT_REQUEST_ANS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRecruitRequestAns(this);
        }
    }
}