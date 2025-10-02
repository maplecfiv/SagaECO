using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client
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
            return (Packet)new CSMG_PING();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((ValidationClient)(client)).OnPing(this);
        }

    }
}