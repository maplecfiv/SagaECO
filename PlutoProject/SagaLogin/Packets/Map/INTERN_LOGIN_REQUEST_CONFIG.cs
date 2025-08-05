using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Map
{
    public class INTERN_LOGIN_REQUEST_CONFIG : Packet
    {
        public INTERN_LOGIN_REQUEST_CONFIG()
        {
            offset = 2;
        }

        public Version Version => (Version)GetByte(2);

        public override Packet New()
        {
            return new INTERN_LOGIN_REQUEST_CONFIG();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnInternMapRequestConfig(this);
        }
    }
}