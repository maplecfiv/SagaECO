using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_CARD_CLOSE : Packet
    {
        public CSMG_IRIS_CARD_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardClose(this);
        }
    }
}