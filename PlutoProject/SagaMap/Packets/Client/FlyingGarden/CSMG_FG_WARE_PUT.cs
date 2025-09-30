using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingGarden
{
    public class CSMG_FG_WARE_PUT : Packet
    {
        public CSMG_FG_WARE_PUT()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public ushort Count => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_FG_WARE_PUT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenWarePut(this);
        }
    }
}