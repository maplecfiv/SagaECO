using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_CHAR_DELETE_ACK : Packet
    {
        public enum Result
        {
            OK = 0,
            WRONG_DELETE_PASSWORD = 0x9C
        }

        public SSMG_CHAR_DELETE_ACK()
        {
            data = new byte[3];
            offset = 2;
            ID = 0xA6;
        }

        public Result DeleteResult
        {
            set => PutByte((byte)value, 2);
        }
    }
}