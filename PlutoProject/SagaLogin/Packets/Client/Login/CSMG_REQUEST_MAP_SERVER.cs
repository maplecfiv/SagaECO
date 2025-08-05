using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_REQUEST_MAP_SERVER : Packet
    {
        public CSMG_REQUEST_MAP_SERVER()
        {
            offset = 2;
        }

        public uint Slot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_REQUEST_MAP_SERVER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnRequestMapServer(this);
        }
    }
}