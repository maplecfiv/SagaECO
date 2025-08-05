using SagaLib;

namespace SagaMap.Packets.Login
{
    public class INTERN_LOGIN_REQUEST_CONFIG : Packet
    {
        public INTERN_LOGIN_REQUEST_CONFIG()
        {
            data = new byte[3];
            offset = 2;
            ID = 0xFFF1;
        }

        public Version Version
        {
            set => PutByte((byte)value, 2);
        }
    }
}