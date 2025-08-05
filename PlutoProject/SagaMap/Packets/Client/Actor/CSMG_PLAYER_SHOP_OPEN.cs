using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_SHOP_OPEN : Packet
    {
        public CSMG_PLAYER_SHOP_OPEN()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_SHOP_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerShopOpen(this);
        }
    }
}