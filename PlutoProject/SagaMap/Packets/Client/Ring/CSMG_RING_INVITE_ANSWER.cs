using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_RING_INVITE_ANSWER : Packet
    {
        private readonly bool accepted;

        public CSMG_RING_INVITE_ANSWER(bool accepted)
        {
            offset = 2;
            this.accepted = accepted;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_RING_INVITE_ANSWER(accepted);
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRingInviteAnswer(this, accepted);
        }
    }
}