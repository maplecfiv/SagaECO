using SagaLib;

namespace SagaValidation.Packets.Server
{
    public class SSMG_SERVER_LST_SEND : Packet
    {
        public SSMG_SERVER_LST_SEND()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x33;
        }
        public byte size;
        public string SevName
        {
            set
            {
                byte[] buf = Global.Unicode.GetBytes(value + "\0");
                byte[] buff = new byte[buf.Length + 4];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 2);
                PutBytes(buf, 3);
                size = (byte)(buf.Length + 3);
            }
        }
        public string SevIP
        {
            set
            {
                byte[] buf = Global.Unicode.GetBytes(value + "\0");
                byte[] buff = new byte[buf.Length + data.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, (ushort)(size));
                PutBytes(buf, (ushort)(1 + size));
            }
        }
    }
}