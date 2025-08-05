using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_MAP_LOADED : Packet
    {
        public CSMG_PLAYER_MAP_LOADED()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_MAP_LOADED();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnMapLoaded(this);
        }
    }
}