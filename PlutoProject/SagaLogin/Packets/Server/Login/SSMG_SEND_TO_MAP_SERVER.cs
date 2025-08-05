using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_SEND_TO_MAP_SERVER : Packet
    {
        public SSMG_SEND_TO_MAP_SERVER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x33;
        }

        public byte ServerID
        {
            set => PutByte(value, 2);
        }

        public string IP
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value);
                var size = (byte)(buf.Length + 1);
                var buff = new byte[size + 8];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte(size, 3);
                PutBytes(buf, 4);
            }
        }

        public int Port
        {
            set => PutInt(value, GetDataOffset());
        }

        private byte GetDataOffset()
        {
            var size = GetByte(3);
            return (byte)(4 + size);
        }
    }
}