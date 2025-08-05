using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_GOLEM_SHOP_OPEN : Packet
    {
        public CSMG_GOLEM_SHOP_OPEN()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopOpen(this);
        }
    }
}