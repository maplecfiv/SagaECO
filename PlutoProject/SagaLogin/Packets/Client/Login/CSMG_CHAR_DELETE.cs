using System.Text;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_CHAR_DELETE : Packet
    {
        public CSMG_CHAR_DELETE()
        {
            offset = 2;
        }

        public byte Slot => GetByte(2);

        public string DeletePassword
        {
            get
            {
                var size = GetByte(3);
                size--;
                return Encoding.ASCII.GetString(GetBytes(size, 4));
            }
        }

        public override Packet New()
        {
            return new CSMG_CHAR_DELETE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnCharDelete(this);
        }
    }
}