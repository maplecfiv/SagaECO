using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTY_INVITE_ANSWER : Packet
    {
        public CSMG_PARTY_INVITE_ANSWER()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PARTY_INVITE_ANSWER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyInviteAnswer(this);
        }
    }
}