using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_CARD_ASSEMBLE_CANCEL : Packet
    {
        public CSMG_IRIS_CARD_ASSEMBLE_CANCEL()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_ASSEMBLE_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardAssembleCancel(this);
        }
    }
}