using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_CHAT_SYSTEM_MESSAGE : Packet
    {
        public enum MessageType
        {
            Yellow,
            Purple
        }

        public SSMG_CHAT_SYSTEM_MESSAGE()
        {
            data = new byte[8];
            ID = 0x00BF;
        }

        public MessageType Type
        {
            set => PutInt((int)value, 2);
        }

        public string Content
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value);
                var buff = new byte[buf.Length + data.Length + 1];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)(buf.Length + 1), 6);
                PutBytes(buf, 7);
            }
        }
    }
}