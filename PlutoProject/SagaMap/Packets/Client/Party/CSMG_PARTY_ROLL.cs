using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTY_ROLL : Packet
    {
        public CSMG_PARTY_ROLL()
        {
            offset = 2;
        }

        public uint status => GetByte(2);

        public override Packet New()
        {
            return new CSMG_PARTY_ROLL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyRoll(this);
        }
    }
}