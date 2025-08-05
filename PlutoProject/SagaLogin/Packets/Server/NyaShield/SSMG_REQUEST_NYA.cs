using SagaLib;

namespace SagaLogin.Packets.Server.NyaShield
{
    public class SSMG_REQUEST_NYA : Packet
    {
        public SSMG_REQUEST_NYA()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0150;
        }
    }
}