using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_RING_EMBLEM_NEW : Packet
    {
        public CSMG_RING_EMBLEM_NEW()
        {
            size = 6;
            offset = 2;
        }

        public uint RingID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_RING_EMBLEM_NEW();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnRingEmblemNew(this);
        }
    }
}