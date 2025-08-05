using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_FG_WARE_GET : Packet
    {
        public CSMG_FG_WARE_GET()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public ushort Count => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_FG_WARE_GET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenWareGet(this);
        }
    }
}