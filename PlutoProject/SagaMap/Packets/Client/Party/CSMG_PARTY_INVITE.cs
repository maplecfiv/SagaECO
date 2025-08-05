using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Party
{
    public class CSMG_PARTY_INVITE : Packet
    {
        public CSMG_PARTY_INVITE()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PARTY_INVITE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyInvite(this);
        }
    }
}