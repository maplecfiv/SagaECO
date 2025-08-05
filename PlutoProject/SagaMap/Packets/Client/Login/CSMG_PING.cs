using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PING : Packet
    {
        public CSMG_PING()
        {
            size = 2;
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PING();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPing(this);
        }
    }
}