using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_PING : Packet
    {
        public CSMG_PING()
        {
            size = 6;
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PING();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnPing(this);
        }
    }
}