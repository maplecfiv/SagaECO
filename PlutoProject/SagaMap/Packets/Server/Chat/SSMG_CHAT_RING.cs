using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_CHAT_RING : Packet
    {
        public SSMG_CHAT_RING()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0411;
        }

        public string Sender
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + 4];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 2);
                PutBytes(buf, 3);
            }
        }

        public string Content
        {
            set
            {
                var size = GetByte(2);
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + data.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, (ushort)(3 + size));
                PutBytes(buf, (ushort)(4 + size));
            }
        }
    }
}