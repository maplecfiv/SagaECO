using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_CHAR_STATUS : Packet
    {
        public CSMG_CHAR_STATUS()
        {
            offset = 2;
        }

        public byte Slot => GetByte(2);

        public override Packet New()
        {
            return new CSMG_CHAR_STATUS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnCharStatus(this);
        }
    }
}