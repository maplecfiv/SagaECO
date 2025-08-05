using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Login
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
            var buf = GetBytes(6, 4);
            return Conversions.bytes2HexString(buf);
        }

        public override Packet New()
        {
            return new CSMG_SEND_VERSION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSendVersion(this);
        }
    }
}