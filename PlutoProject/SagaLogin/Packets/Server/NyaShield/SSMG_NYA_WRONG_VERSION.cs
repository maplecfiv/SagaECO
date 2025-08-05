using SagaLib;

namespace SagaLogin.Packets.Server.NyaShield
{
    public class SSMG_NYA_WRONG_VERSION : Packet
    {
        public SSMG_NYA_WRONG_VERSION()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0152;
        }
    }
}