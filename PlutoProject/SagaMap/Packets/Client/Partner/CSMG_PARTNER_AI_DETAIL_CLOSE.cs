using SagaLib;

namespace SagaMap.Packets.Client.Partner
{
    public class CSMG_PARTNER_AI_DETAIL_CLOSE : Packet
    {
        public CSMG_PARTNER_AI_DETAIL_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_AI_DETAIL_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((MapClient)(client)).NetIo.Disconnect();
        }
    }
}