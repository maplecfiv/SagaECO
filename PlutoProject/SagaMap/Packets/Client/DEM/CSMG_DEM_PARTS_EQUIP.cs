using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_PARTS_EQUIP : Packet
    {
        public CSMG_DEM_PARTS_EQUIP()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_DEM_PARTS_EQUIP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMPartsEquip(this);
        }
    }
}