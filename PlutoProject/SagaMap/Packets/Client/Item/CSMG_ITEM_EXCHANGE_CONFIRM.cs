using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_EXCHANGE_CONFIRM : Packet
    {
        public CSMG_ITEM_EXCHANGE_CONFIRM()
        {
            offset = 2;
        }

        public uint ExchangeType => GetUInt(2);

        public uint InventorySlot => GetUInt(6);

        public uint ExchangeTargetID => GetUInt(10);

        public override Packet New()
        {
            return new CSMG_ITEM_EXCHANGE_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemExchangeConfirm(this);
        }
    }
}