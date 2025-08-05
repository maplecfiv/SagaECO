using SagaLib;

namespace SagaLogin.Packets.Server.Chat
{
    public class SSMG_CHAT_WHISPER_FAILED : Packet
    {
        public SSMG_CHAT_WHISPER_FAILED()
        {
            data = new byte[7];
            ID = 0x00CA;
        }

        public uint Result
        {
            set => PutUInt(value, 2);
        }

        public string Receiver
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + 7];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, (ushort)6);
                PutBytes(buf, (ushort)7);
            }
        }
    }
}