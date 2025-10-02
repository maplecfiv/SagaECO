using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client
{
    public class CSMG_SEND_VERSION : Packet
    {
        public CSMG_SEND_VERSION()
        {
            size = 10;
            offset = 2;
        }

        public string GetVersion()
        {
            byte[] buf = GetBytes(6, 4);
            return Conversions.bytes2HexString(buf);
        }

        public override Packet New()
        {
            return (Packet)new CSMG_SEND_VERSION();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((LoginClient)(client)).NewOnSendVersion(this);
            ((ValidationClient)(client)).OnSendVersion(this);
        }

    }
}
