using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_CHANGE_CANCEL : Packet
    {
        public CSMG_ITEM_CHANGE_CANCEL()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_ITEM_CHANGE_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemChangeCancel(this);
        }
    }
}