using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_TITLE_REQUIRE : Packet
    {
        public CSMG_PLAYER_TITLE_REQUIRE()
        {
            offset = 2;
        }

        public uint tID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_TITLE_REQUIRE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerTitleRequire(this);
        }
    }
}