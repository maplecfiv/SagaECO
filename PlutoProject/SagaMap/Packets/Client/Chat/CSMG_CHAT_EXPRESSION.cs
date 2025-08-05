using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_EXPRESSION : Packet
    {
        public CSMG_CHAT_EXPRESSION()
        {
            offset = 2;
        }

        public byte Motion => GetByte(2);

        public byte Loop => GetByte(3);

        public override Packet New()
        {
            return new CSMG_CHAT_EXPRESSION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnExpression(this);
        }
    }
}