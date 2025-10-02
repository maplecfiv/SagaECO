using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client
{
    public class CSMG_UNKNOWN_LIST : Packet
    {
        public CSMG_UNKNOWN_LIST()
        {
            size = 6;
            offset = 2;
        }

        public override Packet New()
        {
            return (Packet)new CSMG_UNKNOWN_LIST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((ValidationClient)(client)).OnUnknownList(this);
        }

    }
}