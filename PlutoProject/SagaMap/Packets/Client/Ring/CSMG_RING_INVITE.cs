using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_RING_INVITE : Packet
    {
        public CSMG_RING_INVITE()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_RING_INVITE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRingInvite(this);
        }
    }
}