using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_SIGN : Packet
    {
        public CSMG_CHAT_SIGN()
        {
            offset = 2;
        }

        public string Content
        {
            get
            {
                var size = GetByte(2);
                size--;
                return Global.Unicode.GetString(GetBytes(size, 3));
            }
        }

        public override Packet New()
        {
            return new CSMG_CHAT_SIGN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSign(this);
        }
    }
}