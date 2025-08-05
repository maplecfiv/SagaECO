using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Login
{
    public class CSMG_SSO_LOGOUT : Packet
    {
        public CSMG_SSO_LOGOUT()
        {
            offset = 8;
        }


        public override Packet New()
        {
            return new CSMG_SSO_LOGOUT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSSOLogout(this);
        }
    }
}