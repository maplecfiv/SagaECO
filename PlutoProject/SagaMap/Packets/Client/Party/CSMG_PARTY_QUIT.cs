using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTY_QUIT : Packet
    {
        public CSMG_PARTY_QUIT()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PARTY_QUIT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyQuit(this);
        }
    }
}