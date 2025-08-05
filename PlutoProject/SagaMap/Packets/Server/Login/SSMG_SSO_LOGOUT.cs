using SagaLib;

namespace SagaMap.Packets.Server.Login
{
    public class SSMG_SSO_LOGOUT : Packet
    {
        public SSMG_SSO_LOGOUT()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x001D;
        }
    }
}