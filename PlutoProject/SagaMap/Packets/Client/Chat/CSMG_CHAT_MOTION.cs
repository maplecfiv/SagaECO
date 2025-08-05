using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_MOTION : Packet
    {
        public CSMG_CHAT_MOTION()
        {
            offset = 2;
        }

        public MotionType Motion => (MotionType)GetUShort(2);

        public byte Loop => GetByte(4);

        public override Packet New()
        {
            return new CSMG_CHAT_MOTION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnMotion(this);
        }
    }
}