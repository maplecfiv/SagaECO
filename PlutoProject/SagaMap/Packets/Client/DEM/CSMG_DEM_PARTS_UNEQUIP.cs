using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_PARTS_UNEQUIP : Packet
    {
        public CSMG_DEM_PARTS_UNEQUIP()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_DEM_PARTS_UNEQUIP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMPartsUnequip(this);
        }
    }
}