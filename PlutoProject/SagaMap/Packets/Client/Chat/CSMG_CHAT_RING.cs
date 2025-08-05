using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_CHAT_RING : Packet
    {
        public CSMG_CHAT_RING()
        {
            offset = 2;
        }

        public string Sender
        {
            get
            {
                var size = GetByte(2);
                var buf = Global.Unicode.GetString(GetBytes(size, 3));
                return buf.Replace("\0", "");
            }
        }

        public string Content
        {
            get
            {
                var sender = GetByte(2);
                var size = GetByte((ushort)(3 + sender));
                var buf = Global.Unicode.GetString(GetBytes(size, (ushort)(4 + sender)));
                return buf.Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_CHAT_RING();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnChatRing(this);
        }
    }
}