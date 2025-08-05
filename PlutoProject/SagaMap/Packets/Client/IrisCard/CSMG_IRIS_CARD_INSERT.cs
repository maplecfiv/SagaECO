using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_CARD_INSERT : Packet
    {
        public CSMG_IRIS_CARD_INSERT()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_INSERT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardInsert(this);
        }
    }
}