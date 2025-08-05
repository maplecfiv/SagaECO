using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_ELEMENTS : Packet
    {
        public CSMG_PLAYER_ELEMENTS()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_ELEMENTS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerElements(this);
        }
    }
}