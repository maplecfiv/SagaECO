using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Another
{
    public class CSMG_ANO_PAPER_TAKEOFF : Packet
    {
        public CSMG_ANO_PAPER_TAKEOFF()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ANO_PAPER_TAKEOFF();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAnoPaperTakeOff(this);
        }
    }
}