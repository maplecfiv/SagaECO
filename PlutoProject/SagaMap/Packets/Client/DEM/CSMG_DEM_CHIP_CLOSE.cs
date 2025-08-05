using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_CHIP_CLOSE : Packet
    {
        public CSMG_DEM_CHIP_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DEM_CHIP_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMChipClose(this);
        }
    }
}