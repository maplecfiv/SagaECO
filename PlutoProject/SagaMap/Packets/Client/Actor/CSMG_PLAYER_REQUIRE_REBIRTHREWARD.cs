using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_REQUIRE_REBIRTHREWARD : Packet
    {
        public CSMG_PLAYER_REQUIRE_REBIRTHREWARD()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_REQUIRE_REBIRTHREWARD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRequireRebirthReward(this);
        }
    }
}