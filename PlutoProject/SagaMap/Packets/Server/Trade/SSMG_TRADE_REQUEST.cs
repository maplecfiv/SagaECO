using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_REQUEST : Packet
    {
        public SSMG_TRADE_REQUEST()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x0A0C;
        }

        public string Name
        {
            set
            {
                value = value + "\0";
                var buf = Global.Unicode.GetBytes(value);
                var buff = new byte[3 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 2);
                PutBytes(buf, 3);
            }
        }
    }
}