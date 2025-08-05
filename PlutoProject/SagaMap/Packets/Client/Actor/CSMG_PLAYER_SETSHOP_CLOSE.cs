using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_SETSHOP_CLOSE : Packet
    {
        public CSMG_PLAYER_SETSHOP_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_SETSHOP_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerShopBuyClose(this);
        }
    }
}