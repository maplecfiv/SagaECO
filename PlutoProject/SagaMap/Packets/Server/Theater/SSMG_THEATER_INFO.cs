using SagaLib;

namespace SagaMap.Packets.Server.Theater
{
    public class SSMG_THEATER_INFO : Packet
    {
        public enum Type
        {
            MESSAGE = 0x0A,
            MOVIE_ADDRESS = 0x14,
            STOP_BGM = 0x1F,
            PLAY = 0x28
        }

        public SSMG_THEATER_INFO()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1A90;
        }

        public Type MessageType
        {
            set => PutUInt((uint)value, 2);
        }

        public string Message
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[7 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, 6);
                PutBytes(buf, 7);
            }
        }
    }
}