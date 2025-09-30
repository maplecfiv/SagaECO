using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_NO_NYASHIELD : Packet
    {
        public CSMG_NO_NYASHIELD()
        {
            this.offset = 2;
        }

        public override SagaLib.Packet New()
        {
            return (SagaLib.Packet)new SagaMap.Packets.Client.CSMG_NO_NYASHIELD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)(client)).NetIo.Disconnect();
        }

    }
}