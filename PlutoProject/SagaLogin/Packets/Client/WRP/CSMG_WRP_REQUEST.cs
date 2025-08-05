using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_WRP_REQUEST : Packet
    {
        public CSMG_WRP_REQUEST()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_WRP_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnWRPRequest(this);
        }
    }
}