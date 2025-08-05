using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_RETURN_HOME : Packet
    {
        public CSMG_PLAYER_RETURN_HOME()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_RETURN_HOME();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerReturnHome(this);
        }
    }
}