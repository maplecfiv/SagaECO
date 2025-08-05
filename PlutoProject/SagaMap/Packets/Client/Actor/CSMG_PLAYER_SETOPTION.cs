using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_SETOPTION : Packet
    {
        public CSMG_PLAYER_SETOPTION()
        {
            offset = 2;
        }

        public uint GetOption => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_SETOPTION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerSetOption(this);
        }
    }
}