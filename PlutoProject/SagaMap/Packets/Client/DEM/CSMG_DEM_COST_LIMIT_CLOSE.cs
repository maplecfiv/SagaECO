using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_COST_LIMIT_CLOSE : Packet
    {
        public CSMG_DEM_COST_LIMIT_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DEM_COST_LIMIT_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMCostLimitClose(this);
        }
    }
}