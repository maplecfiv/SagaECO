using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_CARD_OPEN : Packet
    {
        public CSMG_IRIS_CARD_OPEN()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardOpen(this);
        }
    }
}