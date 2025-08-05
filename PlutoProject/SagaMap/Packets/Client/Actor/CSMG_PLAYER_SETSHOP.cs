using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_SETSHOP_OPEN : Packet
    {
        public CSMG_PLAYER_SETSHOP_OPEN()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_SETSHOP_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerSetShop(this);
            ((MapClient)client).OnPlayerShopChangeClose(this);
        }
    }
}