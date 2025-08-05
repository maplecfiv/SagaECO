using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_CHAT_JOB : Packet
    {
        public SSMG_CHAT_JOB()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x0425;
        }

        public byte Type
        {
            set => PutByte(value, 2);
        }

        public string Sender
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + 5];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 3);
                PutBytes(buf, 4);
            }
        }

        public string Content
        {
            set
            {
                var size = GetByte(3);
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + data.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, (ushort)(4 + size));
                PutBytes(buf, (ushort)(5 + size));
            }
        }
    }
}