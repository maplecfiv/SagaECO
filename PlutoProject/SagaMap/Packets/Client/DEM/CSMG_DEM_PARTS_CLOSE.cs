using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_PARTS_CLOSE : Packet
    {
        public CSMG_DEM_PARTS_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DEM_PARTS_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMPartsClose(this);
        }
    }
}