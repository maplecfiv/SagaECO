using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_GREETINGS : Packet
    {
        public CSMG_PLAYER_GREETINGS()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_GREETINGS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerGreetings(this);
        }
    }
}