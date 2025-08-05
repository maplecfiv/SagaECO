using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_CHAT_SIGN : Packet
    {
        public SSMG_CHAT_SIGN()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x041B;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public string Message
        {
            set
            {
                if (value != "")
                {
                    if (value.Substring(value.Length - 1, 1) != "\0") value += "\0";
                    var buf = Global.Unicode.GetBytes(value);
                    var buff = new byte[buf.Length + 7];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, 6);
                    PutBytes(buf, 7);
                }
                else
                {
                    PutByte(1, 6);
                }
            }
        }
    }
}