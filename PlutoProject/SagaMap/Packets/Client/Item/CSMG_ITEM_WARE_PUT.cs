using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_WARE_PUT : Packet
    {
        public CSMG_ITEM_WARE_PUT()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public ushort Count => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_ITEM_WARE_PUT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemWarePut(this);
        }
    }
}