using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Party
{
    public class CSMG_PARTY_KICK : Packet
    {
        public CSMG_PARTY_KICK()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PARTY_KICK();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyKick(this);
        }
    }
}