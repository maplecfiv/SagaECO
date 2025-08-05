using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_CHIP_CATEGORY : Packet
    {
        public CSMG_DEM_CHIP_CATEGORY()
        {
            offset = 2;
        }

        public uint Category => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_DEM_CHIP_CATEGORY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMChipCategory(this);
        }
    }
}