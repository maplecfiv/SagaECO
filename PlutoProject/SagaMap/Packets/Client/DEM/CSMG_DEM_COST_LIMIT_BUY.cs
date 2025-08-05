using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_COST_LIMIT_BUY : Packet
    {
        public CSMG_DEM_COST_LIMIT_BUY()
        {
            offset = 2;
        }

        public short EP => GetShort(2);

        public override Packet New()
        {
            return new CSMG_DEM_COST_LIMIT_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMCostLimitBuy(this);
        }
    }
}