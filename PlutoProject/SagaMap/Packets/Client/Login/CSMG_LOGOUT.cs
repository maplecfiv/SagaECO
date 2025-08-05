using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_LOGOUT : Packet
    {
        public enum Results
        {
            OK,
            CANCEL = 0xf9,
            FAILED = 0xff
        }

        public CSMG_LOGOUT()
        {
            offset = 8;
        }

        public Results Result => (Results)GetByte(2);

        public override Packet New()
        {
            return new CSMG_LOGOUT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnLogout(this);
        }
    }
}