using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_NO_NYASHIELD : Packet
    {
        public CSMG_NO_NYASHIELD()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return (Packet)new CSMG_NO_NYASHIELD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)(client)).NetIo.Disconnect();
        }

    }
}