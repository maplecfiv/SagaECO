using SagaLib;

namespace SagaLogin.Packets.Server.Tool
{
    public class SSMG_TOOL_RESULT : Packet
    {
        public SSMG_TOOL_RESULT()
        {
            data = new byte[5];
            ID = 0xDDDE;
        }

        public byte type
        {
            set => PutByte(value, 2);
        }

        public string Text
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[buf.Length + 4];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 3);
                PutBytes(buf, 4);
            }
        }
    }
}