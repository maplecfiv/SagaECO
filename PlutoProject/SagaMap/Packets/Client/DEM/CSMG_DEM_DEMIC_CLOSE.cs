using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_DEMIC_CLOSE : Packet
    {
        public CSMG_DEM_DEMIC_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DEM_DEMIC_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMDemicClose(this);
        }
    }
}