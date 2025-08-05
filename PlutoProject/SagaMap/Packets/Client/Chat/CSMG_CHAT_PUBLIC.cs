using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_PUBLIC : Packet
    {
        public CSMG_CHAT_PUBLIC()
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
            return new CSMG_CHAT_PUBLIC();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnChat(this);
        }
    }
}