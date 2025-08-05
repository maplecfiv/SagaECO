using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_PONG : Packet
    {
        public SSMG_PONG()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0B;
        }
    }
}