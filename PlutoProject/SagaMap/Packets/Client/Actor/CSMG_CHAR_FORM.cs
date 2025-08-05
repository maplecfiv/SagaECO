using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_CHAR_FORM : Packet
    {
        public CSMG_CHAR_FORM()
        {
            offset = 2;
        }

        public byte tailstyle => GetByte(2);

        public byte wingstyle => GetByte(3);

        public byte wingcolor => GetByte(4);

        public override Packet New()
        {
            return new CSMG_CHAR_FORM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnCharFormChange(this);
        }
    }
}