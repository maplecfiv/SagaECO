using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_RING_KICK : Packet
    {
        public CSMG_RING_KICK()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_RING_KICK();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRingKick(this);
        }
    }
}