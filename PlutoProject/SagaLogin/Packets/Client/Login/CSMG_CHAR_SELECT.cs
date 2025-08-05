using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_CHAR_SELECT : Packet
    {
        public CSMG_CHAR_SELECT()
        {
            offset = 2;
        }

        public byte Slot => GetByte(2);

        public override Packet New()
        {
            return new CSMG_CHAR_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnCharSelect(this);
        }
    }
}