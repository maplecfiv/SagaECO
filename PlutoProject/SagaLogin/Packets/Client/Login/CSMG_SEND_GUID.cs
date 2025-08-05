using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_SEND_GUID : Packet
    {
        public CSMG_SEND_GUID()
        {
            size = 360;
            offset = 8;
        }

        public override Packet New()
        {
            return new CSMG_SEND_GUID();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnSendGUID(this);
        }
    }
}