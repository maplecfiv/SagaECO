using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FGarden
{
    public class CSMG_FG_WARE_CLOSE : Packet
    {
        public CSMG_FG_WARE_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_FG_WARE_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenWareClose(this);
        }
    }
}