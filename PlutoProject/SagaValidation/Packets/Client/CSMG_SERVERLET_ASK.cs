using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client
{
    public class CSMG_SERVERLET_ASK : Packet
    {
        public CSMG_SERVERLET_ASK()
        {
            offset = 2;
        }
        public override Packet New()
        {
            return (Packet)new CSMG_SERVERLET_ASK();
        }
    }
}
